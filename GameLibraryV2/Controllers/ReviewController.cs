using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
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
        public readonly IPersonGamesRepository personGamesRepository;
        public readonly IMapper mapper;
        public ReviewController(IReviewRepository _reviewRepository, 
            IGameRepository _gameRepository, 
            IUserRepository _userRepository,
            IPersonGamesRepository _personGamesRepository,
            IMapper _mapper)
        {
            reviewRepository = _reviewRepository;
            gameRepository = _gameRepository;
            userRepository = _userRepository;
            personGamesRepository = _personGamesRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Creates review
        /// </summary>
        /// <param name="reviewCreate"></param>
        /// <returns></returns>
        [HttpPost("createReview")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGameReview([FromBody] ReviewCreateDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            if (!gameRepository.GameExists(reviewCreate.GameId))
                return NotFound($"Not found game with such id {reviewCreate.GameId}");

            if (!userRepository.UserExistsById(reviewCreate.UserId))
                return NotFound($"Not found user with such id {reviewCreate.UserId}");

            if (!personGamesRepository.PersonGameExists(reviewCreate.UserId, reviewCreate.GameId))
                return NotFound($"Not found in person Game");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = mapper.Map<Review>(reviewCreate);

            reviewMap.Rating = personGamesRepository.GetPersonGameByUserIdAndGameId(reviewCreate.UserId, reviewCreate.GameId).Score;
            reviewMap.User = userRepository.GetUserById(reviewCreate.UserId);
            reviewMap.Game = gameRepository.GetGameById(reviewCreate.GameId);

            if (!reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified review
        /// </summary>
        /// <param name="reviewUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateReview")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateGameReview([FromBody] ReviewUpdate reviewUpdate)
        {
            if(reviewUpdate == null)
                return BadRequest(ModelState);

            if (!reviewRepository.ReviewExists(reviewUpdate.Id))
                return NotFound($"Not found review with such id {reviewUpdate.Id}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);  

            var review = reviewRepository.GetReviewById(reviewUpdate.Id);

            review.Text = reviewUpdate.Text;

            if (!reviewRepository.UpdateReview(review))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update ReviewRating specified review
        /// </summary>
        /// <param name="reviewRatingUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateReviewRating")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRaviewRatingGameReview([FromBody] ReviewRatingUpdate reviewRatingUpdate)
        {
            if (reviewRatingUpdate == null)
                return BadRequest(ModelState);

            if (!reviewRepository.ReviewExists(reviewRatingUpdate.Id))
                return NotFound($"Not found review with such id {reviewRatingUpdate.Id}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = reviewRepository.GetReviewById(reviewRatingUpdate.Id);

            review.ReviewRating += reviewRatingUpdate.ReviewRating;

            if (!reviewRepository.UpdateReview(review))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified review
        /// </summary>
        /// <param name="reviewDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteReview")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteGameReview([FromBody] JustIdDto reviewDelete)
        {
            if (reviewDelete == null)
                return BadRequest(ModelState);

            if (!reviewRepository.ReviewExists(reviewDelete.Id))
                return NotFound($"Not found review with such id {reviewDelete.Id}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = reviewRepository.GetReviewById(reviewDelete.Id);

            if (!reviewRepository.DeleteReview(review))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
