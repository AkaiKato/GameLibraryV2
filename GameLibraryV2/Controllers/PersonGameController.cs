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
        private readonly IReviewRepository reviewRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IMapper mapper;

        public PersonGameController(IPersonGamesRepository _personGameRepository,
            IGameRepository _gameRepository, IUserRepository _userRepository,
            IPlatformRepository _platformRepository, IReviewRepository _reviewRepository,
            IRatingRepository _ratingRepository, IMapper _mapper)
        {
            personGameRepository = _personGameRepository;
            gameRepository = _gameRepository;
            userRepository = _userRepository;
            platformRepository = _platformRepository;
            reviewRepository = _reviewRepository;
            ratingRepository = _ratingRepository;
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
            if(!userRepository.UserExistsById(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var PersonGames = mapper.Map<List<PersonGameDto>>(personGameRepository.PersonGames(userId));

            return Ok(PersonGames);
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
            if(!userRepository.UserExistsById(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var PersonGamesByList = mapper.Map<List<PersonGameDto>>(personGameRepository.PersonGamesByList(userId, list));

            return Ok(PersonGamesByList);
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

            if(!userRepository.UserExistsById(personGameCreate.UserId))
                return NotFound($"Not found user with such id {personGameCreate.UserId}");

            if(!gameRepository.GameExists(personGameCreate.GameId))
                return NotFound($"Not found game with such id {personGameCreate.GameId}");

            personGameCreate.List = personGameCreate.List;
            
            if(!Enum.GetNames(typeof(Enums.List)).Contains(personGameCreate.List.Trim().ToLower()))
               return BadRequest("Unsupported list");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

            return Ok("Successfully added");
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
                return NotFound($"Not found persongame with such id {personGameUpdate.Id}");

            if (!Enum.GetNames(typeof(Enums.List)).Contains(personGameUpdate.List.Trim().ToLower()))
                return BadRequest("Unsupported list");

            if ((personGameUpdate.Score < 1 || personGameUpdate.Score > 10) && personGameUpdate.Score != -1)
                return BadRequest("Wrong Score");   

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personGame = personGameRepository.GetPersonGameById(personGameUpdate.Id);

            if(personGame.Score != personGameUpdate.Score && reviewRepository.ReviewExists(personGame.User.Id, personGame.Game.Id))
            {
                var review = reviewRepository.GetReviewByUserIdAndGameId(personGame.User.Id, personGame.Game.Id);
                review.Rating = personGameUpdate.Score;
                if (!reviewRepository.UpdateReview(review))
                    return BadRequest("Something went wrong while saving");
            }

            if(personGame.Score != personGameUpdate.Score)
            {
                ratingRepository.Remove(personGame.Game.Rating, personGame.Score%10);
                ratingRepository.Add(personGame.Game.Rating, personGameUpdate.Score%10);
            }

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

            return Ok("Successfully updated");
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
                return NotFound($"Not found personGame with such id {personGameDelete.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personGame = personGameRepository.GetPersonGameById(personGameDelete.Id);

            if(personGame.Score != -1)
                ratingRepository.Remove(personGame.Game.Rating, personGame.Score % 10);

            if (reviewRepository.ReviewExists(personGame.User.Id, personGame.Game.Id))
            {
                var review = reviewRepository.GetReviewByUserIdAndGameId(personGame.User.Id, personGame.Game.Id);
                if (!reviewRepository.DeleteReview(review))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            if (!personGameRepository.DeletePersonGame(personGame))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
