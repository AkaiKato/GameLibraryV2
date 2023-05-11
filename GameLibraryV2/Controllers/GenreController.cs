using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreRepository genreRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public GenreController(IGenreRepository _genreRepository, IGameRepository _gameRepository,IMapper _mapper)
        {
            genreRepository = _genreRepository;
            gameRepository = _gameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Genres
        /// </summary>
        /// <returns></returns>
        [HttpGet("genreAll")]
        [ProducesResponseType(200, Type = typeof(IList<GenreDto>))]
        public async Task<IActionResult> GetGenres()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Genres = mapper.Map<List<GenreDto>>(await genreRepository.GetGenresAsync());

            return Ok(Genres);
        }

        /// <summary>
        /// Return specified genre
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("{genreId}")]
        [ProducesResponseType(200, Type = typeof(GenreDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGenreById(int genreId) 
        {
            if(!await genreRepository.GenreExistsAsync(genreId))
                return NotFound($"Not found genre with such id {genreId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Genre = mapper.Map<GenreDto>(await genreRepository.GetGenreByIdAsync(genreId));

            return Ok(Genre);
        }

        /// <summary>
        /// Return all genre games
        /// </summary>
        /// <param name="genreId"></param>
        /// <param name="filterParameters"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet("{genreId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGenreGames(int genreId, [FromQuery] FilterParameters filterParameters, [FromQuery] Pagination pagination)
        {
            if (!await genreRepository.GenreExistsAsync(genreId))
                return NotFound($"Not found genre with such id {genreId}");

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

            var games = await gameRepository.GetGamesByGenreAsync(genreId, filterParameters, pagination);

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
        /// Creates new Genre
        /// </summary>
        /// <param name="genreCreate"></param>
        /// <returns></returns>
        [HttpPost("createGenre")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto genreCreate)
        {
            if (genreCreate == null)
                return BadRequest(ModelState);

            var genre = await genreRepository.GetGenreByNameAsync(genreCreate.Name);

            if (genre != null)
                return BadRequest("Genre already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = mapper.Map<Genre>(genreCreate);

            genreRepository.CreateGenre(genreMap);
            await genreRepository.SaveGenreAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified genre
        /// </summary>
        /// <param name="genreUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateGenre")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateGenreInfo([FromBody] CommonUpdate genreUpdate)
        {
            if (genreUpdate == null)
                return BadRequest(ModelState);

            if (!await genreRepository.GenreExistsAsync(genreUpdate.Id))
                return NotFound($"Not found genre with such id {genreUpdate.Id}");

            if (await genreRepository.GenreNameAlredyInUseAsync(genreUpdate.Id, genreUpdate.Name))
                return BadRequest("Name already in use");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var genre = await genreRepository.GetGenreByIdAsync(genreUpdate.Id);

            genre.Name = genreUpdate.Name;
            genre.Description = genreUpdate.Description;

            genreRepository.UpdateGenre(genre);
            await genreRepository.SaveGenreAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified genre
        /// </summary>
        /// <param name="genreDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteGenre")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteGenre([FromQuery] int genreDelete)
        {
            if (!await genreRepository.GenreExistsAsync(genreDelete))
                return NotFound($"Not found genre with such id {genreDelete}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genre = await genreRepository.GetGenreByIdAsync(genreDelete);

            genreRepository.DeleteGenre(genre);
            await genreRepository.SaveGenreAsync();

            return Ok("Successfully deleted");
        }
    }
}
