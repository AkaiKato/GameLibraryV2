using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GameLibraryV2.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IConfiguration configuration;

        public AuthController(IUserRepository _userRepository, 
            IRoleRepository _roleRepository,
            IConfiguration _configuration)
        {
            userRepository = _userRepository;
            roleRepository = _roleRepository;
            configuration = _configuration;
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody] UserCreateDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            if (userRepository.HasEmail(userCreate.Email))
            {
                ModelState.AddModelError("", "User with Email already registrated");
                return StatusCode(422, ModelState);
            }

            if (userRepository.HasNickname(userCreate.Nickname))
            {
                ModelState.AddModelError("", "User with this Nickname already exists");
                return StatusCode(422, ModelState);
            }

            if (userCreate.Gender.Trim().ToLower() != Enums.Genders.male.ToString() &&
                userCreate.Gender.Trim().ToLower() != Enums.Genders.female.ToString())
            {
                ModelState.AddModelError("", "Unsupported Gender");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = new User();

            userMap.Email = userCreate.Email;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userCreate.Password);
            userMap.Password = passwordHash;

            userMap.Nickname = userCreate.Nickname;
            userMap.Age = userCreate.Age;
            userMap.Gender = userCreate.Gender;

            userMap.PicturePath = $"\\Images\\userPicture\\Def.jpg";
            userMap.RegistrationdDate = DateTime.Now;
            userMap.UserGames = new List<PersonGame>() { };
            userMap.UserRoles = new List<Role>() { roleRepository.GetRoleByName(Enums.Roles.user.ToString()) };
            userMap.UserFriends = new List<Friend>() { };

            if (!userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            if (userLogin == null) 
                return BadRequest();

            if(!userRepository.UserExistsByNickname(userLogin.Nickname))
                return NotFound();

            var user = userRepository.GetUserByNickname(userLogin.Nickname);

            if(!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
                return BadRequest("Wrong Password");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(token);
        }

        [HttpPost("/refreshToken")]
        public IActionResult RefreshToken(JustIdDto userId)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(!userRepository.UserExistsById(userId.Id))
            {
                return NotFound();
            }
            var user = userRepository.GetUserById(userId.Id);

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(token);
        }


        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault()!.RoleName),
                new Claim(ClaimTypes.Name, user.Nickname)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
