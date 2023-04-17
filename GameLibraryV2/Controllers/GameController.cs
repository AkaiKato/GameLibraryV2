﻿using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IReviewRepository reviewRepository;
        private readonly IDLCRepository dlcRepository;
        private readonly IAgeRatingRepository ageRatingRepository;
        private readonly IMapper mapper;

        public GameController(IGameRepository _gameRepository,
            IDeveloperRepository _developerRepository, 
            IPublisherRepository _publisherRepository, 
            IGenreRepository _genreRepository,
            ITagRepository _tagRepository,
            IPlatformRepository _platformRepository,
            IReviewRepository _reviewRepository,
            IDLCRepository _dlcRepository,
            IAgeRatingRepository _ageRatingRepository,
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
            ageRatingRepository = _ageRatingRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all games
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<GameSmallListDto>))]
        public IActionResult GetGames([FromQuery] SearchParameters searchParameters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!searchParameters.ValidYearRange)
                return BadRequest("Max release year cannot be less than min year");

            if (!searchParameters.ValidPlayTime)
                return BadRequest("Max playtime cannot be less than min playtime");

            if (!searchParameters.ValidRating)
                return BadRequest("Rating cannot be less than 0");

            if(!searchParameters.ValidStatus)
                return BadRequest("Not Valid Status");

            if (!searchParameters.ValidType)
                return BadRequest("Not Valid Type");

            var games = gameRepository.GetGames(searchParameters);

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
                return BadRequest("Game with this name already exists");
            }

            if (gameCreate.Type.Trim().ToLower() != Enums.Types.game.ToString() 
                && gameCreate.Type.Trim().ToLower() != Enums.Types.dlc.ToString())
            {
                return BadRequest("Unsupported type");
            }

            if(gameCreate.Status.Trim().ToLower() != Enums.Status.released.ToString()
                && gameCreate.Status.Trim().ToLower() != Enums.Status.announsed.ToString()) 
            {
                return BadRequest("Unsupported status");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var devS = new List<Developer>();
            foreach (var item in gameCreate.Developers) 
            {
                var dev = developerRepository.GetDeveloperById(item.Id);
                if (dev == null)
                    return BadRequest($"Not found developer with such id {item.Id}");
                devS.Add(dev);
            }

            var pubS = new List<Publisher>();
            foreach (var item in gameCreate.Publishers)
            {
                var pub = publisherRepository.GetPublisherById(item.Id);
                if (pub == null)
                    return BadRequest($"Not found publisher with such id {item}");
                pubS.Add(pub);
            }

            var platS = new List<Platform>();
            foreach (var item in gameCreate.Platforms)
            {
                var plat = platformRepository.GetPlatformById(item.Id);
                if (plat == null)
                    return BadRequest($"Not found platform with such id {item.Id}");
                platS.Add(plat);
            }

            var genrS = new List<Genre>();
            foreach (var item in gameCreate.Genres)
            {
                var genr = genreRepository.GetGenreById(item.Id);
                if (genr == null)
                    return BadRequest($"Not found genre with such id {item.Id}");
                genrS.Add(genr);
            }

            var tagS = new List<Tag>();
            foreach (var item in gameCreate.Tags)
            {
                var tag = tagRepository.GetTagById(item.Id);
                if (tag == null)
                    return BadRequest($"Not found tag with such id {item.Id}");
                tagS.Add(tag);
            }

            var gameMap = mapper.Map<Game>(gameCreate);

            gameMap.AgeRating = gameCreate.AgeRating;
            gameMap.PicturePath = $"\\Images\\gamePicture\\Def.jpg";
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

            if(gameRepository.GameNameAlreadyInUse(gameUpdate.Id, gameUpdate.Name))
            {
                ModelState.AddModelError("", $"Name already in use");
                return StatusCode(422, ModelState);
            }

            if (gameUpdate.Status.Trim().ToLower() != Enums.Status.released.ToString()
                && gameUpdate.Status.Trim().ToLower() != Enums.Status.announsed.ToString())
            {
                return BadRequest("Unsupported status");
            }

            if (!ageRatingRepository.AgeRatingExists(gameUpdate.AgeRating.Id))
                return BadRequest($"Not found AgeRating with such id {gameUpdate.AgeRating.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var devS = new List<Developer>();
            foreach (var item in gameUpdate.Developers)
            {
                var dev = developerRepository.GetDeveloperById(item.Id);
                if (dev == null)
                    return BadRequest($"Not found developer with such id {item.Id}");
                devS.Add(dev);
            }

            var pubS = new List<Publisher>();
            foreach (var item in gameUpdate.Publishers)
            {
                var pub = publisherRepository.GetPublisherById(item.Id);
                if (pub == null)
                    return BadRequest($"Not found publisher with such id {item.Id}");
                pubS.Add(pub);
            }

            var platS = new List<Platform>();
            foreach (var item in gameUpdate.Platforms)
            {
                var plat = platformRepository.GetPlatformById(item.Id);
                if (plat == null)
                    return BadRequest($"Not found platform with such id {item.Id}");
                platS.Add(plat);
            }

            var genrS = new List<Genre>();
            foreach (var item in gameUpdate.Genres)
            {
                var genr = genreRepository.GetGenreById(item.Id);
                if (genr == null)
                    return BadRequest($"Not found genre with such id {item.Id}");
                genrS.Add(genr);
            }

            var tagS = new List<Tag>();
            foreach (var item in gameUpdate.Tags)
            {
                var tag = tagRepository.GetTagById(item.Id);
                if (tag == null)
                    return BadRequest($"Not found tag with such id {item.Id}");
                tagS.Add(tag);
            }

            var game = gameRepository.GetGameById(gameUpdate.Id);

            game.Name = gameUpdate.Name;
            game.Description = gameUpdate.Description;
            game.Status = gameUpdate.Status;
            game.ReleaseDate = gameUpdate.ReleaseDate;
            game.NSFW = gameUpdate.NSFW;
            game.AveragePlayTime = gameUpdate.AveragePlayTime;

            game.AgeRating = ageRatingRepository.GetAgeRatingById(gameUpdate.AgeRating.Id);

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

            if (!gameRepository.UpdateGame(game))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
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

            return Ok("Successfully Deleted");
        }

    }
}
