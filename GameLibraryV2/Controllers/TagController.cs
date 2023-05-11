using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static GameLibraryV2.Helper.Enums;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public TagController(ITagRepository _tagRepository, IGameRepository _gameRepository,IMapper _mapper)
        {
            tagRepository = _tagRepository;
            gameRepository = _gameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Tags
        /// </summary>
        /// <returns></returns>
        [HttpGet("tagAll")]
        [ProducesResponseType(200, Type = typeof(IList<TagDto>))]
        public async Task<IActionResult> GetTags()
        {
            var Tags = mapper.Map<List<TagDto>>(await tagRepository.GetTagsAsync());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Tags);
        }

        /// <summary>
        /// Return specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet("{tagId}")]
        [ProducesResponseType(200, Type = typeof(TagDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTagById(int tagId)
        {
            if (!await tagRepository.TagExistsAsync(tagId))
                return NotFound($"Not found tag with such id {tagId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Tag = mapper.Map<TagDto>(await tagRepository.GetTagByIdAsync(tagId));

            return Ok(Tag);
        }

        /// <summary>
        /// Return all tag games
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="filterParameters"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet("{tagId}/games")]
        [ProducesResponseType(200, Type = typeof(IList<GameSmallListDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTagGames(int tagId, [FromQuery] FilterParameters filterParameters, [FromQuery] Pagination pagination)
        {
            if (!await tagRepository.TagExistsAsync(tagId))
                return NotFound($"Not found tag with such id {tagId}");

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

            var games = await gameRepository.GetGamesByTagAsync(tagId, filterParameters, pagination);

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
        /// Creates new Tag
        /// </summary>
        /// <param name="tagCreate"></param>
        /// <returns></returns>
        [HttpPost("createTag")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTag([FromBody] TagCreateDto tagCreate)
        {
            if (tagCreate == null)
                return BadRequest(ModelState);

            var tag = await tagRepository.GetTagByNameAsync(tagCreate.Name);

            if (tag != null)
                return BadRequest("Tag already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagMap = mapper.Map<Tag>(tagCreate);

            tagRepository.CreateTag(tagMap);
            await tagRepository.SaveTagAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update specified tag
        /// </summary>
        /// <param name="tagUpdate"></param>
        /// <returns></returns>
        [HttpPut("updateTag")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateTagInfo([FromBody] CommonUpdate tagUpdate)
        {
            if (tagUpdate == null)
                return BadRequest(ModelState);

            if (!await tagRepository.TagExistsAsync(tagUpdate.Id))
                return NotFound($"Not found tag with such id {tagUpdate.Id}");

            if (await tagRepository.TagNameAlreadyInUseAsync(tagUpdate.Id, tagUpdate.Name))
                return BadRequest("Name already in use");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = await tagRepository.GetTagByIdAsync(tagUpdate.Id);

            tag.Name = tagUpdate.Name;
            tag.Description = tagUpdate.Description;

            tagRepository.UpdateTag(tag);
            await tagRepository.SaveTagAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete specified tag
        /// </summary>
        /// <param name="tagDelete"></param>
        /// <returns></returns>
        [HttpDelete("deleteTag")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTag([FromQuery] int tagDelete)
        {
            if (!await tagRepository.TagExistsAsync(tagDelete))
                return NotFound($"Not found tag with such id {tagDelete}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var platform = await tagRepository.GetTagByIdAsync(tagDelete);

            tagRepository.DeleteTag(platform);
            await tagRepository.SaveTagAsync();

            return Ok("Successfully deleted");
        }
    }
}
