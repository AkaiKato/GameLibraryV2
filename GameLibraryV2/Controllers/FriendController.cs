using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : Controller
    {
        private readonly IFriendRepository friendRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public FriendController(IFriendRepository _friendRepository, IUserRepository _userRepository,IMapper _mapper)
        {
            friendRepository = _friendRepository;
            userRepository = _userRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return specified user friends
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/friends")]
        [ProducesResponseType(200, Type = typeof(IList<FriendDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserFriends(int userId)
        {
            if (!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserFriends = mapper.Map<List<FriendDto>>(await friendRepository.GetUserFriendsAsync(userId));

            return Ok(UserFriends);
        }

        /// <summary>
        /// Add Friend
        /// </summary>
        /// <param name="addFriend"></param>
        /// <returns></returns>
        [HttpPut("addFriend")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddFriend([FromBody] FriendUpdate addFriend)
        {
            if (!await userRepository.UserExistsByIdAsync(addFriend.UserId))
                return NotFound($"Not found user with such id {addFriend.UserId}");

            if (!await userRepository.UserExistsByIdAsync(addFriend.FriendId))
                return NotFound($"Not found user with such id {addFriend.FriendId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.GetUserByIdAsync(addFriend.UserId);
            var fusers = await friendRepository.GetUserFriendsAsync(addFriend.UserId);

            if (fusers.Any(u => u.Friendu.Id == addFriend.FriendId))
                return BadRequest("Already friends");

            var friend = await userRepository.GetUserByIdAsync(addFriend.FriendId);

            friendRepository.CreateFriend(new Friend { Friendu = friend, User = user });
            await friendRepository.SaveFriendAsync();

            return Ok("Successfully added");
        }

        /// <summary>
        /// Delete friend
        /// </summary>
        /// <param name="friendConnectionId"></param>
        /// <returns></returns>
        [HttpDelete("deleteFriend")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteFriend([FromQuery] int friendConnectionId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await friendRepository.FriendExists(friendConnectionId))
                return NotFound();

            var friend = await friendRepository.GetFriendAsync(friendConnectionId);

            friendRepository.DeleteFriend(friend);
            await friendRepository.SaveFriendAsync();

            return Ok("Successfully deleted");
        }
    }
}
