using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly IGameRepository gameRepository;
        private readonly IDeveloperRepository developerRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITagRepository tagRepository;
        private readonly IPlatformRepository platformRepository;
        private readonly IPersonGamesRepository personGamesRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IDLCRepository dlcRepository;
        private readonly IAgeRatingRepository ageRatingRepository;
        private readonly ISystemRequirements systemRequirements;
        private readonly IUserRepository userRepository;
        private readonly ILogger<Game> logger;
        private readonly IMapper mapper;

        public GameController(IGameRepository _gameRepository,
            IDeveloperRepository _developerRepository, 
            IPublisherRepository _publisherRepository, 
            IGenreRepository _genreRepository,
            ITagRepository _tagRepository,
            IPlatformRepository _platformRepository,
            IPersonGamesRepository _personGamesRepository,
            IReviewRepository _reviewRepository,
            IRatingRepository _ratingRepository,
            IDLCRepository _dlcRepository,
            IAgeRatingRepository _ageRatingRepository,
            ISystemRequirements _systemRequirements,
            IUserRepository _userRepository,
            ILogger<Game> _logger,
            IMapper _mapper)
        {
            gameRepository = _gameRepository;
            developerRepository = _developerRepository;
            publisherRepository = _publisherRepository;
            platformRepository = _platformRepository;
            personGamesRepository = _personGamesRepository;
            genreRepository = _genreRepository;
            tagRepository = _tagRepository;
            reviewRepository = _reviewRepository;
            ratingRepository = _ratingRepository;
            dlcRepository = _dlcRepository;
            ageRatingRepository = _ageRatingRepository;
            systemRequirements = _systemRequirements;
            userRepository = _userRepository;
            logger = _logger;
            mapper = _mapper;
        }

        /// <summary>
        /// Return games
        /// </summary>
        /// <returns></returns>
        [HttpGet("games")]
        [ProducesResponseType(200, Type = typeof(List<GameSmallListDto>))]
        public async Task<IActionResult> GetGames([FromQuery] FilterParameters filterParameters, [FromQuery] Pagination pagination)
        {
            if(!filterParameters.ValidYearRange)
                return BadRequest("Max release year cannot be less than min year");

            if (!filterParameters.ValidPlayTime)
                return BadRequest("Max playtime cannot be less than min playtime");

            if (!filterParameters.ValidRating)
                return BadRequest("Rating cannot be less than 0");

            if(!filterParameters.ValidStatus)
                return BadRequest("Not Valid Status");

            if (!filterParameters.ValidType)
                return BadRequest("Not Valid Type");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await gameRepository.GetGamesAsync(filterParameters, pagination);

            logger.Log(LogLevel.Information, $"Requested Path: {Request.Path}");
            logger.LogInformation(filterParameters.ToString());

            var metadata = new
            {
                games.TotalCount,
                games.PageSize,
                games.CurrentPage,
                games.TotalPages,
                games.HasNext,
                games.HasPrevious,
            };

            var Games = mapper.Map<List<GameSmallListDto>>(games);

            Response.Headers.Add("X-pagination", JsonSerializer.Serialize(metadata));

            return Ok(Games);
        }

        /// <summary>
        /// Return specified game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGameById(int gameId)
        {
            if (!await gameRepository.GameExistsAsync(gameId))
                return NotFound($"Not found game with such id {gameId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Game = mapper.Map<GameDto>(await gameRepository.GetGameByIdAsync(gameId));

            Game.PicturePath = PictureController.PathToUrl(Game.PicturePath);

            return Ok(Game);
        }

        /// <summary>
        /// Return reviews of specified game  
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}/review")]
        [ProducesResponseType(200, Type = typeof(IList<ReviewDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGameReview(int gameId)
        {
            if (!await gameRepository.GameExistsAsync(gameId))
                return NotFound($"Not found game with such id {gameId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Review = mapper.Map<List<ReviewDto>>(await reviewRepository.GetGameReviewsAsync(gameId));

            return Ok(Review);
        }

        /// <summary>
        /// Create new Game
        /// </summary>
        /// <param name="gameCreate"></param>
        /// <returns></returns>
        [HttpPost("createGame")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGame([FromBody] GameCreateDto gameCreate)
        {
            if (gameCreate == null)
                return BadRequest(ModelState);

            if (await gameRepository.GetGameByNameAsync(gameCreate.Name) != null)
                return BadRequest("Game with this name already exists");

            if (!Enum.GetNames(typeof(Enums.Types)).Contains(gameCreate.Type.Trim().ToLower()))
                return BadRequest("Unsupported type");

            if (!Enum.GetNames(typeof(Enums.Status)).Contains(gameCreate.Status.Trim().ToLower()))
                return BadRequest("Unsupported status");

            if (!await ageRatingRepository.AgeRatingExistsAsync(gameCreate.AgeRating))
                return NotFound($"Not found age rating with such id {gameCreate.AgeRating}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var devS = new List<Developer>();
            foreach (var item in gameCreate.Developers) 
            {
                var dev = await developerRepository.GetDeveloperByIdAsync(item);
                if (dev == null)
                    return NotFound($"Not found developer with such id {item}");
                devS.Add(dev);
            }

            var pubS = new List<Publisher>();
            foreach (var item in gameCreate.Publishers)
            {
                var pub = await publisherRepository.GetPublisherByIdAsync(item);
                if (pub == null)
                    return NotFound($"Not found publisher with such id {item}");
                pubS.Add(pub);
            }

            var platS = new List<Platform>();
            foreach (var item in gameCreate.Platforms)
            {
                var plat = await platformRepository.GetPlatformByIdAsync(item);
                if (plat == null)
                    return NotFound($"Not found platform with such id {item}");
                platS.Add(plat);
            }

            var genrS = new List<Genre>();
            foreach (var item in gameCreate.Genres)
            {
                var genr = await genreRepository.GetGenreByIdAsync(item);
                if (genr == null)
                    return NotFound($"Not found genre with such id {item}");
                genrS.Add(genr);
            }

            var tagS = new List<Tag>();
            foreach (var item in gameCreate.Tags)
            {
                var tag = await tagRepository.GetTagByIdAsync(item);
                if (tag == null)
                    return NotFound($"Not found tag with such id {item}");
                tagS.Add(tag);
            }


            var gameMap = mapper.Map<Game>(gameCreate);

            gameMap.AgeRating = await ageRatingRepository.GetAgeRatingByIdAsync(gameCreate.AgeRating);
            gameMap.PicturePath = $"\\Images\\gamePicture\\Def.jpg";
            gameMap.Reviews = new List<Review>();
            gameMap.DLCs = new List<DLC>();
            gameMap.Rating = new Rating();
            gameMap.Developers = devS;
            gameMap.Publishers = pubS;
            gameMap.Platforms = platS;
            gameMap.Genres = genrS;
            gameMap.Tags = tagS;

            gameRepository.CreateGame(gameMap);
            await gameRepository.SaveGameAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified Game
        /// </summary>
        /// <param name="gameUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateGame")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateGameInfo([FromBody] GameUpdate gameUpdate)
        {
            if (gameUpdate == null)
                return BadRequest(ModelState);

            if (!await gameRepository.GameExistsAsync(gameUpdate.Id))
                return NotFound($"Not found game with such id {gameUpdate.Id}");

            if (await gameRepository.GameNameAlreadyInUseAsync(gameUpdate.Id, gameUpdate.Name))
                return BadRequest("Name already in use");

            if (!Enum.GetNames(typeof(Enums.Status)).Contains(gameUpdate.Status.Trim().ToLower()))
                return BadRequest("Unsupported status");

            if (!await ageRatingRepository.AgeRatingExistsAsync(gameUpdate.AgeRating.Id))
                return BadRequest($"Not found AgeRating with such id {gameUpdate.AgeRating.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var devS = new List<Developer>();
            foreach (var item in gameUpdate.Developers)
            {
                var dev = await developerRepository.GetDeveloperByIdAsync(item.Id);
                if (dev == null)
                    return BadRequest($"Not found developer with such id {item.Id}");
                devS.Add(dev);
            }

            var pubS = new List<Publisher>();
            foreach (var item in gameUpdate.Publishers)
            {
                var pub = await publisherRepository.GetPublisherByIdAsync(item.Id);
                if (pub == null)
                    return BadRequest($"Not found publisher with such id {item.Id}");
                pubS.Add(pub);
            }

            var platS = new List<Platform>();
            foreach (var item in gameUpdate.Platforms)
            {
                var plat = await platformRepository.GetPlatformByIdAsync(item.Id);
                if (plat == null)
                    return BadRequest($"Not found platform with such id {item.Id}");
                platS.Add(plat);
            }

            var genrS = new List<Genre>();
            foreach (var item in gameUpdate.Genres)
            {
                var genr = await genreRepository.GetGenreByIdAsync(item.Id);
                if (genr == null)
                    return BadRequest($"Not found genre with such id {item.Id}");
                genrS.Add(genr);
            }

            var tagS = new List<Tag>();
            foreach (var item in gameUpdate.Tags)
            {
                var tag = await tagRepository.GetTagByIdAsync(item.Id);
                if (tag == null)
                    return BadRequest($"Not found tag with such id {item.Id}");
                tagS.Add(tag);
            }

            var game = await gameRepository.GetGameByIdAsync(gameUpdate.Id);

            game.Name = gameUpdate.Name;
            game.Description = gameUpdate.Description;
            game.Status = gameUpdate.Status;
            game.ReleaseDate = gameUpdate.ReleaseDate;
            game.NSFW = gameUpdate.NSFW;
            game.AveragePlayTime = gameUpdate.AveragePlayTime;

            game.AgeRating = await ageRatingRepository.GetAgeRatingByIdAsync(gameUpdate.AgeRating.Id);

            game.Developers = devS;
            game.Publishers = pubS;
            game.Platforms = platS;
            game.Genres = genrS;
            game.Tags = tagS;

            gameRepository.UpdateGame(game);
            await gameRepository.SaveGameAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified game
        /// </summary>
        /// <param name="gameDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteGame")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteGame([FromQuery] int gameDelete)
        {
            if(!await gameRepository.GameExistsAsync(gameDelete))
                return NotFound($"Not found game with such id {gameDelete}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var game = await gameRepository.GetGameByIdAsync(gameDelete);

            if (game.DLCs != null)
            {
                foreach (var item in game.DLCs.ToList())
                {
                    dlcRepository.DLCDelete(item);
                    item.DLCGame.ParentGame = null;
                }
                await dlcRepository.SaveDLCAsync();
            }

            if (game.PicturePath != $"\\Images\\gamePicture\\Def.jpg")
            {
                FileInfo f = new(game.PicturePath);
                f.Delete();
            }
            
            gameRepository.DeleteGame(game);
            await gameRepository.SaveGameAsync();

            ratingRepository.DeleteRating(game.Rating);
            await ratingRepository.SaveRatingAsync();

            if (game.SystemRequirements != null)
            {
                foreach (var item in game.SystemRequirements)
                    systemRequirements.DeleteSystemRequirements(item);
                await systemRequirements.SaveSystemRequirementsAsync();
            }

            return Ok("Successfully Deleted");
        }

        [HttpGet("{userId}/favouriteGames")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserFavouriteGames(int userId)
        {
            if (!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = mapper.Map<List<GameSmallListDto>>(await personGamesRepository.GetPersonFavouriteGameAsync(userId));

            return Ok(games);
        }

    }
}
