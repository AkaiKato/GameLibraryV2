using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalStatisticController : ControllerBase
    {
        private readonly IGameRepository gameRepository;
        public GlobalStatisticController(IGameRepository _gameRepository)
        {
            gameRepository = _gameRepository;   
        }

        [HttpGet("numberOfGamesAndDLC")]
        public async Task<IActionResult> GetNumberOfGamesAndDLC() 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var totalNumberOfGames = await gameRepository.GetTotalNumberOfGames();
            var numberOfGames = await gameRepository.GetNumberOfGames();
            var numberOfDLC = await gameRepository.GetNumberOfDLC();

            var statNumbers = new
            {
                totalNumberOfGames,
                numberOfGames,
                numberOfDLC
            };

            return Ok(statNumbers);
        }

    }
}
