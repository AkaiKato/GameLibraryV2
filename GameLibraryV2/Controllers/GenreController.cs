using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<GenreDto>))]
        public IActionResult GetGenres()
        {
            var Genres = mapper.Map<List<GenreDto>>(genreRepository.GetGenres());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Genres));
        }

        /// <summary>
        /// Return specified genre
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("{genreId}")]
        [ProducesResponseType(200, Type = typeof(GenreDto))]
        [ProducesResponseType(400)]
        public IActionResult GetGenreById(int genreId) 
        {
            if(!genreRepository.GenreExists(genreId))
                return NotFound();

            var Genre = mapper.Map<GenreDto>(genreRepository.GetGenreById(genreId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Genre));
        }

        /// <summary>
        /// Return all genre games
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        [HttpGet("{genreId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetGenreGames(int genreId)
        {
            if (!genreRepository.GenreExists(genreId))
                return NotFound();

            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGamesByGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }

        /// <summary>
        /// Creates new Genre
        /// </summary>
        /// <param name="genreCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGenre([FromBody] GenreCreateDto genreCreate)
        {
            if (genreCreate == null)
                return BadRequest(ModelState);

            var genre = genreRepository.GetGenreByName(genreCreate.Name);

            if (genre != null)
            {
                ModelState.AddModelError("", "Genre already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = mapper.Map<Genre>(genreCreate);

            if (!genreRepository.CreateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
