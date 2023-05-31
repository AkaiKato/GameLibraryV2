using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class SearchControllerTest
    {
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IDeveloperRepository> developerRepositoryMock = new();
        private readonly Mock<IPublisherRepository> publisherRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task SearchController_Search_ReturnExpectedItems()
        {
            int numberItemsExpected = 5;
            var gameList = new List<Game>();
            var developerList = new List<Developer>();
            var publisherList = new List<Publisher>();
            var userList = new List<User>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                gameList.Add(help.CreateRandomGame());
                developerList.Add(help.CreateRandomDeveloper());
                publisherList.Add(help.CreateRandomPublisher());
                userList.Add(help.CreateRandomUser());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var games = mapper.Map<List<GameSmallListDto>>(gameList);
            var developers = mapper.Map<List<DeveloperDto>>(developerList);
            var publishers = mapper.Map<List<PublisherDto>>(publisherList);
            var users = mapper.Map<List<UserDto>>(userList);

            gameRepositoryMock.Setup(r => r.GetGamesThatContainsStringAsync(It.IsAny<string>())).ReturnsAsync(gameList);
            developerRepositoryMock.Setup(r => r.GetDevelopersThatContainsStringAsync(It.IsAny<string>())).ReturnsAsync(developerList);
            publisherRepositoryMock.Setup(r => r.GetDevelopersThatContainsStringAsync(It.IsAny<string>())).ReturnsAsync(publisherList);
            userRepositoryMock.Setup(r => r.GetDevelopersThatContainsStringAsync(It.IsAny<string>())).ReturnsAsync(userList);

            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(gameList)).Returns(games);
            mapperMock.Setup(r => r.Map<List<DeveloperDto>>(developerList)).Returns(developers);
            mapperMock.Setup(r => r.Map<List<PublisherDto>>(publisherList)).Returns(publishers);
            mapperMock.Setup(r => r.Map<List<UserDto>>(userList)).Returns(users);

            var searchExpected = new
            {
                games,
                developers,
                publishers,
                users,
            };

            var controller = SearchControllerCreate();

            var result = await controller.Search("Hey Guys");

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = okResult!.Value;
            verifyRes.Should().BeEquivalentTo(searchExpected);
        }

        private SearchController SearchControllerCreate()
        {
            return new SearchController (gameRepositoryMock.Object, 
                developerRepositoryMock.Object, publisherRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);
        }
    }
}
