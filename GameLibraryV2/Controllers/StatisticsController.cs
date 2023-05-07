
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IPersonGamesRepository personGamesRepository;

        public StatisticsController(
            IUserRepository _userRepository,
            IPersonGamesRepository _personGamesRepository)
        {
            userRepository = _userRepository;
            personGamesRepository = _personGamesRepository;
        }

        /// <summary>
        /// Return publisherStatistic for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/publishers")]
        public async Task<IActionResult> GetUserPublisherStatistics(int userId) 
        {
            if(await userRepository.GetUserByIdAsync(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await personGamesRepository.GetPersonPublisherStatisticAsync(userId);

            return Ok(games);
        }

        /// <summary>
        /// Return tagStatisitic for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/tags")]
        public async Task<IActionResult> GetUserTagStatistics(int userId)
        {
            if (await userRepository.GetUserByIdAsync(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await personGamesRepository.GetPersonTagStatisticAsync(userId);

            return Ok(games);
        }

        /// <summary>
        /// return developerStatisitc for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/developer")]
        public async Task<IActionResult> GetUserDeveloperStatistics(int userId)
        {
            if (await userRepository.GetUserByIdAsync(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await personGamesRepository.GetPersonDeveloperStatisticAsync(userId);

            return Ok(games);
        }

        /// <summary>
        /// Return platformStatisitc for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/platforms")]
        public async Task<IActionResult> GetUserPlatformStatistics(int userId)
        {
            if (await userRepository.GetUserByIdAsync(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await personGamesRepository.GetPersonPlatformStatisticAsync(userId);

            return Ok(games);
        }

        /// <summary>
        /// Return genreStatistic for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/genres")]
        public async Task<IActionResult> GetUserGenreStatistics(int userId)
        {
            if (await userRepository.GetUserByIdAsync(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await personGamesRepository.GetPersonGenreStatisticAsync(userId);

            return Ok(games);

        }
    }
}
