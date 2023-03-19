using AutoMapper;
using GameLibraryV2.Dto;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public GameController(IGameRepository _gameRepository, IMapper _mapper)
        {
            gameRepository = _gameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all games
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<GameListDto>))]
        public IActionResult GetGames()
        {
            var Games = mapper.Map<List<GameListDto>>(gameRepository.GetGames());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }

        /// <summary>
        /// Return specified game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        [ProducesResponseType(400)]
        public IActionResult GetGameById(int gameId)
        {
            if(!gameRepository.GameExists(gameId))
                return NotFound();

            var Game = mapper.Map<GameDto>(gameRepository.GetGameById(gameId));
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Game));
        }

        /// <summary>
        /// Return specified game review
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}/review")]
        [ProducesResponseType(200, Type = typeof(IList<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetGameReview(int gameId)
        {
            if(!gameRepository.GameExists(gameId))
                return NotFound();

            var Review = mapper.Map<List<ReviewDto>>(gameRepository.GetGameReviews(gameId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Review));
        }

        /// <summary>
        /// Return specified game picture
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}/picture")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetGamePicture(int gameId)
        {
            if(!gameRepository.GameExists(gameId))
                return NotFound();

            var PicturePath = gameRepository.GetGamePicturePath(gameId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PicturePath));
        }

    }
}
