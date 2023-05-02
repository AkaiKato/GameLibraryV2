
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

        [HttpGet("{userId}/publishers")]
        public IActionResult GetUserPublisherStatistics(int userId) 
        {
            if(userRepository.GetUserById(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = personGamesRepository.GetPersonPublisherStatistic(userId);

            return Ok(games);
        }

        [HttpGet("{userId}/tags")]
        public IActionResult GetUserTagStatistics(int userId)
        {
            if (userRepository.GetUserById(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = personGamesRepository.GetPersonTagStatistic(userId);

            return Ok(games);
        }

        [HttpGet("{userId}/developer")]
        public IActionResult GetUserDeveloperStatistics(int userId)
        {
            if (userRepository.GetUserById(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = personGamesRepository.GetPersonDeveloperStatistic(userId);

            return Ok(games);
        }

        [HttpGet("{userId}/platforms")]
        public IActionResult GetUserPlatformStatistics(int userId)
        {
            if (userRepository.GetUserById(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = personGamesRepository.GetPersonPlatformStatistic(userId);

            return Ok(games);
        }

        [HttpGet("{userId}/genres")]
        public IActionResult GetUserGenreStatistics(int userId)
        {
            if (userRepository.GetUserById(userId) == null)
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = personGamesRepository.GetPersonGenreStatistic(userId);

            return Ok(games);

        }
    }
}
