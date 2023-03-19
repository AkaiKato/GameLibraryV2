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
    public class TagController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IMapper mapper;

        public TagController(ITagRepository _tagRepository, IMapper _mapper)
        {
            tagRepository = _tagRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<TagDto>))]
        public IActionResult GetGenres()
        {
            var Tags = mapper.Map<List<TagDto>>(tagRepository.GetTags());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Tags));
        }

        /// <summary>
        /// Return specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(TagDto))]
        [ProducesResponseType(400)]
        public IActionResult GetGenreById(int tagId)
        {
            if (!tagRepository.TagExists(tagId))
                return NotFound();

            var Tag = mapper.Map<TagDto>(tagRepository.GetTagById(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Tag));
        }

        /// <summary>
        /// Return all tag games
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameListDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetGenreGames(int tagId)
        {
            if (!tagRepository.TagExists(tagId))
                return NotFound();

            var Games = mapper.Map<List<GameListDto>>(tagRepository.GetGamesByTag(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Games));
        }
    }
}
