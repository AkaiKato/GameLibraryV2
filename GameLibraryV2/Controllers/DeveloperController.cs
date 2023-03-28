using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        /// Return all developer games
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [HttpGet("{developerId}/Games")]
        [ProducesResponseType(200, Type = typeof(List<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetDeveloperGemes(int developerId)
        {
            if (!developerRepository.DeveloperExists(developerId))
                return NotFound();

            var DeveloperGames = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGamesByDeveloper(developerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(DeveloperGames));
        }

        /// <summary>
        /// Create new Developer
        /// </summary>
        /// <param name="developerCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDeveloper([FromBody] DeveloperCreateDto developerCreate)
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

        /// <summary>
        /// Update specified developer
        /// </summary>
        /// <param name="developerUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateDeveloper")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateDeveloperInfo([FromBody] CommonUpdate developerUpdate)
        {
            if (developerUpdate == null)
                return BadRequest(ModelState);

            if (!developerRepository.DeveloperExists(developerUpdate.Id))
                return NotFound();

            if (developerRepository.DeveloperNameAlreadyExists(developerUpdate.Id, developerUpdate.Name))
            {
                ModelState.AddModelError("", $"Name already in use");
                return StatusCode(422, ModelState);
            }

            var developer = developerRepository.GetDeveloperById(developerUpdate.Id);

            developer.Name = developerUpdate.Name;
            developer.Description = developerUpdate.Description;

            if (!developerRepository.UpdateDeveloper(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update developer picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadDeveloperPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadDeveloperPicture([FromQuery] int developerId, IFormFile pic)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("", "Unsupported extension");
                return StatusCode(422, ModelState);
            }

            if (!developerRepository.DeveloperExists(developerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var developer = developerRepository.GetDeveloperById(developerId);
            var newfilePath = $"\\Images\\developerPicture\\{unique}{ext}";
            var oldfilePath = developer.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            developer!.PicturePath = newfilePath;

            if (!developerRepository.UpdateDeveloper(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\developerPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }


            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update developer mini picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadDeveloperMiniPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadDeveloperMiniPicture([FromQuery] int developerId, IFormFile pic)
        {
            string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("", "Unsupported extension");
                return StatusCode(422, ModelState);
            }

            if (!developerRepository.DeveloperExists(developerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var developer = developerRepository.GetDeveloperById(developerId);
            var newfilePath = $"\\Images\\developerMiniPicture\\{unique}{ext}";
            var oldfilePath = developer.MiniPicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            developer!.MiniPicturePath = newfilePath;

            if (!developerRepository.UpdateDeveloper(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\developerMiniPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

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
        public IActionResult DeleteUser([FromBody] JustIdDto developerDelete)
        {
            if (!developerRepository.DeveloperExists(developerDelete.Id))
                return NotFound();

            var developer = developerRepository.GetDeveloperById(developerDelete.Id);

            if (!ModelState.IsValid)
                return BadRequest();

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

            if (!developerRepository.DeleteDeveloper(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
