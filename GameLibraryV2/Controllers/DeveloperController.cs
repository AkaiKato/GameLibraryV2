﻿using AutoMapper;
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
    public class DeveloperController : Controller
    {
        private readonly IDeveloperRepository developerRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public DeveloperController(IDeveloperRepository _developerRepository, IGameRepository _gameRepository ,IMapper _mapper)
        {
            developerRepository = _developerRepository;
            gameRepository = _gameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Developers
        /// </summary>
        /// <returns></returns>
        [HttpGet("developerAll")]
        [ProducesResponseType(200, Type = typeof(IList<DeveloperDto>))]
        public async Task<IActionResult> GetDevelopers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Developers = mapper.Map<List<DeveloperDto>>(await developerRepository.GetDevelopersAsync());

            return Ok(Developers);
        }

        /// <summary>
        /// Return specified developer
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [HttpGet("{developerId}")]
        [ProducesResponseType(200, Type = typeof(DeveloperDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDeveloperById(int developerId) 
        {
            if(!await developerRepository.DeveloperExistsAsync(developerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Developer = mapper.Map<DeveloperDto>(await developerRepository.GetDeveloperByIdAsync(developerId));

            return Ok(Developer);
        }

        /// <summary>
        /// Return all developer games
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="filterParameters"></param>
        /// <returns></returns>
        [HttpGet("{developerId}/games")]
        [ProducesResponseType(200, Type = typeof(List<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDeveloperGames(int developerId, [FromQuery] FilterParameters filterParameters)
        {
            if (!await developerRepository.DeveloperExistsAsync(developerId))
                return NotFound();

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

            var games = await gameRepository.GetGamesByDeveloperAsync(developerId, filterParameters);

            var metadata = new
            {
                games.TotalCount,
                games.PageSize,
                games.CurrentPage,
                games.TotalPages,
                games.HasNext,
                games.HasPrevious,
            };

            var DeveloperGames = mapper.Map<List<GameSmallListDto>>(games);

            Response.Headers.Add("X-pagination", JsonSerializer.Serialize(metadata));

            return Ok(DeveloperGames);
        }

        /// <summary>
        /// Create new Developer
        /// </summary>
        /// <param name="developerCreate"></param>
        /// <returns></returns>
        [HttpPost("createDeveloper")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateDeveloper([FromBody] DeveloperCreateDto developerCreate)
        {
            if (developerCreate == null)
                return BadRequest(ModelState);

            var developer = await developerRepository.GetDeveloperByNameAsync(developerCreate.Name);

            if(developer != null)
                return BadRequest("Developer already exists");

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var developerMap = mapper.Map<Developer>(developerCreate);

            developerRepository.CreateDeveloper(developerMap);
            await developerRepository.SaveDeveloperAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified developer
        /// </summary>
        /// <param name="developerUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateDeveloper")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateDeveloperInfo([FromBody] CommonUpdate developerUpdate)
        {
            if (developerUpdate == null)
                return BadRequest(ModelState);

            if (!await developerRepository.DeveloperExistsAsync(developerUpdate.Id))
                return NotFound($"Not found developer with such id {developerUpdate.Id}");

            if (await developerRepository.DeveloperNameAlreadyExistsAsync(developerUpdate.Id, developerUpdate.Name))
                return BadRequest($"Name already in use");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var developer = await developerRepository.GetDeveloperByIdAsync(developerUpdate.Id);

            developer.Name = developerUpdate.Name;
            developer.Description = developerUpdate.Description;

            developerRepository.UpdateDeveloper(developer);
            await developerRepository.SaveDeveloperAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified developer
        /// </summary>
        /// <param name="developerDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteDeveloper")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteUser([FromBody] JustIdDto developerDelete)
        {
            if (!await developerRepository.DeveloperExistsAsync(developerDelete.Id))
                return NotFound($"Not found developer with such id {developerDelete.Id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var developer = await developerRepository.GetDeveloperByIdAsync(developerDelete.Id);

            if (developer.PicturePath != $"\\Images\\developerPicture\\Def.jpg")
            {
                FileInfo f = new(developer.PicturePath);
                f.Delete();
            }

            if (developer.MiniPicturePath != $"\\Images\\developerMiniPicture\\Def.jpg")
            {
                FileInfo f = new(developer.MiniPicturePath);
                f.Delete();
            }

            developerRepository.DeleteDeveloper(developer);
            await developerRepository.SaveDeveloperAsync();

            return Ok("Successfully deleted");
        }
    }
}
