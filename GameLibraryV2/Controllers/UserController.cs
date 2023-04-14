using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var Users = mapper.Map<List<UserDto>>(userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Users));
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

            var User = mapper.Map<UserDto>(userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(User));
        }

        /// <summary>
        /// Creates new User
        /// </summary>
        /// <param name="userCreate"></param>
        /// <returns></returns>
        [HttpPost]
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
        }

        /// <summary>
        /// Return all users by specified role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}/users")]
        [ProducesResponseType(200, Type = typeof(IList<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetRoleUsers(int roleId)
        {
            if (!roleRepository.RoleExists(roleId))
                return NotFound();

            var User = mapper.Map<List<UserDto>>(userRepository.GetUsersByRole(roleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(User));
        }

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
                return NotFound();

            var UserRole = mapper.Map<List<RoleDto>>(roleRepository.GetUserRole(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(UserRole));
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
                return NotFound();

            if(userRepository.UserEmailAlreadyInUse(userUpdate.Id, userUpdate.Email))
            {
                ModelState.AddModelError("", $"Email already in use");
                return StatusCode(422, ModelState);
            }

            if(userRepository.UserNicknameAlreadyInUser(userUpdate.Id, userUpdate.Nickname))
            {
                ModelState.AddModelError("", $"Nickname already in use");
                return StatusCode(422, ModelState);
            }

            if (userUpdate.Gender.Trim().ToLower() != Enums.Genders.male.ToString() && 
                userUpdate.Gender.Trim().ToLower() != Enums.Genders.female.ToString())
            {
                ModelState.AddModelError("", "Unsupported Gender");
                return StatusCode(422, ModelState);
            }

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
        /// Add specified role to user
        /// </summary>
        /// <param name="addRole"></param>
        /// <returns></returns>
        [HttpPut("addRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddRole([FromBody] RoleUpdate addRole)
        {
            if (!userRepository.UserExistsById(addRole.UserId))
                return NotFound(ModelState);

            if (!roleRepository.RoleExists(addRole.RoleId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(addRole.UserId);
            var role = roleRepository.GetRoleById(addRole.RoleId);
            user.UserRoles.Add(role);

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added");
        }

        /// <summary>
        /// Update User Picture
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadUserPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadUserPicture([FromQuery] int userId, IFormFile pic)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("", "Unsupported extension");
                return StatusCode(422, ModelState);
            }

            if (!userRepository.UserExistsById(userId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var user = userRepository.GetUserById(userId);
            var newfilePath = $"\\Images\\userPicture\\{unique}{ext}";
            var oldfilePath = user.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            user!.PicturePath = newfilePath;

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\userPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
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
                return NotFound();

            var user = userRepository.GetUserById(userDelete.Id);

            if (!ModelState.IsValid)
                return BadRequest();

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

        /// <summary>
        /// Delete specified role of user 
        /// </summary>
        /// <param name="deleteRole"></param>
        /// <returns></returns>
        [HttpDelete("deleteRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteRole([FromBody] RoleUpdate deleteRole)
        {
            if (!userRepository.UserExistsById(deleteRole.UserId))
                return NotFound(ModelState);

            if (!roleRepository.RoleExists(deleteRole.RoleId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(deleteRole.UserId);
            var fusers = roleRepository.GetUserRole(deleteRole.UserId).ToList();

            if (fusers.Count <= 1)
            {
                ModelState.AddModelError("", "User can't have less then one role" +
                    " Or user doesn't have this role");
                return StatusCode(422, ModelState);
            }

            fusers.RemoveAll(x => x.Id == deleteRole.RoleId);

            user.UserRoles = fusers;

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
