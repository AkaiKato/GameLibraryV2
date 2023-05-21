using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class GlobalStatisticControllerTest
    {
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task GlobalStatisticController_GetNumberOfGamesAndDLC_ReturnOk()
        {
            var totalNumberOfGames = rand.Next(100);
            var numberOfGames = rand.Next(50);
            var numberOfDLC = rand.Next(50);

            gameRepositoryMock.Setup(r => r.GetTotalNumberOfGames()).ReturnsAsync(totalNumberOfGames);
            gameRepositoryMock.Setup(r => r.GetNumberOfGames()).ReturnsAsync(numberOfGames);
            gameRepositoryMock.Setup(r => r.GetNumberOfDLC()).ReturnsAsync(numberOfDLC);

            var expectedItem = new
            {
                totalNumberOfGames,
                numberOfGames,
                numberOfDLC,
            };

            var controller = GenreControllerCreate();

            var result = await controller.GetNumberOfGamesAndDLC();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var va = okResult!.Value;
            var verifyRes = okResult!.Value;
            verifyRes.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GlobalStatisticController_GetMostRatedGame_ReturnOk()
        {
            var expectedItem = help.CreateRandomGame();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<GameDto>(expectedItem);

            gameRepositoryMock.Setup(r => r.GetMostRatedGame()).ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<GameDto>(expectedItem)).Returns(mappedExpected);

            var controller = GenreControllerCreate();

            var result = await controller.GetMostRatedGame();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var va = okResult!.Value;
            var verifyRes = okResult!.Value as GameDto;
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task GlobalStatisticController_GetMostRatedDLC_ReturnOk()
        {
            var expectedItem = help.CreateRandomGame();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<GameDto>(expectedItem);

            gameRepositoryMock.Setup(r => r.GetMostRatedDLC()).ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<GameDto>(expectedItem)).Returns(mappedExpected);

            var controller = GenreControllerCreate();

            var result = await controller.GetMostRatedDLC();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var va = okResult!.Value;
            var verifyRes = okResult!.Value as GameDto;
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task GlobalStatisticController_GetMostRatedGameByYear_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Game>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomGame());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<GameDto>>(expectedItems);

            gameRepositoryMock.Setup(r => r.GetMostRatedGameByYear()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<GameDto>>(expectedItems)).Returns(mappedExpected);

            var controller = GenreControllerCreate();

            var result = await controller.GetMostRatedGameByYear();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<GameDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task GlobalStatisticController_GetMostRatedDLCByYear_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Game>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomGame());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<GameDto>>(expectedItems);

            gameRepositoryMock.Setup(r => r.GetMostRatedDLCByYear()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<GameDto>>(expectedItems)).Returns(mappedExpected);

            var controller = GenreControllerCreate();

            var result = await controller.GetMostRatedDLCByYear();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<GameDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        private GlobalStatisticController GenreControllerCreate()
        {
            return new GlobalStatisticController(gameRepositoryMock.Object, mapperMock.Object);
        }
    }
}
