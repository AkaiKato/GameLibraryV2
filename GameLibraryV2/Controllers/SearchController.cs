using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IGameRepository gameRepository;
        private readonly IDeveloperRepository developerRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public SearchController(IGameRepository _gameRepository,
            IDeveloperRepository _developerRepository,
            IPublisherRepository _publisherRepository,
            IUserRepository _userRepository,
            IMapper _mapper)
        {
            gameRepository = _gameRepository;
            developerRepository = _developerRepository;
            publisherRepository = _publisherRepository;
            userRepository = _userRepository;
            mapper = _mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchString)
        {
            if (searchString == null)
                return Ok();

            var games = mapper.Map<List<GameSmallListDto>>((await gameRepository.GetGamesThatContainsStringAsync(searchString)).Take(5));
            var developers = mapper.Map<List<DeveloperDto>>((await developerRepository.GetDevelopersThatContainsStringAsync(searchString)).Take(5));
            var publishers = mapper.Map<List<PublisherDto>>((await publisherRepository.GetDevelopersThatContainsStringAsync(searchString)).Take(5));
            var users = mapper.Map<List<UserDto>>((await userRepository.GetDevelopersThatContainsStringAsync(searchString)).Take(5));

            var search = new
            {
                games,
                developers,
                publishers,
                users,
            };

            return Ok(search);
        }
    }
}
