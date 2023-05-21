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
        public async Task<IActionResult> CreateGameReview([FromBody] ReviewCreateDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            if (!await gameRepository.GameExistsAsync(reviewCreate.GameId))
                return NotFound($"Not found game with such id {reviewCreate.GameId}");

            if (!await userRepository.UserExistsByIdAsync(reviewCreate.UserId))
                return NotFound($"Not found user with such id {reviewCreate.UserId}");

            if (!await personGamesRepository.PersonGameExistsAsync(reviewCreate.UserId, reviewCreate.GameId))
                return NotFound($"Not found in person Game");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = mapper.Map<Review>(reviewCreate);

            reviewMap.Rating = (await personGamesRepository.GetPersonGameByUserIdAndGameIdAsync(reviewCreate.UserId, reviewCreate.GameId)).Score;
            reviewMap.User = await userRepository.GetUserByIdAsync(reviewCreate.UserId);
            reviewMap.Game = await gameRepository.GetGameByIdAsync(reviewCreate.GameId);

            reviewRepository.CreateReview(reviewMap);
            await reviewRepository.SaveReviewAsync();

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
        public async Task<IActionResult> UpdateGameReview([FromBody] ReviewUpdate reviewUpdate)
        {
            if (reviewUpdate == null)
                return BadRequest(ModelState);

            if (!await reviewRepository.ReviewExistsAsync(reviewUpdate.Id))
                return NotFound($"Not found review with such id {reviewUpdate.Id}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);  

            var review = await reviewRepository.GetReviewByIdAsync(reviewUpdate.Id);

            review.Text = reviewUpdate.Text;

            reviewRepository.UpdateReview(review);
            await reviewRepository.SaveReviewAsync();

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
        public async Task<IActionResult> UpdateRaviewRating([FromBody] ReviewRatingUpdate reviewRatingUpdate)
        {
            if (reviewRatingUpdate == null)
                return BadRequest(ModelState);

            if (!await reviewRepository.ReviewExistsAsync(reviewRatingUpdate.Id))
                return NotFound($"Not found review with such id {reviewRatingUpdate.Id}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await reviewRepository.GetReviewByIdAsync(reviewRatingUpdate.Id);

            review.ReviewRating += reviewRatingUpdate.ReviewRating;

            reviewRepository.UpdateReview(review);
            await reviewRepository.SaveReviewAsync();

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
        public async Task<IActionResult> DeleteGameReview([FromQuery] int reviewDelete)
        {
            if (!await reviewRepository.ReviewExistsAsync(reviewDelete))
                return NotFound($"Not found review with such id {reviewDelete}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await reviewRepository.GetReviewByIdAsync(reviewDelete);

            reviewRepository.DeleteReview(review);
            await reviewRepository.SaveReviewAsync();

            return Ok("Successfully deleted");
        }
    }
}
