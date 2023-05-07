﻿using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

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
        [HttpGet("platfrormAll")]
        [ProducesResponseType(200, Type = typeof(IList<PlatformDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPlatforms()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Platforms = mapper.Map<List<PlatformDto>>(await platformRepository.GetPlatformsAsync());

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
        public async Task<IActionResult> GetPersonGamesByList(int platformId)
        {
            if (!await platformRepository.PlatformExistAsync(platformId))
                return NotFound($"Not found platform with such id {platformId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Platform = mapper.Map<PlatformDto>(await platformRepository.GetPlatformByIdAsync(platformId));

            return Ok(Platform);
        }

        /// <summary>
        /// Return games on specified platform 
        /// </summary>
        /// <param name="platformId"></param>
        /// <param name="filterParameters"></param>
        /// <returns></returns>
        [HttpGet("{platformId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPlatformGameOrderByRating(int platformId, [FromQuery] FilterParameters filterParameters)
        {
            if (!await platformRepository.PlatformExistAsync(platformId))
                return NotFound($"Not found platform with such id {platformId}");

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

            var games = await gameRepository.GetGameByPlatformAsync(platformId, filterParameters);

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
        /// Creates new Platform
        /// </summary>
        /// <param name="platformCreate"></param>
        /// <returns></returns>
        [HttpPost("createPlatform")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePLatform([FromBody] PlatformCreateDto platformCreate)
        {
            if (platformCreate == null)
                return BadRequest(ModelState);

            var platform = await platformRepository.GetPlatformByNameAsync(platformCreate.Name);

            if (platform != null)
                return BadRequest("Platform already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var platformMap = mapper.Map<Platform>(platformCreate);

            platformRepository.CreatePlatform(platformMap);
            await platformRepository.SavePlatformAsync();

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
        public async Task<IActionResult> UpdatePlatformInfo([FromBody] CommonUpdate platformUpdate)
        {
            if (platformUpdate == null)
                return BadRequest(ModelState);

            if (!await platformRepository.PlatformExistAsync(platformUpdate.Id))
                return NotFound($"Not found platform with such id {platformUpdate.Id}");

            if (await platformRepository.PlatformNameAlredyInUseAsync(platformUpdate.Id, platformUpdate.Name))
                return BadRequest("Name already in use");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var platform = await platformRepository.GetPlatformByIdAsync(platformUpdate.Id);

            platform.Name = platformUpdate.Name;
            platform.Description = platformUpdate.Description;

            platformRepository.UpdatePlatfrom(platform);
            await platformRepository.SavePlatformAsync();

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
        public async Task<IActionResult> DeletePlatform([FromBody] JustIdDto platfromDelete)
        {
            if (!await platformRepository.PlatformExistAsync(platfromDelete.Id))
                return NotFound($"Not found platform with such id {platfromDelete.Id}");

            if (!ModelState.IsValid)
                return BadRequest();

            var platform = await platformRepository.GetPlatformByIdAsync(platfromDelete.Id);

            var pg = await personGameRepository.GetAllPersonGamesAsync();
            var personGames = pg.Where(pg => pg.PlayedPlatform!.Id == platform.Id).ToList();

            foreach (var item in personGames)
            {
                item.PlayedPlatform = null;
                personGameRepository.UpdatePersonGame(item);
            }
            await personGameRepository.SavePersonGameAsync();

            platformRepository.DeletePlatform(platform);
            await platformRepository.SavePlatformAsync();

            return Ok("Successfully deleted");
        }
    }
}
