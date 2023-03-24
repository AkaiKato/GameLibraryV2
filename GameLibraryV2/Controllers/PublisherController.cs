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
    }
}
