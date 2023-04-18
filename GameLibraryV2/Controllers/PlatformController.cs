using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IGameRepository gameRepository;
        private readonly IPersonGamesRepository personGameRepository;
        private readonly IMapper mapper;

        public PlatformController(IPlatformRepository _platformRepository, 
            IGameRepository _gameRepository, IPersonGamesRepository _personGameRepository,
            IMapper _mapper)
        {
            platformRepository = _platformRepository;
            gameRepository = _gameRepository;
            personGameRepository = _personGameRepository;
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Platforms = mapper.Map<List<PlatformDto>>(platformRepository.GetPlatforms());

            return Ok(Platforms);
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
                return NotFound($"Not found platform with such id {platformId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Platform = mapper.Map<PlatformDto>(platformRepository.GetPlatformById(platformId));

            return Ok(Platform);
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
                return NotFound($"Not found platform with such id {platformId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGameByPlatform(platformId));

            return Ok(Games);
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
                return BadRequest("Platform already exists");

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

        /// <summary>
        /// Update specified platform
        /// </summary>
        /// <param name="platformUpdate"></param>
        /// <returns></returns>
        [HttpPut("updatePlatform")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePlatformInfo([FromBody] CommonUpdate platformUpdate)
        {
            if (platformUpdate == null)
                return BadRequest(ModelState);

            if (!platformRepository.PlatformExist(platformUpdate.Id))
                return NotFound($"Not found platform with such id {platformUpdate.Id}");

            if (platformRepository.PlatformNameAlredyInUse(platformUpdate.Id, platformUpdate.Name))
                return BadRequest("Name already in use");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var platform = platformRepository.GetPlatformById(platformUpdate.Id);

            platform.Name = platformUpdate.Name;
            platform.Description = platformUpdate.Description;

            if (!platformRepository.UpdatePlatfrom(platform))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified platform
        /// </summary>
        /// <param name="platfromDelete"></param>
        /// <returns></returns>
        [HttpDelete("deletePlatform")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeletePlatform([FromBody] JustIdDto platfromDelete)
        {
            if (!platformRepository.PlatformExist(platfromDelete.Id))
                return NotFound($"Not found platform with such id {platfromDelete.Id}");

            if (!ModelState.IsValid)
                return BadRequest();

            var platform = platformRepository.GetPlatformById(platfromDelete.Id);

            var pg = personGameRepository.GetAllPersonGames().Where(pg => pg.PlayedPlatform!.Id == platform.Id).ToList();

            foreach (var item in pg)
            {
                item.PlayedPlatform = null;
                if (!personGameRepository.UpdatePersonGame(item))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
            }

            if (!platformRepository.DeletePlatform(platform))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
