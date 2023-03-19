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
    public class PlatformController : Controller
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;

        public PlatformController(IPlatformRepository _platformRepository, IMapper _mapper)
        {
            platformRepository = _platformRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all platforms
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<PlatformDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPlatforms()
        {
            var Platforms = mapper.Map<List<PlatformDto>>(platformRepository.GetPlatforms());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Platforms));
        }

        /// <summary>
        /// Return specified platform
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        [HttpGet("{platformId}")]
        [ProducesResponseType(200, Type = typeof(PlatformDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPersonGamesByList(int platformId)
        {
            if (!platformRepository.PlatformExist(platformId))
                return NotFound();

            var Platform = mapper.Map<PlatformDto>(platformRepository.GetPlatformById(platformId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Platform));
        }


        /// <summary>
        /// Return games on specified platform
        /// </summary>
        /// <param name="platformId"></param>
        /// <returns></returns>
        [HttpGet("{platformId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPlatformGame(int platformId)
        {
            if (!platformRepository.PlatformExist(platformId))
                return NotFound();

            var Games = mapper.Map<List<GameListDto>>(platformRepository.GetGameByPlatform(platformId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }
    }
}
