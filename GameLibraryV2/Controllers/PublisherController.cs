using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        [HttpGet("publisherAll")]
        public async Task<IActionResult> GetPublishers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Publisbhers = mapper.Map<List<PublisherDto>>(await publisherRepository.GetPublishersAsync());

            return Ok(Publisbhers);
        }

        /// <summary>
        /// Get specified Publisher
        /// </summary>
        /// <param name="publisherId"></param>
        /// <returns></returns>
        [HttpGet("{publisherId}")]
        public async Task<IActionResult> GetPublisherById(int publisherId)
        {
            if (!await publisherRepository.PublisherExistsAsync(publisherId))
                return NotFound($"Not found publisher with such id {publisherId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Publisbher = mapper.Map<PublisherDto>(await publisherRepository.GetPublisherByIdAsync(publisherId));

            return Ok(Publisbher);
        }

        /// <summary>
        /// Get all publisher Games
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="filterParameters"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("{publisherId}/games")]
        public async Task<IActionResult> GetPublisherGames(int publisherId, [FromBody] FilterParameters filterParameters)
        {
            if (!await publisherRepository.PublisherExistsAsync(publisherId))
                return NotFound($"Not found publisher with such id {publisherId}");

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

            var games = await gameRepository.GetGamesByPublisherAsync(publisherId, filterParameters);

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
        /// Creates new Publisher
        /// </summary>
        /// <param name="publisherCreate"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost("createPublisher")]
        public async Task<IActionResult> CreatePublisher([FromBody] PublisherCreateDto publisherCreate)
        {
            if (publisherCreate == null)
                return BadRequest(ModelState);

            var publisher = await publisherRepository.GetPublisherByNameAsync(publisherCreate.Name);

            if (publisher != null)
                return BadRequest("Publisher already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var publisherMap = mapper.Map<Publisher>(publisherCreate);

            publisherRepository.CreatePublisher(publisherMap);
            await publisherRepository.SavePublisherAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified publisher
        /// </summary>
        /// <param name="publisherUpdate"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("updatePublisher")]
        public async Task<IActionResult> UpdatePublisherInfo([FromBody] CommonUpdate publisherUpdate)
        {
            if (publisherUpdate == null)
                return BadRequest(ModelState);

            if (!await publisherRepository.PublisherExistsAsync(publisherUpdate.Id))
                return NotFound($"Not found publisher with such id {publisherUpdate.Id}");

            if (await publisherRepository.PublisherNameAlreadyExistsAsync(publisherUpdate.Id, publisherUpdate.Name))
                return BadRequest("Name already in use");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var developer = await publisherRepository.GetPublisherByIdAsync(publisherUpdate.Id);

            developer.Name = publisherUpdate.Name;
            developer.Description = publisherUpdate.Description;

            publisherRepository.UpdatePublisher(developer);
            await publisherRepository.SavePublisherAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified publisher
        /// </summary>
        /// <param name="deletePublisher"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("deletePublisher")]
        public async Task<IActionResult> DeletePublisher([FromQuery] int deletePublisher)
        {
            if (!await publisherRepository.PublisherExistsAsync(deletePublisher))
                return NotFound($"Not found publisher with such id {deletePublisher}");

            var publisher = await publisherRepository.GetPublisherByIdAsync(deletePublisher);

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

            publisherRepository.DeletePublisher(publisher);
            await publisherRepository.SavePublisherAsync();

            return Ok("Successfully deleted");
        }
    }
}
