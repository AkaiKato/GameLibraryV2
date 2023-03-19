using AutoMapper;
using GameLibraryV2.Dto;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
