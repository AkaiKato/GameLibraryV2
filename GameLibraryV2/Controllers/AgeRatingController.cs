using AutoMapper;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GameLibraryV2.Dto.Create;
using GameLibraryV2.Models;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Dto.Common;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeRatingController : Controller
    {
        private readonly IAgeRatingRepository ageRatingRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public AgeRatingController(IAgeRatingRepository _ageRatingRepository, 
            IGameRepository _gameRepository,
            IMapper _mapper)
        {
            ageRatingRepository = _ageRatingRepository;
            gameRepository = _gameRepository;
            mapper = _mapper;
        }


        /// <summary>
        /// Return all AgeRatings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAgeRatings()
        {
            var AgeRatings = ageRatingRepository.GetAgeRatings();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(AgeRatings);
        }

        /// <summary>
        /// Return Specified AgeRating
        /// </summary>
        /// <param name="ageRatingId"></param>
        /// <returns></returns>
        [HttpGet("{ageRatingId}")]
        public IActionResult GetAgeRatingById(int ageRatingId)
        {
            if(!ageRatingRepository.AgeRatingExists(ageRatingId))
                return NotFound($"Not found AgeRating with such id {ageRatingId}");

            var AgeRating = ageRatingRepository.GetAgeRatingById(ageRatingId);

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(AgeRating);
        }

        /// <summary>
        /// Return all AgeRating games
        /// </summary>
        /// <param name="ageRatingId"></param>
        /// <returns></returns>
        [HttpGet("{ageRatingId}/games")]
        public IActionResult GetAgeRatingGames(int ageRatingId)
        {
            if(!ageRatingRepository.AgeRatingExists(ageRatingId))
                return NotFound($"Not found game with such id {ageRatingId}");

            var AgeRatingGames = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGamesByAgeRating(ageRatingId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(AgeRatingGames);
        }

        /// <summary>
        /// Create AgeRating
        /// </summary>
        /// <param name="ageRatingCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateAgeRating([FromBody] AgeRatingCreateDto ageRatingCreate) 
        {
            if(ageRatingCreate == null)
                return BadRequest(ModelState);

            var ageRating = ageRatingRepository.GetAgeRatingByName(ageRatingCreate.Name);

            if (ageRating != null)
                return BadRequest("AgeRating already exists");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var ageRatingMap = mapper.Map<AgeRating>(ageRatingCreate);

            if(!ageRatingRepository.CreateAgeRating(ageRatingMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified ageRating
        /// </summary>
        /// <param name="ageRatingUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateAgeRating")]
        public IActionResult UpdateAgeRating([FromBody] CommonUpdate ageRatingUpdate)
        {
            if(ageRatingUpdate == null)
                return BadRequest(ModelState);

            if(!ageRatingRepository.AgeRatingExists(ageRatingUpdate.Id))
                return NotFound($"Not found game with such id {ageRatingUpdate.Id}");

            if(ageRatingRepository.AgeRatingAlreadyExists(ageRatingUpdate.Id, ageRatingUpdate.Name))
                return BadRequest("Age Rating Name already in use");

            var ageRating = ageRatingRepository.GetAgeRatingById(ageRatingUpdate.Id);

            ageRating.Name = ageRatingUpdate.Name;
            ageRating.Description = ageRatingUpdate.Description;

            if(!ageRatingRepository.UpdateAgeRating(ageRating))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully updated");
        }

        [HttpDelete("deleteAgeRating")]
        public IActionResult DeleteAgeRating([FromBody] JustIdDto ageRatingDelete)
        {
            if (!ageRatingRepository.AgeRatingExists(ageRatingDelete.Id))
                return NotFound($"Not found game with such id {ageRatingDelete.Id}");

            var ageRating = ageRatingRepository.GetAgeRatingById(ageRatingDelete.Id);

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (!ageRatingRepository.DeleteAgeRating(ageRating))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
