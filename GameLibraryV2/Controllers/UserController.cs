using AutoMapper;
using GameLibraryV2.Dto;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserController(IUserRepository _userRepository, IMapper _mapper)
        {
            userRepository = _userRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
            if (!userRepository.UserExists(userId))
                return NotFound();

            var User = mapper.Map<UserDto>(userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(User));
        }

        /// <summary>
        /// Return specified user friends
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/friends")]
        [ProducesResponseType(200, Type = typeof(IList<FriendDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserFriends(int userId)
        {
            if (!userRepository.UserExists(userId))
                return NotFound();

            var UserFriends = mapper.Map<List<FriendDto>>(userRepository.GetUserFriends(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(UserFriends));
        }

        /// <summary>
        /// Return specified user picture
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/picture")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetUserPicture(int userId)
        {
            if (!userRepository.UserExists(userId))
                return NotFound();

            var PicturePath = userRepository.GetUserPicturePath(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PicturePath));
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
            if (!userRepository.UserExists(userId))
                return NotFound();

            var UserRole = mapper.Map<List<RoleDto>>(userRepository.GetUserRole(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(UserRole));
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

            if (userCreate.Gender.Trim().ToLower() != "male" && userCreate.Gender.Trim().ToLower() != "woman")
            {
                ModelState.AddModelError("", "Unsupported Gender");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = mapper.Map<User>(userCreate);

            userMap.PicturePath = $"\\Images\\gamePicture\\Def";
            userMap.Library = new Library();
            userMap.UserFriends = new List<Friend>();

            if (!userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Change User Picture
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadGamePicture([FromQuery] int userId, IFormFile pic)
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

            if (!userRepository.UserExists(userId))
                return NotFound();

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var game = userRepository.GetUserById(userId);
            var newfilePath = $"\\Images\\userPicture\\{unique}{ext}";
            var oldfilePath = game.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            if (!userRepository.SaveUserPicturePath(userId, newfilePath))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\userPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }


            return Ok("Successfully created");
        }
    }
}
