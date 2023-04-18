﻿using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Repositories;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Genres = mapper.Map<List<GenreDto>>(genreRepository.GetGenres());

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
        public IActionResult GetGenreById(int genreId) 
        {
            if(!genreRepository.GenreExists(genreId))
                return NotFound($"Not found genre with such id {genreId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Genre = mapper.Map<GenreDto>(genreRepository.GetGenreById(genreId));

            return Ok(Genre);
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
                return NotFound($"Not found genre with such id {genreId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGamesByGenre(genreId));

            return Ok(Games);
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
                return BadRequest("Genre already exists");

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

        /// <summary>
        /// Update specified genre
        /// </summary>
        /// <param name="genreUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateGenre")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateGenreInfo([FromBody] CommonUpdate genreUpdate)
        {
            if (genreUpdate == null)
                return BadRequest(ModelState);

            if (!genreRepository.GenreExists(genreUpdate.Id))
                return NotFound($"Not found genre with such id {genreUpdate.Id}");

            if (genreRepository.GenreNameAlredyInUse(genreUpdate.Id, genreUpdate.Name))
                return BadRequest("Name already in use");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var genre = genreRepository.GetGenreById(genreUpdate.Id);

            genre.Name = genreUpdate.Name;
            genre.Description = genreUpdate.Description;

            if (!genreRepository.UpdateGenre(genre))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult DeleteGenre([FromBody] JustIdDto genreDelete)
        {
            if (!genreRepository.GenreExists(genreDelete.Id))
                return NotFound($"Not found genre with such id {genreDelete.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genre = genreRepository.GetGenreById(genreDelete.Id);

            if (!genreRepository.DeleteGenre(genre))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
