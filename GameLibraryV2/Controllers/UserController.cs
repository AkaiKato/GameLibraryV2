using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GameLibraryV2.Helper.Enums;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IFriendRepository friendRepository;
        private readonly IMapper mapper;

        public UserController(IUserRepository _userRepository,
            IRoleRepository _roleRepository,
            IFriendRepository _friendRepository,
            IMapper _mapper)
        {
            userRepository = _userRepository;
            roleRepository = _roleRepository;
            friendRepository = _friendRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(IList<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Users = mapper.Map<List<UserDto>>(userRepository.GetUsers());

            return Ok(Users);
        }

        /// <summary>
        /// Return specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUserById(int userId)
        {
            if (!userRepository.UserExistsById(userId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var User = mapper.Map<UserDto>(userRepository.GetUserById(userId));

            return Ok(User);
        }

        /// <summary>
        /// Creates new User
        /// </summary>
        /// <param name="userCreate"></param>
        /// <returns></returns>
        /*[HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserCreateDto userCreate)
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
            userMap.UserRoles = new List<Role>() { roleRepository.GetRoleByName(Enums.Roles.user.ToString())};
            userMap.UserFriends = new List<Friend>() { };

            if (!userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }*/

        /// <summary>
        /// Return specified user role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/role")]
        [ProducesResponseType(200, Type = typeof(IList<RoleDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserRole(int userId)
        {
            if (!userRepository.UserExistsById(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserRole = mapper.Map<List<RoleDto>>(roleRepository.GetUserRole(userId));

            return Ok(UserRole);
        }

        /// <summary>
        /// Update specified user
        /// </summary>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUserInfo([FromBody] UserUpdate userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            if (!userRepository.UserExistsById(userUpdate.Id))
                return NotFound($"Not found user with such id {userUpdate.Id}");

            if(userRepository.UserEmailAlreadyInUse(userUpdate.Id, userUpdate.Email))
                return BadRequest("Email already in use");

            if(userRepository.UserNicknameAlreadyInUser(userUpdate.Id, userUpdate.Nickname))
                return BadRequest("Nickname already in use");

            if (userUpdate.Gender.Trim().ToLower() != Enums.Genders.male.ToString() && 
                userUpdate.Gender.Trim().ToLower() != Enums.Genders.female.ToString())
            {
                return BadRequest("Unsupported Gender");
            }

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(userUpdate.Id);

            user.Email = userUpdate.Email;
            user.Password = userUpdate.Password;
            user.Nickname = userUpdate.Nickname;
            user.Age = userUpdate.Age;
            user.Gender = userUpdate.Gender;

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified user
        /// </summary>
        /// <param name="userDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteUser([FromBody] JustIdDto userDelete) 
        {
            if (!userRepository.UserExistsById(userDelete.Id))
                return NotFound($"Not found user with such id {userDelete.Id}");

            var user = userRepository.GetUserById(userDelete.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fusers = friendRepository.GetUserFriends(userDelete.Id).ToList();

            if (fusers.Count > 0)
            {
                foreach (var item in fusers)
                {
                    if (!friendRepository.DeleteFriend(item))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            if (user.PicturePath != $"\\Images\\userPicture\\Def.jpg")
            {
                FileInfo f = new(user.PicturePath);
                f.Delete();
            }

            if (!userRepository.DeleteUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
