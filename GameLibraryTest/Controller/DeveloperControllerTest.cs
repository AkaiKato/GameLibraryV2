using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.Common;

namespace GameLibraryTest.Controller
{
    public class DeveloperControllerTest
    {
        private readonly Mock<IDeveloperRepository> developerRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task DeveloperController_GetDevelopers_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Developer>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomDeveloper());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<DeveloperDto>>(expectedItems);

            developerRepositoryMock.Setup(r => r.GetDevelopersAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<DeveloperDto>>(expectedItems)).Returns(mappedExpected);

            var controller = DeveloperControllerCreate();

            var result = await controller.GetDevelopers();

            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<DeveloperDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task DeveloperController_GetDeveloperById_ReturnNotFound()
        {
            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = DeveloperControllerCreate();

            var result = await controller.GetDeveloperById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_GetDeveloperById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomDeveloper();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<DeveloperDto>(expectedItem);

            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<DeveloperDto>(expectedItem)).Returns(mappedExpected);

            var controller = DeveloperControllerCreate();

            var result = await controller.GetDeveloperById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as DeveloperDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        
        [Fact]
        public async Task DeveloperController_GetDeveloperGames_ReturnNotFound()
        {
            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = DeveloperControllerCreate();

            var result = await controller.GetDeveloperGames(rand.Next(100), filter.Object);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        
        [Fact]
        public async Task DeveloperController_GetDeveloperGames_ReturnGameList()
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

            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGamesByDeveloperAsync(It.IsAny<int>(),
                It.IsAny<FilterParameters>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = DeveloperControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetDeveloperGames(rand.Next(100), filter.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }
        
        [Fact]
        public async Task DeveloperController_CreateDeveloper_ReturnBadRequest()
        {
            var expectedItem = help.CreateRandomDeveloper();
            developerRepositoryMock.Setup(r => r.GetDeveloperByNameAsync(It.IsAny<string>()))!.ReturnsAsync(expectedItem);

            var controller = DeveloperControllerCreate();

            var result = await controller.CreateDeveloper(CreateRandomDeveloperCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_CreateDeveloper_ReturnOk()
        {
            developerRepositoryMock.Setup(r => r.GetDeveloperByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Developer)null!);

            var controller = DeveloperControllerCreate();

            var result = await controller.CreateDeveloper(CreateRandomDeveloperCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_UpdateDeveloperInfo_ReturnNotFound()
        {
            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = DeveloperControllerCreate();

            var result = await controller.UpdateDeveloperInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_UpdateDeveloperInfo_ReturnBadRequest()
        {
            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.DeveloperNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = DeveloperControllerCreate();

            var result = await controller.UpdateDeveloperInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_UpdateDeveloperInfo_ReturnOk()
        {
            var expectedItem = help.CreateRandomDeveloper();
            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.DeveloperNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = DeveloperControllerCreate();

            var result = await controller.UpdateDeveloperInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_DeleteDeveloper_ReturnNotFound()
        {
            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = DeveloperControllerCreate();

            var result = await controller.DeleteDeveloper(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task DeveloperController_DeleteDeveloper_ReturnOk()
        {
            var expectedItem = help.CreateRandomDeveloper();

            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = DeveloperControllerCreate();

            var result = await controller.DeleteDeveloper(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private DeveloperController DeveloperControllerCreate()
        {
            return new DeveloperController(developerRepositoryMock.Object,
                gameRepositoryMock.Object, mapperMock.Object);
        }

        private DeveloperCreateDto CreateRandomDeveloperCreateDto()
        {
            return new DeveloperCreateDto
            {
                Name = help.RandomString(20),
            };
        }
    }
}
