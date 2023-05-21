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
    public class PublisherControllerTest
    {
        private readonly Mock<IPublisherRepository> publishserRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task PublisherController_GetPublishers_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Publisher>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomPublisher());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<PublisherDto>>(expectedItems);

            publishserRepositoryMock.Setup(r => r.GetPublishersAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<PublisherDto>>(expectedItems)).Returns(mappedExpected);

            var controller = PublisherControllerCreate();

            var result = await controller.GetPublishers();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<PublisherDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task PublisherController_GetPublisherById_ReturnNotFound()
        {
            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = PublisherControllerCreate();

            var result = await controller.GetPublisherById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PublisherController_GetPublisherById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomPublisher();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<PublisherDto>(expectedItem);

            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            publishserRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<PublisherDto>(expectedItem)).Returns(mappedExpected);

            var controller = PublisherControllerCreate();

            var result = await controller.GetPublisherById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as PublisherDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task PublisherController_GetPublisherGames_ReturnNotFound()
        {
            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = PublisherControllerCreate();

            var result = await controller.GetPublisherGames(rand.Next(100), filter.Object, pagination.Object);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PublisherController_GetPublisherGames_ReturnGameList()
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

            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGamesByPublisherAsync(It.IsAny<int>(),
                It.IsAny<FilterParameters>(), It.IsAny<Pagination>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = PublisherControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetPublisherGames(rand.Next(100), filter.Object, pagination.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task PublisherController_CreatePublisher_ReturnBadRequest()
        {
            var expectedItem = help.CreateRandomPublisher();
            publishserRepositoryMock.Setup(r => r.GetPublisherByNameAsync(It.IsAny<string>()))!.ReturnsAsync(expectedItem);

            var controller = PublisherControllerCreate();

            var result = await controller.CreatePublisher(CreateRandomPublisherCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PublisherController_CreatePublisher_ReturnOk()
        {
            publishserRepositoryMock.Setup(r => r.GetPublisherByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Publisher)null!);

            var controller = PublisherControllerCreate();

            var result = await controller.CreatePublisher(CreateRandomPublisherCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PublisherController_UpdatePublisherInfo_ReturnNotFound()
        {
            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PublisherControllerCreate();

            var result = await controller.UpdatePublisherInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PublisherController_UpdatePublisherInfo_ReturnBadRequest()
        {
            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            publishserRepositoryMock.Setup(r => r.PublisherNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = PublisherControllerCreate();

            var result = await controller.UpdatePublisherInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PublisherController_UpdatePublisherInfo_ReturnOk()
        {
            var expectedItem = help.CreateRandomPublisher();
            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            publishserRepositoryMock.Setup(r => r.PublisherNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            publishserRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = PublisherControllerCreate();

            var result = await controller.UpdatePublisherInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PublisherController_DeletePublisher_ReturnNotFound()
        {
            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PublisherControllerCreate();

            var result = await controller.DeletePublisher(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PublisherController_DeletePublisher_ReturnOk()
        {
            var expectedItem = help.CreateRandomPublisher();

            publishserRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            publishserRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = PublisherControllerCreate();

            var result = await controller.DeletePublisher(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private PublisherController PublisherControllerCreate()
        {
            return new PublisherController(publishserRepositoryMock.Object,
                gameRepositoryMock.Object, mapperMock.Object);
        }

        private PublisherCreateDto CreateRandomPublisherCreateDto()
        {
            return new PublisherCreateDto
            {
                Name = help.RandomString(20),
                PicturePath = help.RandomString(20),
                MiniPicturePath = help.RandomString(20),
            };
        }
    }
}
