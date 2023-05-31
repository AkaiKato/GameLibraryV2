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
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Users = mapper.Map<List<UserDto>>(await userRepository.GetUsersAsync());

            return Ok(Users);
        }

        /// <summary>
        /// Return specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int userId)
        {
            if (!await userRepository.UserExistsByIdAsync(userId))
                return NotFound("Not found user with such id");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var User = mapper.Map<UserDto>(await userRepository.GetUserByIdAsync(userId));

            var FavouriteGames = (await personGamesRepository.GetPersonFavouriteGameAsync(userId)).Take(5);

            var PublisherStatistic = (await personGamesRepository.GetPersonPublisherStatisticAsync(userId)).Take(5);

            var TagStatistic = (await personGamesRepository.GetPersonTagStatisticAsync(userId)).Take(5);

            var DeveloperStatistic = (await personGamesRepository.GetPersonDeveloperStatisticAsync(userId)).Take(5);

            var PlatformStatistic = await personGamesRepository.GetPersonPlatformStatisticAsync(userId);

            var UserReviews = mapper.Map<List<ReviewDto>>(await reviewRepository.GetUserReviewsAsync(userId));

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
        /// Return role specified user 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/role")]
        public async Task<IActionResult> GetUserRole(int userId)
        {
            if (!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserRole = mapper.Map<List<RoleDto>>(await roleRepository.GetUserRoleAsync(userId));

            return Ok(UserRole);
        }

        /// <summary>
        /// Update specified user
        /// </summary>
        /// <param name="userUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateUser")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserUpdate userUpdate)
        {
            if (userUpdate == null)
                return BadRequest(ModelState);

            if (!await userRepository.UserExistsByIdAsync(userUpdate.Id))
                return NotFound($"Not found user with such id {userUpdate.Id}");

            if (await userRepository.UserEmailAlreadyInUseAsync(userUpdate.Id, userUpdate.Email))
                return BadRequest("Email already in use");

            if (await userRepository.UserNicknameAlreadyInUserAsync(userUpdate.Id, userUpdate.Nickname))
                return BadRequest("Nickname already in use");

            if (!Enum.GetNames(typeof(Genders)).Contains(userUpdate.Gender))
                return BadRequest("Unsupported Gender");

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var user = await userRepository.GetUserByIdAsync(userUpdate.Id);

            user.Email = userUpdate.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password);
            user.Nickname = userUpdate.Nickname;
            user.Age = userUpdate.Age;
            user.Gender = userUpdate.Gender;

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified user
        /// </summary>
        /// <param name="userDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser([FromQuery] int userDelete) 
        {
            if (!await userRepository.UserExistsByIdAsync(userDelete))
                return NotFound($"Not found user with such id {userDelete}");

            var user = await userRepository.GetUserByIdAsync(userDelete);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fusers = await friendRepository.GetUserFriendsAsync(userDelete);

            if (fusers.Count > 0)
            {
                foreach (var item in fusers)
                {
                    friendRepository.DeleteFriend(item);
                }
                await friendRepository.SaveFriendAsync();
            }

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            if (user.PicturePath != $"/uploads/userPicture/Def.jpg")
            {
                FileInfo f = new(ttt!.FullName + user.PicturePath);
                f.Delete();
            }

            userRepository.DeleteUser(user);
            await userRepository.SaveUserAsync();

            return Ok("Successfully deleted");
        }
    }
}
