using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class GlobalStatisticController : Controller
    {
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;
        public GlobalStatisticController(IGameRepository _gameRepository, IMapper _mapper)
        {
            gameRepository = _gameRepository;
            mapper = _mapper;
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

        [HttpGet("mostRatedGame")]
        public async Task<IActionResult> GetMostRatedGame()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<GameDto>(await gameRepository.GetMostRatedGame()));
        }

        [HttpGet("mostRatedDLC")]
        public async Task<IActionResult> GetMostRatedDLC()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<GameDto>(await gameRepository.GetMostRatedDLC()));
        }

        [HttpGet("getMostRatedGameByYear")]
        public async Task<IActionResult> GetMostRatedGameByYear()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = mapper.Map<List<GameDto>>(await gameRepository.GetMostRatedGameByYear());

            return Ok(games);
        }

        [HttpGet("getMostRatedDLCByYear")]
        public async Task<IActionResult> GetMostRatedDLCByYear()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = mapper.Map<List<GameDto>>(await gameRepository.GetMostRatedDLCByYear());

            return Ok(games);
        }

    }
}
