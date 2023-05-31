using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static GameLibraryV2.Helper.Enums;

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
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            if (await userRepository.HasEmailAsync(userCreate.Email))
                return BadRequest("User with Email already registrated");

            if (await userRepository.HasNicknameAsync(userCreate.Nickname))
                return BadRequest("User with this Nickname already exists");

            if (!Enum.GetNames(typeof(Genders)).Contains(userCreate.Gender.ToLower()))
                return BadRequest("Unsupported Gender");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = new User
            {
                Email = userCreate.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userCreate.Password),
                Nickname = userCreate.Nickname,
                Age = userCreate.Age,
                Gender = userCreate.Gender,
                PicturePath = $"/uploads/userPicture/Def.jpg",
                RegistrationdDate = DateOnly.FromDateTime(DateTime.Now),
                UserGames = new List<PersonGame>() { },
                UserRoles = new List<Role>() { await roleRepository.GetRoleByNameAsync(Enums.Roles.user.ToString()) },
                UserFriends = new List<Friend>() { },
            };

            userRepository.CreateUser(userMap);
            await userRepository.SaveUserAsync();

            return Ok("Successfully created");
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (userLogin == null) 
                return BadRequest(ModelState);

            if(!await userRepository.UserExistsByNicknameAsync(userLogin.Nickname))
                return NotFound($"Not found user with such nickname {userLogin.Nickname}");

            var user = await userRepository.GetUserByNicknameAsync(userLogin.Nickname);

            if(!ValidatePassword(userLogin.Password, user.Password))
                return BadRequest("Wrong Password");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            return Ok(token);
        }

        [HttpPost("/refreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] int userId)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            var user = await userRepository.GetUserByIdAsync(userId);

            if (!user.RefreshToken.Equals(refreshToken))
                return Unauthorized("Invalid Refresh Token");
            else if(user.TokenExpires < DateTime.Now)
                return Unauthorized("Token expired");

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            return Ok(token);
        }


        private static RefreshToken GenerateRefreshToken()
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
            List<Claim> claims = new List<Claim>();
            foreach (var item in user.UserRoles)
            {
                if (Enum.GetNames(typeof(Roles)).Contains(item.RoleName.Trim().ToLower()))
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.RoleName.Trim().ToLower()));
                }
            }

            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Nickname));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(555),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private bool ValidatePassword(string loginPassword, string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(loginPassword, userPassword);
        }
    }
}
