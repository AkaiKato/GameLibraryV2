using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static GameLibraryV2.Helper.Enums;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterAndConstsController : Controller
    {
        private readonly IGameRepository gameRepository;
        private readonly IDeveloperRepository developerRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ITagRepository tagRepository;
        private readonly IPlatformRepository platformRepository;
        private readonly IAgeRatingRepository ageRatingRepository;
        private readonly IMapper mapper;

        public FilterAndConstsController(IGameRepository _gameRepository,
            IDeveloperRepository _developerRepository,
            IPublisherRepository _publisherRepository,
            IGenreRepository _genreRepository,
            ITagRepository _tagRepository,
            IPlatformRepository _platformRepository,
            IAgeRatingRepository _ageRatingRepository,
            IMapper _mapper)
        {
            gameRepository = _gameRepository;
            developerRepository = _developerRepository;
            publisherRepository = _publisherRepository;
            platformRepository = _platformRepository;
            genreRepository = _genreRepository;
            tagRepository = _tagRepository;
            ageRatingRepository = _ageRatingRepository;
            mapper = _mapper;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilter()
        {
            var sortBy = new string[]
            {
                "rating",
                "alphabet"
            };

            var status = new List<string>
            {
                Status.announсed.ToString(),
                Status.released.ToString()
            };

            var type = new List<string>
            {
                Types.game.ToString(),
                Types.dlc.ToString()
            };

            var rating = "rating";
            var releaseYear = "releaseYear";
            var playTime = "playTime";

            var genres = mapper.Map<List<GenreDto>>(await genreRepository.GetGenresAsync());
            var tags = mapper.Map<List<TagDto>>(await tagRepository.GetTagsAsync());
            var platforms = mapper.Map<List<PlatformDto>>(await platformRepository.GetPlatformsAsync());
            var developers = mapper.Map<List<DeveloperDto>>(await developerRepository.GetDevelopersAsync());
            var publisbhers = mapper.Map<List<PublisherDto>>(await publisherRepository.GetPublishersAsync());
            var ageRatings = await ageRatingRepository.GetAgeRatingsAsync();
            var nsfw = "nsfw";

            var Filter = new
            {
                sortBy,
                status,
                type,
                rating,
                releaseYear,
                playTime,
                genres,
                tags,
                platforms,
                developers,
                publisbhers,
                ageRatings,
                nsfw,
            };

            return Ok(Filter);
        }

        [HttpGet("consts")]
        public async Task<IActionResult> GetConsts()
        {
            var status = new List<string>
            {
                Status.announсed.ToString(),
                Status.released.ToString()
            };

            var type = new List<string>
            {
                Types.game.ToString(),
                Types.dlc.ToString()
            };

            var list = new List<string>
            {
                List.planned.ToString(),
                List.playing.ToString(),
                List.completed.ToString(),
                List.dropped.ToString(),
                List.onhold.ToString(),
            };

            var genders = new List<string>
            {
                Genders.male.ToString(),
                Genders.female.ToString(),
            };

            var roles = new List<string>
            {
                Roles.user.ToString(),
                Roles.admin.ToString(),
            };

            var consts = new
            {
                list,
                type,
                genders,
                roles,
                status,
            };

            return Ok(consts);
        }
    }
}
