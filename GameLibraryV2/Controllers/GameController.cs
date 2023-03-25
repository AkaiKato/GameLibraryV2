using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
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
        private readonly IDeveloperRepository developerRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITagRepository tagRepository;
        private readonly IPlatformRepository platformRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IDLCRepository dlcRepository;
        private readonly IMapper mapper;

        public GameController(IGameRepository _gameRepository,
            IDeveloperRepository _developerRepository, 
            IPublisherRepository _publisherRepository, 
            IGenreRepository _genreRepository,
            ITagRepository _tagRepository,
            IPlatformRepository _platformRepository,
            IReviewRepository _reviewRepository,
            IDLCRepository _dlcRepository,
            IMapper _mapper)
        {
            gameRepository = _gameRepository;
            developerRepository = _developerRepository;
            publisherRepository = _publisherRepository;
            platformRepository = _platformRepository;
            genreRepository = _genreRepository;
            tagRepository = _tagRepository;
            reviewRepository = _reviewRepository;
            dlcRepository = _dlcRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all games
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        public IActionResult GetGames()
        {
            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGames());

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
        /// Return reviews of specified game  
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

            var Review = mapper.Map<List<ReviewDto>>(reviewRepository.GetGameReviews(gameId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Review));
        }

        /// <summary>
        /// Create new Game
        /// </summary>
        /// <param name="gameCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGame([FromBody] GameCreateDto gameCreate)
        {
            if (gameCreate == null)
                return BadRequest(ModelState);

            if (gameRepository.GetGameByName(gameCreate.Name) != null)
            {
                ModelState.AddModelError("", "Game with this name already exists");
                return StatusCode(422, ModelState);
            }

            if (gameCreate.Type.Trim().ToLower() != Enums.Types.game.ToString() 
                && gameCreate.Type.Trim().ToLower() != Enums.Types.dlc.ToString())
            {
                ModelState.AddModelError("", "Unsupported type");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var devS = new List<Developer>();
            foreach (var item in gameCreate.Developers) 
            {
                var dev = developerRepository.GetDeveloperById(item.Id);
                if (dev != null)
                    devS.Add(dev);
                else
                {
                    ModelState.AddModelError("", $"Not found developer with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
            }

            var pubS = new List<Publisher>();
            foreach (var item in gameCreate.Publishers)
            {
                var pub = publisherRepository.GetPublisherById(item.Id);
                if (pub != null)
                    pubS.Add(pub);
                else
                {
                    ModelState.AddModelError("", $"Not found publisher with such id {item}");
                    return StatusCode(422, ModelState);
                }
            }

            var platS = new List<Platform>();
            foreach (var item in gameCreate.Platforms)
            {
                var plat = platformRepository.GetPlatformById(item.Id);
                if (plat != null)
                    platS.Add(plat);
                else
                {
                    ModelState.AddModelError("", $"Not found platform with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
            }

            var genrS = new List<Genre>();
            foreach (var item in gameCreate.Genres)
            {
                var genr = genreRepository.GetGenreById(item.Id);
                if (genr != null)
                    genrS.Add(genr);
                else
                {
                    ModelState.AddModelError("", $"Not found genre with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
            }

            var tagS = new List<Tag>();
            foreach (var item in gameCreate.Tags)
            {
                var tag = tagRepository.GetTagById(item.Id);
                if (tag != null)
                    tagS.Add(tag);
                else
                {
                    ModelState.AddModelError("", $"Not found tag with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
            }

            var gameMap = mapper.Map<Game>(gameCreate);

            gameMap.PicturePath = $"\\Images\\gamePicture\\Def";
            gameMap.Reviews = new List<Review>();
            gameMap.DLCs = new List<DLC>();
            gameMap.Rating = new Rating();
            gameMap.Developers = devS;
            gameMap.Publishers = pubS;
            gameMap.Platforms = platS;
            gameMap.Genres = genrS;
            gameMap.Tags = tagS;

            if (!gameRepository.CreateGame(gameMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult UpdateGameInfo([FromBody] GameUpdate gameUpdate)
        {
            if (gameUpdate == null)
                return BadRequest(ModelState);

            if (!gameRepository.GameExists(gameUpdate.Id))
                return NotFound();

            var devS = new List<Developer>();
            foreach (var item in gameUpdate.Developers)
            {
                var dev = developerRepository.GetDeveloperById(item.Id);
                if (dev == null)
                {
                    ModelState.AddModelError("", $"Not found developer with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
                devS.Add(dev);
            }

            var pubS = new List<Publisher>();
            foreach (var item in gameUpdate.Publishers)
            {
                var pub = publisherRepository.GetPublisherById(item.Id);
                if (pub == null)
                {
                    ModelState.AddModelError("", $"Not found publisher with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
                pubS.Add(pub);
            }

            var platS = new List<Platform>();
            foreach (var item in gameUpdate.Platforms)
            {
                var plat = platformRepository.GetPlatformById(item.Id);
                if (plat == null)
                {
                    ModelState.AddModelError("", $"Not found platform with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
                platS.Add(plat);
            }

            var genrS = new List<Genre>();
            foreach (var item in gameUpdate.Genres)
            {
                var genr = genreRepository.GetGenreById(item.Id);
                if (genr == null)
                {
                    ModelState.AddModelError("", $"Not found genre with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
                genrS.Add(genr);
            }

            var tagS = new List<Tag>();
            foreach (var item in gameUpdate.Tags)
            {
                var tag = tagRepository.GetTagById(item.Id);
                if (tag == null)
                {
                    ModelState.AddModelError("", $"Not found tag with such id {item.Id}");
                    return StatusCode(422, ModelState);
                }
                tagS.Add(tag);
            }

            var game = gameRepository.GetGameById(gameUpdate.Id);

            if (game.Type.Trim().ToLower() == "dlc".Trim().ToLower() && gameUpdate.DLCs != null) 
            {
                ModelState.AddModelError("", $"DLC can't have DLC");
                return StatusCode(422, ModelState);
            }

            game.Name = gameUpdate.Name;
            game.Description = gameUpdate.Description;
            game.ReleaseDate = gameUpdate.ReleaseDate;
            game.AgeRating = gameUpdate.AgeRating;
            game.NSFW = gameUpdate.NSFW;
            game.AveragePlayTime = gameUpdate.AveragePlayTime;

            game.SystemRequirementsMin.OC = gameUpdate.SystemRequirementsMin.OC;
            game.SystemRequirementsMin.Processor = gameUpdate.SystemRequirementsMin.Processor;
            game.SystemRequirementsMin.RAM = gameUpdate.SystemRequirementsMin.OC;
            game.SystemRequirementsMin.VideoCard = gameUpdate.SystemRequirementsMin.VideoCard;
            game.SystemRequirementsMin.DirectX = gameUpdate.SystemRequirementsMin.DirectX;
            game.SystemRequirementsMin.Ethernet = gameUpdate.SystemRequirementsMin.Ethernet;
            game.SystemRequirementsMin.HardDriveSpace = gameUpdate.SystemRequirementsMin.HardDriveSpace;
            game.SystemRequirementsMin.Additional = gameUpdate.SystemRequirementsMin.Additional;

            game.SystemRequirementsMax.OC = gameUpdate.SystemRequirementsMax.OC;
            game.SystemRequirementsMax.Processor = gameUpdate.SystemRequirementsMax.Processor;
            game.SystemRequirementsMax.RAM = gameUpdate.SystemRequirementsMax.OC;
            game.SystemRequirementsMax.VideoCard = gameUpdate.SystemRequirementsMax.VideoCard;
            game.SystemRequirementsMax.DirectX = gameUpdate.SystemRequirementsMax.DirectX;
            game.SystemRequirementsMax.Ethernet = gameUpdate.SystemRequirementsMax.Ethernet;
            game.SystemRequirementsMax.HardDriveSpace = gameUpdate.SystemRequirementsMax.HardDriveSpace;
            game.SystemRequirementsMax.Additional = gameUpdate.SystemRequirementsMax.Additional;

            game.Developers = devS;
            game.Publishers = pubS;
            game.Platforms = platS;
            game.Genres = genrS;
            game.Tags = tagS;


            var dlcS = new List<DLC>();
            if (game.Type.Trim().ToLower() == Enums.Types.game.ToString() && gameUpdate.DLCs != null)
            {
                foreach (var item in gameUpdate.DLCs)
                {
                    var gameDlc = gameRepository.GetDLCById(item.Id);
                    if (gameDlc == null || gameDlc.ParentGame != null)
                    {
                        ModelState.AddModelError("", $"Not found DLC with such id {item.Id} or DLC already have Parent Game");
                        return StatusCode(422, ModelState);
                    }
                    dlcS.Add(new DLC() { ParentGame = game, DLCGame = gameDlc, });
                }
            }

            game.DLCs = dlcS;

            if (!gameRepository.UpdateGame(game))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (dlcS != null)
            {
                foreach (var item in dlcS)
                {
                    if (!gameRepository.UpdateGame(item.DLCGame.ParentGame = game))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                }
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update Game Picture
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("upload")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadGamePicture([FromQuery] int gameId, IFormFile pic)
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

            game!.PicturePath = newfilePath;

            if (!gameRepository.UpdateGame(game))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\gamePicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

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
        public IActionResult DeleteGame([FromBody] JustIdDto gameDelete)
        {
            if(!gameRepository.GameExists(gameDelete.Id))
                return NotFound();

            var game = gameRepository.GetGameById(gameDelete.Id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (game.DLCs != null)
            {
                foreach (var item in game.DLCs.ToList())
                {
                    if (!dlcRepository.DLCDelete(item))
                    {
                        ModelState.AddModelError("", "Something went wrong while saving");
                        return StatusCode(500, ModelState);
                    }
                    item.DLCGame.ParentGame = null;
                }
            }

            if (game.PicturePath != $"\\Images\\gamePicture\\Def.jpg")
            {
                FileInfo f = new(game.PicturePath);
                f.Delete();
            }

            if (!gameRepository.DeleteGame(game))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //Добавить удаление картинки игры

            return Ok("Successfully Deleted");
        }

    }
}
