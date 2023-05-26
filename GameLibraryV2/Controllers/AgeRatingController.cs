using AutoMapper;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GameLibraryV2.Dto.Create;
using GameLibraryV2.Models;
using GameLibraryV2.Dto.Update;
using System.Text.Json;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet("ageRatingAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAgeRatings()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var AgeRatings = await ageRatingRepository.GetAgeRatingsAsync();

            return Ok(AgeRatings);
        }

        /// <summary>
        /// Return Specified AgeRating
        /// </summary>
        /// <param name="ageRatingId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{ageRatingId}")]
        public async Task<IActionResult> GetAgeRatingById(int ageRatingId)
        {
            if(!await ageRatingRepository.AgeRatingExistsAsync(ageRatingId))
                return NotFound($"Not found AgeRating with such id {ageRatingId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var AgeRating = await ageRatingRepository.GetAgeRatingByIdAsync(ageRatingId);

            return Ok(AgeRating);
        }

        /// <summary>
        /// Return all AgeRating games
        /// </summary>
        /// <param name="ageRatingId"></param>
        /// <param name="filterParameters"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{ageRatingId}/games")]
        public async Task<IActionResult> GetAgeRatingGames(int ageRatingId, [FromQuery] FilterParameters filterParameters, [FromQuery] Pagination pagination)
        {
            if(!await ageRatingRepository.AgeRatingExistsAsync(ageRatingId))
                return NotFound($"Not found AgeRating with such id {ageRatingId}");

            if (!filterParameters.ValidYearRange)
                return BadRequest("Max release year cannot be less than min year");

            if (!filterParameters.ValidPlayTime)
                return BadRequest("Max playtime cannot be less than min playtime");

            if (!filterParameters.ValidRating)
                return BadRequest("Rating cannot be less than 0");

            if (!filterParameters.ValidStatus)
                return BadRequest("Not Valid Status");

            if (!filterParameters.ValidType)
                return BadRequest("Not Valid Type");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var games = await gameRepository.GetGamesByAgeRatingAsync(ageRatingId, filterParameters, pagination);

            var metadata = new
            {
                games.TotalCount,
                games.PageSize,
                games.CurrentPage,
                games.TotalPages,
                games.HasNext,
                games.HasPrevious,
            };

            var AgeRatingGames = mapper.Map<List<GameSmallListDto>>(games);

            Response.Headers.Add("X-pagination", JsonSerializer.Serialize(metadata));

            return Ok(AgeRatingGames);
        }

        /// <summary>
        /// Create AgeRating
        /// </summary>
        /// <param name="ageRatingCreate"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost("createAgeRating")]
        public async Task<IActionResult> CreateAgeRating([FromBody] AgeRatingCreateDto ageRatingCreate) 
        {
            if(ageRatingCreate == null)
                return BadRequest(ModelState);

            var ageRating = await ageRatingRepository.GetAgeRatingByNameAsync(ageRatingCreate.Name);

            if (ageRating != null)
                return BadRequest("AgeRating already exists");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var ageRatingMap = mapper.Map<AgeRating>(ageRatingCreate);

            ageRatingRepository.CreateAgeRating(ageRatingMap);
            await ageRatingRepository.SaveAgeRatingAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified ageRating
        /// </summary>
        /// <param name="ageRatingUpdate"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("updateAgeRating")]
        public async Task<IActionResult> UpdateAgeRating([FromBody] CommonUpdate ageRatingUpdate)
        {
            if(ageRatingUpdate == null)
                return BadRequest(ModelState);

            if(!await ageRatingRepository.AgeRatingExistsAsync(ageRatingUpdate.Id))
                return NotFound($"Not found game with such id {ageRatingUpdate.Id}");

            if(await ageRatingRepository.AgeRatingAlreadyExistsAsync(ageRatingUpdate.Id, ageRatingUpdate.Name))
                return BadRequest("Age Rating Name already in use");

            var ageRating = await ageRatingRepository.GetAgeRatingByIdAsync(ageRatingUpdate.Id);

            ageRating.Name = ageRatingUpdate.Name;
            ageRating.Description = ageRatingUpdate.Description;

            ageRatingRepository.UpdateAgeRating(ageRating);
            await ageRatingRepository.SaveAgeRatingAsync();

            return Ok("successfully updated");
        }

        /// <summary>
        /// Delete Specified ageRating
        /// </summary>
        /// <param name="ageRatingDelete"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("deleteAgeRating")]
        public async Task<IActionResult> DeleteAgeRating([FromQuery] int ageRatingDelete)
        {
            if (!await ageRatingRepository.AgeRatingExistsAsync(ageRatingDelete))
                return NotFound($"Not found game with such id {ageRatingDelete}");

            var ageRating = await ageRatingRepository.GetAgeRatingByIdAsync(ageRatingDelete);

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            ageRatingRepository.DeleteAgeRating(ageRating);
            await ageRatingRepository.SaveAgeRatingAsync();

            return Ok("Successfully deleted");
        }
    }
}
