using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
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

            return Ok(Publisbhers);
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
                return NotFound($"Not found publisher with such id {publisherId}");

            var Publisbher = mapper.Map<PublisherDto>(publisherRepository.GetPublisherById(publisherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Publisbher);
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
                return NotFound($"Not found publisher with such id {publisherId}");

            var Games = mapper.Map<List<GameSmallListDto>>(gameRepository.GetGamesByPublisher(publisherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Games);
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
                return BadRequest("Publisher already exists");

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
                return NotFound($"Not found publisher with such id {publisherUpdate.Id}");

            if (publisherRepository.PublisherNameAlreadyExists(publisherUpdate.Id, publisherUpdate.Name))
                return BadRequest("Name already in use");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
                return NotFound($"Not found publisher with such id {deletePublisher.Id}");

            var publisher = publisherRepository.GetPublisherById(deletePublisher.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
