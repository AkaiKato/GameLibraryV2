using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GameLibraryV2.Helper;

namespace GameLibraryTest.Controller
{
    public class PlatformControllerTest
    {
        private readonly Mock<IPlatformRepository> platformRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IPersonGamesRepository> personGamesRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task PlatformController_GetPlatforms_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Platform>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomPlatform());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<PlatformDto>>(expectedItems);

            platformRepositoryMock.Setup(r => r.GetPlatformsAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<PlatformDto>>(expectedItems)).Returns(mappedExpected);

            var controller = PlatformControllerCreate();

            var result = await controller.GetPlatforms();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<PlatformDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task PlatformController_GetGenreById_ReturnNotFound()
        {
            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = PlatformControllerCreate();

            var result = await controller.GetPlatformById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PlatformController_GetGenreById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomPlatform();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<PlatformDto>(expectedItem);

            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<PlatformDto>(expectedItem)).Returns(mappedExpected);

            var controller = PlatformControllerCreate();

            var result = await controller.GetPlatformById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as PlatformDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task PlatformController_GetPlatformGame_ReturnNotFound()
        {
            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = PlatformControllerCreate();

            var result = await controller.GetPlatformGame(rand.Next(100), filter.Object);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PlatformController_GetPlatformGame_ReturnGameList()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Game>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomGame());
            }
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();
            var Response = new Mock<HttpResponse>();
            var expectedPagedList = help.ReturnPagedList(expectedItems);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var expectedMapped = mapper.Map<List<GameSmallListDto>>(expectedItems);

            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGameByPlatformAsync(It.IsAny<int>(),
                It.IsAny<FilterParameters>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = PlatformControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetPlatformGame(rand.Next(100), filter.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task PlatformController_CreatePLatform_ReturnBadRequest()
        {
            var expectedItem = help.CreateRandomPlatform();
            platformRepositoryMock.Setup(r => r.GetPlatformByNameAsync(It.IsAny<string>()))!.ReturnsAsync(expectedItem);

            var controller = PlatformControllerCreate();

            var result = await controller.CreatePLatform(CreateRandomPlatformCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PlatformController_CreatePLatform_ReturnOk()
        {
            platformRepositoryMock.Setup(r => r.GetPlatformByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Platform)null!);

            var controller = PlatformControllerCreate();

            var result = await controller.CreatePLatform(CreateRandomPlatformCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PlatformController_UpdatePlatformInfo_ReturnNotFound()
        {
            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PlatformControllerCreate();

            var result = await controller.UpdatePlatformInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PlatformController_UpdatePlatformInfo_ReturnBadRequest()
        {
            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            platformRepositoryMock.Setup(r => r.PlatformNameAlredyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = PlatformControllerCreate();

            var result = await controller.UpdatePlatformInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PlatformController_UpdatePlatformInfo_ReturnOk()
        {
            var expectedItem = help.CreateRandomPlatform();
            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            platformRepositoryMock.Setup(r => r.PlatformNameAlredyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = PlatformControllerCreate();

            var result = await controller.UpdatePlatformInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PlatformController_DeletePlatform_ReturnNotFound()
        {
            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PlatformControllerCreate();

            var result = await controller.DeletePlatform(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PlatformController_DeletePlatform_ReturnOk()
        {
            var expectedItem = help.CreateRandomPlatform();
            var pgame = help.CreateRandomPersonGame();
            var pg = new List<PersonGame>() { pgame };

            platformRepositoryMock.Setup(r => r.PlatformExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);
            personGamesRepositoryMock.Setup(r => r.GetAllPersonGamesAsync())!.ReturnsAsync(pg);

            var controller = PlatformControllerCreate();

            var result = await controller.DeletePlatform(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private PlatformController PlatformControllerCreate()
        {
            return new PlatformController(platformRepositoryMock.Object,
                gameRepositoryMock.Object, personGamesRepositoryMock.Object,
                mapperMock.Object);
        }

        private PlatformCreateDto CreateRandomPlatformCreateDto()
        {
            return new PlatformCreateDto
            {
                Name = help.RandomString(20),
            };
        }
    }
}
