using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonGameController : Controller
    {
        private readonly IPersonGamesRepository personGameRepository;
        private readonly IMapper mapper;

        public PersonGameController(IPersonGamesRepository _personGameRepository, IMapper _mapper)
        {
            personGameRepository = _personGameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Person Games
        /// </summary>
        /// <returns></returns>
        [HttpGet("{libraryId}/persongames")]
        [ProducesResponseType(200, Type = typeof(IList<PersonGameDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPersonGames(int libraryId)
        {
            if(!personGameRepository.PersonLibraryExists(libraryId))
                return NotFound();

            var PersonGames = mapper.Map<List<PersonGameDto>>(personGameRepository.PersonGames(libraryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PersonGames));
        }

        /// <summary>
        /// Return all Person Games By List
        /// </summary>
        /// <param name="libraryId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpGet("{libraryId}/persongamesbylist")]
        [ProducesResponseType(200, Type = typeof(IList<PersonGameDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPersonGamesByList(int libraryId, string list) 
        {
            if(!personGameRepository.PersonLibraryExists(libraryId))
                return NotFound();

            var PersonGamesByList = mapper.Map<List<PersonGameDto>>(personGameRepository.PersonGamesByList(libraryId, list));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PersonGamesByList));
        }
    }
}
