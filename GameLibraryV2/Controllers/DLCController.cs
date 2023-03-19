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
    public class DLCController : Controller
    {
        private readonly IDLCRepository dlcRepository;
        private readonly IMapper mapper;

        public DLCController(IDLCRepository _dlcRepository, IMapper _mapper)
        {
            dlcRepository = _dlcRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all DLC
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<GameListDto>))]
        public IActionResult GetDLCs()
        {
            var DLC = mapper.Map<List<GameListDto>>(dlcRepository.GetDLCs());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(DLC));
        }

        /// <summary>
        /// Return specified DLC
        /// </summary>
        /// <param name="dlcId"></param>
        /// <returns></returns>
        [HttpGet("{dlcId}")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDLCById(int dlcId)
        {
            if(!dlcRepository.DLCExists(dlcId))
                return NotFound();

            var DLC = mapper.Map<GameDto>(dlcRepository.GetDLCById(dlcId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(DLC));
        }

    }
}
