using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : Controller
    {
        private readonly int numberOfScores = 1;
        private readonly IRatingRepository ratingRepository;
        private readonly IGameRepository gameRepository;
        public RatingController(IRatingRepository _ratingRepository, IGameRepository _gameRepository)
        {
            ratingRepository = _ratingRepository;
            gameRepository = _gameRepository;
        }

        /// <summary>
        /// Get all ratings
        /// </summary>
        /// <returns></returns>
        [HttpGet("ratings")]
        [ProducesResponseType(200, Type = typeof(IList<Rating>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await ratingRepository.GetRatingsAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ratings);
        }

        /// <summary>
        /// Get specified game rating
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("gameRating")]
        [ProducesResponseType(200, Type = typeof(Rating))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGameRating(int gameId)
        {
            if (!await gameRepository.GameExistsAsync(gameId))
                return NotFound($"Not found game with such id {gameId}");

            var game = await gameRepository.GetGameByIdAsync(gameId);

            if (!await ratingRepository.RatingExistsAsync(game.Rating.Id))
                return NotFound($"Not found rating with such id {game.Rating.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rating = await ratingRepository.GetRatingByIdAsync(game.Rating.Id);

            return Ok(rating);
        }

        /// <summary>
        /// Update game Rating
        /// </summary>
        /// <param name="ratingUpdate"></param>
        /// <returns></returns>
        [HttpPut("gameRatingAdd")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateGameRating(RatingUpdate ratingUpdate)
        {
            if (ratingUpdate == null)
                return BadRequest(ModelState);

            if(!await ratingRepository.RatingExistsAsync(ratingUpdate.RatingId))
                return NotFound($"Not found rating with such id {ratingUpdate.RatingId}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var rating = await ratingRepository.GetRatingByIdAsync(ratingUpdate.RatingId);

            ratingRepository.Add(rating, ratingUpdate.Rating%10);

            ratingRepository.UpdateRating(rating);
            await ratingRepository.SaveRatingAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update to null
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        [HttpPut("gameRatingNulling")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteGameRating(int ratingId)
        {
            if(!await ratingRepository.RatingExistsAsync(ratingId))
                return NotFound($"Not found rating with such id {ratingId}");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var rating = await ratingRepository.GetRatingByIdAsync(ratingId);

            rating.TotalRating = 0;
            rating.NumberOfOne = 0;
            rating.NumberOfTwo = 0;
            rating.NumberOfThree = 0;
            rating.NumberOfFour = 0;
            rating.NumberOfFive = 0;
            rating.NumberOfSix = 0;
            rating.NumberOfSeven = 0;
            rating.NumberOfEight = 0;
            rating.NumberOfNine = 0;
            rating.NumberOfTen = 0;

            ratingRepository.UpdateRating(rating);
            await ratingRepository.SaveRatingAsync();

            return Ok("Successfully nulled");
        }

        [HttpGet("/tr")]
        public async Task<IActionResult> TotalRatingCalculation()
        {
            var gamesRatings = await ratingRepository.GetRatingsAsync();

            double avg = Math.Round((double)gamesRatings.Average(x =>
            {
                int count = CalcCountOfNumbers(x);

                if (count < numberOfScores)
                    return null;

                return (CalcSumOfNumbers(x) / count); 
            })!, 2);

            foreach (var gR in gamesRatings)
            {
                double currentNumbers = CalcCountOfNumbers(gR);

                if (currentNumbers < numberOfScores)
                    continue;

                double currentRating = CalcSumOfNumbers(gR);

                double cR = Math.Round(currentRating / currentNumbers, 2);

                gR.TotalRating = Math.Round(currentNumbers / (currentNumbers + numberOfScores) * cR + 
                    numberOfScores / (currentNumbers + numberOfScores) * avg,2);

                ratingRepository.UpdateRating(gR);
            }

            await ratingRepository.SaveRatingAsync();

            return Ok();
        }

        private static int CalcCountOfNumbers(Rating x)
        {
            return x.NumberOfTen + x.NumberOfNine + x.NumberOfEight + x.NumberOfSeven
                + x.NumberOfSix + x.NumberOfFive + x.NumberOfFour + x.NumberOfThree
                + x.NumberOfTwo + x.NumberOfOne;
        }

        private static int CalcSumOfNumbers(Rating x)
        {
            return 10 * x.NumberOfTen + 9 * x.NumberOfNine + 8 * x.NumberOfEight +
                7 * x.NumberOfSeven + 6 * x.NumberOfSix + 5 * x.NumberOfFive +
                4 * x.NumberOfFour + 3 * x.NumberOfThree + 2 * x.NumberOfTwo + x.NumberOfOne;
        }

    }
}
