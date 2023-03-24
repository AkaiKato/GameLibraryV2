using AutoMapper;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        public readonly IReviewRepository reviewRepository;
        public readonly IGameRepository gameRepository;
        public readonly IUserRepository userRepository;
        public readonly IMapper mapper;
        public ReviewController(IReviewRepository _reviewRepository, 
            IGameRepository _gameRepository, 
            IUserRepository _userRepository,
            IMapper _mapper)
        {
            reviewRepository = _reviewRepository;
            gameRepository = _gameRepository;
            userRepository = _userRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Creates review
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="GameId"></param>
        /// <param name="reviewCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGameReview([FromQuery] int UserId, [FromQuery] int GameId, [FromBody] ReviewCreateDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest();

            if (!gameRepository.GameExists(GameId))
                return NotFound(ModelState);

            if (!userRepository.UserExists(UserId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = mapper.Map<Review>(reviewCreate);

            reviewMap.User = userRepository.GetUserById(UserId);
            reviewMap.Game = gameRepository.GetGameById(GameId);

            if (!reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
