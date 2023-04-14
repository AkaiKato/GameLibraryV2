using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
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
        public IActionResult GetUserFriends(int userId)
        {
            if (!userRepository.UserExistsById(userId))
                return NotFound();

            var UserFriends = mapper.Map<List<FriendDto>>(friendRepository.GetUserFriends(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(UserFriends));
        }

        /// <summary>
        /// Add Friend
        /// </summary>
        /// <param name="addFriend"></param>
        /// <returns></returns>
        [HttpPut("addFriend")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddFriend([FromBody] FriendUpdate addFriend)
        {
            if (!userRepository.UserExistsById(addFriend.UserId))
                return NotFound(ModelState);

            if (!userRepository.UserExistsById(addFriend.FriendId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(addFriend.UserId);
            var fusers = friendRepository.GetUserFriends(addFriend.UserId);

            if (fusers.Any(u => u.Friendu.Id == addFriend.FriendId))
            {
                ModelState.AddModelError("", "Already friends");
                return StatusCode(500, ModelState);
            }

            var friend = userRepository.GetUserById(addFriend.FriendId);

            if (!friendRepository.CreateFriend(new Friend { Friendu = friend, User = user }))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added");
        }

        /// <summary>
        /// Delete friend
        /// </summary>
        /// <param name="deleteFriend"></param>
        /// <returns></returns>
        [HttpDelete("deleteFriend")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteFriend([FromBody] FriendUpdate deleteFriend)
        {
            if (!userRepository.UserExistsById(deleteFriend.UserId))
                return NotFound(ModelState);

            if (!userRepository.UserExistsById(deleteFriend.FriendId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fusers = friendRepository.GetUserFriends(deleteFriend.UserId).Where(u => u.Friendu.Id == deleteFriend.FriendId).FirstOrDefault();

            if (fusers == null)
            {
                ModelState.AddModelError("", "No such friend");
                return StatusCode(422, ModelState);
            }

            if (!friendRepository.DeleteFriend(fusers))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
