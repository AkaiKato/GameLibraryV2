using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
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
        private readonly IPersonGamesRepository personGamesRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public UserController(IUserRepository _userRepository,
            IRoleRepository _roleRepository,
            IFriendRepository _friendRepository,
            IPersonGamesRepository _personGamesRepository,
            IReviewRepository _reviewRepository,
            IMapper _mapper)
        {
            userRepository = _userRepository;
            roleRepository = _roleRepository;
            friendRepository = _friendRepository;
            personGamesRepository = _personGamesRepository;
            reviewRepository = _reviewRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("userAll")]
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

            var FavouriteGames = personGamesRepository.GetPersonFavouriteGame(userId).Take(5);

            var PublisherStatistic = personGamesRepository.GetPersonPublisherStatistic(userId).Take(5);

            var TagStatistic = personGamesRepository.GetPersonTagStatistic(userId).Take(5);

            var DeveloperStatistic = personGamesRepository.GetPersonDeveloperStatistic(userId).Take(5);

            var PlatformStatistic = personGamesRepository.GetPersonPlatformStatistic(userId);

            var UserReviews = mapper.Map<List<ReviewDto>>(reviewRepository.GetUserReviews(userId));

            var userPage = new
            {
                User,
                FavouriteGames,
                PublisherStatistic,
                TagStatistic,
                DeveloperStatistic,
                PlatformStatistic,
                UserReviews,
            };

            return Ok(userPage);
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

            if(!Enum.GetNames(typeof(Genders)).Contains(userUpdate.Gender))
                return BadRequest("Unsupported Gender");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(userUpdate.Id);

            user.Email = userUpdate.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password);
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
