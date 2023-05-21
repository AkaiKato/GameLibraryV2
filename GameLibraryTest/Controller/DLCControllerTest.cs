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
using GameLibraryV2.Dto.Update;

namespace GameLibraryTest.Controller
{
    public class DLCControllerTest
    {
        private readonly Mock<IDLCRepository> dlcRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task DLCController_GetDLCs_ReturnOk()
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
            var mappedExpected = mapper.Map<List<GameSmallListDto>>(expectedItems);

            gameRepositoryMock.Setup(r => r.GetDLCsAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedItems)).Returns(mappedExpected);

            var controller = DLCControllerCreate();

            var result = await controller.GetDLCs();

            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<GameSmallListDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }
        
        [Fact]
        public async Task DLCController_GetDLCById_ReturnNotFound()
        {
            gameRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = DLCControllerCreate();

            var result = await controller.GetDLCById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task DLCController_GetDLCById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomGame();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<GameDto>(expectedItem);

            gameRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetDLCByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<GameDto>(expectedItem)).Returns(mappedExpected);

            var controller = DLCControllerCreate();

            var result = await controller.GetDLCById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as GameDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task DLCController_CreateGameDlc_ReturnNotFound_NotFoundGameWithId()
        {
            var dlc = CreateRandomDlcUpdate();
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = DLCControllerCreate();

            var result = await controller.CreateGameDlc(dlc);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo($"Not found game with such id {dlc.ParentGameId}");
        }

        [Fact]
        public async Task DLCController_CreateGameDlc_ReturnNotFound_NotFoundDlcWithId()
        {
            var dlc = CreateRandomDlcUpdate();
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = DLCControllerCreate();

            var result = await controller.CreateGameDlc(dlc);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo($"Not found dlc with such id {dlc.ParentGameId}");
        }

        [Fact]
        public async Task DLCController_CreateGameDlc_ReturnBadRequest()
        {
            var dlc = CreateRandomDlcUpdate();
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            dlcRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var controller = DLCControllerCreate();

            var result = await controller.CreateGameDlc(dlc);

            result.Should().BeOfType<BadRequestObjectResult>();
            var okResult = result as BadRequestObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo("DLC already have parent Game");
        }

        [Fact]
        public async Task DLCController_CreateGameDlc_ReturnOk()
        {
            var dlc = CreateRandomDlcUpdate();
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            dlcRepositoryMock.Setup(r => r.DLCExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            gameRepositoryMock.Setup(r => r.GetDLCByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomGame);
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomGame);
            
            var controller = DLCControllerCreate();

            var result = await controller.CreateGameDlc(dlc);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DLCController_DeleteGameDlc_ReturnNotFound()
        {
            dlcRepositoryMock.Setup(r => r.DLCExistsByConnIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = DLCControllerCreate();

            var result = await controller.DeleteGameDlc(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task DLCController_DeleteGameDlc_ReturnOk()
        {
            var expectedItem = help.CreateRandomDlc();

            dlcRepositoryMock.Setup(r => r.DLCExistsByConnIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            dlcRepositoryMock.Setup(r => r.GetDLCConnByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = DLCControllerCreate();

            var result = await controller.DeleteGameDlc(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private DLCController DLCControllerCreate()
        {
            return new DLCController(dlcRepositoryMock.Object,
                gameRepositoryMock.Object, mapperMock.Object);
        }

        private DlcUpdate CreateRandomDlcUpdate()
        {
            return new DlcUpdate
            {
                ParentGameId = rand.Next(100),
                DLCGameId = rand.Next(100),
            };
        }
    }
}
