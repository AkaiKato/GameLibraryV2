using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Create;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonGameController : Controller
    {
        private readonly IPersonGamesRepository personGameRepository;
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;
        //private readonly Enums e = new();

        public PersonGameController(IPersonGamesRepository _personGameRepository,
            IGameRepository _gameRepository, IUserRepository _userRepository,
            IPlatformRepository _platformRepository,
            IMapper _mapper)
        {
            personGameRepository = _personGameRepository;
            gameRepository = _gameRepository;
            userRepository = _userRepository;
            platformRepository = _platformRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Person Games
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId}/persongames")]
        [ProducesResponseType(200, Type = typeof(IList<PersonGameDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPersonGames(int userId)
        {
            if(!userRepository.UserExists(userId))
                return NotFound();

            var PersonGames = mapper.Map<List<PersonGameDto>>(personGameRepository.PersonGames(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PersonGames));
        }

        /// <summary>
        /// Return all Person Games By List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpGet("{userId}/persongamesbylist")]
        [ProducesResponseType(200, Type = typeof(IList<PersonGameDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPersonGamesByList(int userId, string list) 
        {
            if(!userRepository.UserExists(userId))
                return NotFound();

            var PersonGamesByList = mapper.Map<List<PersonGameDto>>(personGameRepository.PersonGamesByList(userId, list));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PersonGamesByList));
        }

        /// <summary>
        /// Add Person Game to User
        /// </summary>
        /// <param name="personGameCreate"></param>
        /// <returns></returns>
        [HttpPost("addPersonGame")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddPersonGame([FromBody] PersonGameCreate personGameCreate)
        {
            if(personGameCreate == null)
                return BadRequest(ModelState);

            if(!userRepository.UserExists(personGameCreate.UserId))
                return NotFound(ModelState);

            if(!gameRepository.GameExists(personGameCreate.GameId))
                return NotFound(ModelState);

            personGameCreate.List = personGameCreate.List.Trim().ToLower();

            if (personGameCreate.List != Enums.List.planned.ToString()  && personGameCreate.List != Enums.List.playing.ToString() && 
                personGameCreate.List != Enums.List.completed.ToString() && personGameCreate.List != Enums.List.dropped.ToString() && 
                personGameCreate.List != Enums.List.postponed.ToString())
            {
                ModelState.AddModelError("", "Unssuported list");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var personGame = new PersonGame() 
            {
                User = userRepository.GetUserById(personGameCreate.UserId),
                Game = gameRepository.GetGameById(personGameCreate.GameId),
                List = personGameCreate.List,
            };

            if (!personGameRepository.CreatePersonGame(personGame))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(Json("Successfully added"));
        }

        /// <summary>
        /// Update Person Game
        /// </summary>
        /// <param name="personGameUpdate"></param>
        /// <returns></returns>
        [HttpPut("updatePersonGame")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePersonGame([FromBody] PersonGameUpdate personGameUpdate)
        {
            if (personGameUpdate == null)
                return BadRequest(ModelState);

            if (!personGameRepository.PersonGameExists(personGameUpdate.Id))
                return NotFound();

            personGameUpdate.List = personGameUpdate.List.Trim().ToLower();

            if (personGameUpdate.List != Enums.List.planned.ToString() && personGameUpdate.List != Enums.List.playing.ToString() &&
                personGameUpdate.List != Enums.List.completed.ToString() && personGameUpdate.List != Enums.List.dropped.ToString() &&
                personGameUpdate.List != Enums.List.postponed.ToString())
            {
                ModelState.AddModelError("", "Unssuported list");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var personGame = personGameRepository.GetPersonGameById(personGameUpdate.Id);

            personGame.Score = personGameUpdate.Score;
            personGame.Comment = personGameUpdate.Comment;
            personGame.List = personGameUpdate.List.Trim().ToLower();
            if (personGameUpdate.PlayedPlatform != null)
                personGame.PlayedPlatform = platformRepository.GetPlatformById(personGameUpdate.PlayedPlatform.Id);

            personGame.Favourite = personGameUpdate.Favourite;

            if (!personGameRepository.UpdatePersonGame(personGame))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(Json("Successfully updated"));
        }

        /// <summary>
        /// Delete Person Game
        /// </summary>
        /// <param name="personGameDelete"></param>
        /// <returns></returns>
        [HttpDelete("deletePersonGame")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeletePersonGame([FromBody] JustGuIdDto personGameDelete)
        {
            if (personGameDelete == null)
                return BadRequest(ModelState);

            if (!personGameRepository.PersonGameExists(personGameDelete.Id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var personGame = personGameRepository.GetPersonGameById(personGameDelete.Id);

            if (!personGameRepository.DeletePersonGame(personGame))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(Json("Successfully deleted"));
        }
    }
}
