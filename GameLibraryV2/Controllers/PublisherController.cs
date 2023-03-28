using AutoMapper;
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
    public class PublisherController : Controller
    {
        private readonly IPublisherRepository publisherRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public PublisherController(IPublisherRepository _publisherRepository, IGameRepository _gameRepository,IMapper _mapper)
        {
            publisherRepository = _publisherRepository;
            gameRepository = _gameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Get all Publishers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<PublisherDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPublishers()
        {
            var Publisbhers = mapper.Map<List<PublisherDto>>(publisherRepository.GetPublishers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Publisbhers));
        }

        /// <summary>
        /// Get specified Publisher
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("{publisherId}")]
        [ProducesResponseType(200, Type = typeof(PublisherDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPublisherById(int publisherId)
        {
            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            var Publisbher = mapper.Map<PublisherDto>(publisherRepository.GetPublisherById(publisherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Publisbher));
        }

        /// <summary>
        /// Get all publisher Games
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("{publisherId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPublisherGames(int publisherId)
        {
            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGamesByPublisher(publisherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }

        /// <summary>
        /// Creates new Publisher
        /// </summary>
        /// <param name="publisherCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePublisher([FromBody] PublisherCreateDto publisherCreate)
        {
            if (publisherCreate == null)
                return BadRequest(ModelState);

            var publisher = publisherRepository.GetPublisherByName(publisherCreate.Name);

            if (publisher != null)
            {
                ModelState.AddModelError("", "Publisher already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var publisherMap = mapper.Map<Publisher>(publisherCreate);

            if (!publisherRepository.CreatePublisher(publisherMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified publisher
        /// </summary>
        /// <param name="publisherUpdate"></param>
        /// <returns></returns>
        [HttpPut("updatePublisher")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePublisherInfo([FromBody] CommonUpdate publisherUpdate)
        {
            if (publisherUpdate == null)
                return BadRequest(ModelState);

            if (!publisherRepository.PublisherExists(publisherUpdate.Id))
                return NotFound();

            if (publisherRepository.PublisherNameAlreadyExists(publisherUpdate.Id, publisherUpdate.Name))
            {
                ModelState.AddModelError("", $"Name already in use");
                return StatusCode(422, ModelState);
            }

            var developer = publisherRepository.GetPublisherById(publisherUpdate.Id);

            developer.Name = publisherUpdate.Name;
            developer.Description = publisherUpdate.Description;

            if (!publisherRepository.UpdatePublisher(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update publisher picture
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadPublisherPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadPublisherPicture([FromQuery] int publisherId, IFormFile pic)
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

            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var publisher = publisherRepository.GetPublisherById(publisherId);
            var newfilePath = $"\\Images\\publisherPicture\\{unique}{ext}";
            var oldfilePath = publisher.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);
            publisher!.PicturePath = newfilePath;

            if (!publisherRepository.UpdatePublisher(publisher))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\publisherPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }


            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update publisher mini picture
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadPublisherMiniPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UploadPublihserMiniPicture([FromQuery] int publisherId, IFormFile pic)
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

            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var publisher = publisherRepository.GetPublisherById(publisherId);
            var newfilePath = $"\\Images\\publisherMiniPicture\\{unique}{ext}";
            var oldfilePath = publisher.MiniPicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);
            publisher!.MiniPicturePath = newfilePath;

            if (!publisherRepository.UpdatePublisher(publisher))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\publisherMiniPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified publisher
        /// </summary>
        /// <param name="deletePublisher"></param>
        /// <returns></returns>
        [HttpDelete("deletePublisher")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeletePublisher([FromBody] JustIdDto deletePublisher)
        {
            if (!publisherRepository.PublisherExists(deletePublisher.Id))
                return NotFound();

            var publisher = publisherRepository.GetPublisherById(deletePublisher.Id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (publisher.PicturePath != $"\\Images\\publisherPicture\\Def.jpg")
            {
                FileInfo f = new(publisher.PicturePath);
                f.Delete();
            }

            if (publisher.MiniPicturePath != $"\\Images\\publisherMiniPicture\\Def.jpg")
            {
                FileInfo f = new(publisher.MiniPicturePath);
                f.Delete();
            }

            if (!publisherRepository.DeletePublisher(publisher))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
