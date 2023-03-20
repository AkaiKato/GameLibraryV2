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
    public class DeveloperController : Controller
    {
        private readonly IDeveloperRepository developerRepository;
        private readonly IMapper mapper;

        public DeveloperController(IDeveloperRepository _developerRepository, IMapper _mapper)
        {
            developerRepository = _developerRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Developers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<DeveloperDto>))]
        public IActionResult GetDevelopers()
        {
            var Developers = mapper.Map<List<DeveloperDto>>(developerRepository.GetDevelopers());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Developers));
        }

        /// <summary>
        /// Return specified developer
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [HttpGet("{developerId}")]
        [ProducesResponseType(200, Type = typeof(DeveloperDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDeveloperById(int developerId) 
        {
            if(!developerRepository.DeveloperExists(developerId))
                return NotFound();

            var Developer = mapper.Map<DeveloperDto>(developerRepository.GetDeveloperById(developerId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Developer));
        }

        /// <summary>
        /// Return specified developer picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [HttpGet("{developerId}/picturePath")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetDeveloperPicture(int developerId)
        {
            if (!developerRepository.DeveloperExists(developerId))
                return NotFound();

            var PicturePath = developerRepository.GetDeveloperPicturePath(developerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PicturePath));
        }

        /// <summary>
        /// Return specified developer mini picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [HttpGet("{developerId}/miniPicturePath")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetDeveloperMiniPicture(int developerId)
        {
            if (!developerRepository.DeveloperExists(developerId))
                return NotFound();

            var PicturePath = developerRepository.GetDeveloperMiniPicturePath(developerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PicturePath));
        }

        /// <summary>
        /// Return all developer games
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [HttpGet("{developerId}/Games")]
        [ProducesResponseType(200, Type = typeof(List<GameListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetDeveloperGemes(int developerId)
        {
            if (!developerRepository.DeveloperExists(developerId))
                return NotFound();

            var DeveloperGames = mapper.Map<List<GameListDto>>(developerRepository.GetGamesByDeveloper(developerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(DeveloperGames));
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDeveloper([FromBody] DeveloperDto developerCreate)
        {
            if (developerCreate == null)
                return BadRequest(ModelState);

            var developer = developerRepository.GetDeveloperByName(developerCreate.Name);

            if(developer != null)
            {
                ModelState.AddModelError("", "Developer already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var developerMap = mapper.Map<Developer>(developerCreate);

            if (!developerRepository.CreateDeveloper(developerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}
