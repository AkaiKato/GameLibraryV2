using AutoMapper;
using GameLibraryV2.Dto;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Repositories;
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
            if (!gameRepository.GameExists(gameId))
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
            if (!gameRepository.GameExists(gameId))
                return NotFound();

            var Review = mapper.Map<List<ReviewDto>>(gameRepository.GetGameReviews(gameId));

            if (!ModelState.IsValid)
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
            if (!gameRepository.GameExists(gameId))
                return NotFound();

            var PicturePath = gameRepository.GetGamePicturePath(gameId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PicturePath));
        }

        /// <summary>
        /// Create new Game
        /// </summary>
        /// <param name="DeveloperIds"></param>
        /// <param name="PublisherIds"></param>
        /// <param name="PlatformIds"></param>
        /// <param name="GenreIds"></param>
        /// <param name="TagIds"></param>
        /// <param name="gameCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGame([FromQuery] int[] DeveloperIds, [FromQuery] int[] PublisherIds, [FromQuery] int[] PlatformIds, [FromQuery] int[] GenreIds, [FromQuery] int[] TagIds, [FromBody] GameCreateDto gameCreate)
        {
            if (gameCreate == null)
                return BadRequest(ModelState);

            Console.WriteLine(DeveloperIds);

            if (gameRepository.GetGameByName(gameCreate.Name) != null)
            {
                ModelState.AddModelError("", "Game with this name already exists");
                return StatusCode(422, ModelState);
            }

            if (gameCreate.Type.Trim().ToLower() != "game" && gameCreate.Type.Trim().ToLower() != "dlc")
            {
                ModelState.AddModelError("", "Unsupported type");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var gameMap = mapper.Map<Game>(gameCreate);

            gameMap.PicturePath = $"\\Images\\gamePicture\\Def";
            gameMap.Reviews = new List<Review>();
            gameMap.DLCs = new List<DLC>();
            gameMap.Rating = new Rating();

            if (!gameRepository.CreateGame(DeveloperIds, PublisherIds, PlatformIds, GenreIds, TagIds, gameMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Change Game Picture
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadGamePicture([FromQuery] int gameId,IFormFile pic)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("", "Unsupported extension");
                return StatusCode(422, ModelState);
            }

            if (!gameRepository.GameExists(gameId))
                return NotFound();

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var game = gameRepository.GetGameById(gameId);
            var newfilePath = $"\\Images\\gamePicture\\{unique}{ext}";
            var oldfilePath = game.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            if (!gameRepository.SaveGamePicturePath(gameId, newfilePath))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\gamePicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully created");
        }

    }
}
