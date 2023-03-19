using AutoMapper;
using GameLibraryV2.Dto;
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
        private readonly IMapper mapper;

        public GenreController(IGenreRepository _genreRepository, IMapper _mapper)
        {
            genreRepository = _genreRepository;
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
        [ProducesResponseType(200, Type = typeof(IList<GameListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetGenreGames(int genreId)
        {
            if (!genreRepository.GenreExists(genreId))
                return NotFound();

            var Games = mapper.Map<List<GameListDto>>(genreRepository.GetGamesByGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }

    }
}
