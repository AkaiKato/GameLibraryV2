using AutoMapper;
using GameLibraryV2.Dto;
using GameLibraryV2.Dto.smallInfo;
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
        private readonly IMapper mapper;

        public PublisherController(IPublisherRepository _publisherRepository, IMapper _mapper)
        {
            publisherRepository = _publisherRepository;
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
        /// Get specified publisher picture
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("{publisherId}/picture")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetPublisherPicturePath(int publisherId)
        {
            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            var PicturePath = publisherRepository.GetPublisherPicturePath(publisherId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(PicturePath));
        }

        /// <summary>
        /// Get specified publisher mini picture
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("{publisherId}/minipicture")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetPublisherMiniPicturePath(int publisherId)
        {
            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            var MiniPicturePath = publisherRepository.GetPublisherMiniPicturePath(publisherId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(MiniPicturePath));
        }

        /// <summary>
        /// Get all publisher Games
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("{publisherId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPublisherGames(int publisherId)
        {
            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound();

            var Games = mapper.Map<List<GameListDto>>(publisherRepository.GetGamesByPublisher(publisherId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }
    }
}
