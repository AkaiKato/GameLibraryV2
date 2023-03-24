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
    public class PlatformController : Controller
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public PlatformController(IPlatformRepository _platformRepository, IGameRepository _gameRepository,IMapper _mapper)
        {
            platformRepository = _platformRepository;
            gameRepository = _gameRepository;
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
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPlatformGame(int platformId)
        {
            if (!platformRepository.PlatformExist(platformId))
                return NotFound();

            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGameByPlatform(platformId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }

        /// <summary>
        /// Creates new Platform
        /// </summary>
        /// <param name="platformCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePLatform([FromBody] PlatformCreateDto platformCreate)
        {
            if (platformCreate == null)
                return BadRequest(ModelState);

            var platform = platformRepository.GetPlatformByName(platformCreate.Name);

            if (platform != null)
            {
                ModelState.AddModelError("", "Platform already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var platformMap = mapper.Map<Platform>(platformCreate);

            if (!platformRepository.CreatePlatform(platformMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
