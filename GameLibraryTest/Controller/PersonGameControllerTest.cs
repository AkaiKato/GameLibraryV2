using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GameLibraryV2.Repositories;
using GameLibraryV2.Dto.Update;

namespace GameLibraryTest.Controller
{
    public class PersonGameControllerTest
    {
        private readonly Mock<IPersonGamesRepository> personGamesRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPlatformRepository> platformRepositoryMock = new();
        private readonly Mock<IReviewRepository> reviewRepositoryMock = new();
        private readonly Mock<IRatingRepository> ratingRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task PersonGameController_GetPersonGames_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.GetPersonGames(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_GetPersonGames_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<PersonGame>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomPersonGame());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<PersonGameDto>>(expectedItems);

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGamesRepositoryMock.Setup(r => r.PersonGamesAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<PersonGameDto>>(expectedItems)).Returns(mappedExpected);

            var controller = PersonGameControllerCreate();

            var result = await controller.GetPersonGames(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<PersonGameDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task PersonGameController_GetPersonGamesByList_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.GetPersonGamesByList(It.IsAny<int>(), It.IsAny<string>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_GetPersonGamesByList_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<PersonGame>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomPersonGame());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<PersonGameDto>>(expectedItems);

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGamesRepositoryMock.Setup(r => r.PersonGamesByListAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<PersonGameDto>>(expectedItems)).Returns(mappedExpected);

            var controller = PersonGameControllerCreate();

            var result = await controller.GetPersonGamesByList(It.IsAny<int>(), It.IsAny<string>());

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<PersonGameDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task PersonGameController_AddPersonGame_ReturnNotFound_UserNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.AddPersonGame(CreateRandomPersonGameCreate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_AddPersonGame_ReturnBadRequest()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserRefreshTokenValid(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.AddPersonGame(CreateRandomPersonGameCreate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_AddPersonGame_ReturnNotFound_GameNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserRefreshTokenValid(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.AddPersonGame(CreateRandomPersonGameCreate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_AddPersonGame_ReturnOk()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserRefreshTokenValid(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomUser());
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomGame());

            var controller = PersonGameControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.AddPersonGame(CreateRandomPersonGameCreate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_UpdatePersonGame_ReturnNotFound_PersonGameNotFound()
        {
            personGamesRepositoryMock.Setup(r => r.PersonGameExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.UpdatePersonGame(CreateRandomPersonGameUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_UpdatePersonGame_ReturnOk()
        {
            var personGame = help.CreateRandomPersonGame();

            personGamesRepositoryMock.Setup(r => r.PersonGameExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            personGamesRepositoryMock.Setup(r => r.GetPersonGameByIdAsync(It.IsAny<Guid>())).ReturnsAsync(personGame);
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.UpdatePersonGame(CreateRandomPersonGameUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_DeletePersonGame_ReturnNotFound_PersonGameNotFound()
        {
            personGamesRepositoryMock.Setup(r => r.PersonGameExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.DeletePersonGame(It.IsAny<Guid>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PersonGameController_DeletePersonGame_ReturnOk()
        {
            var personGame = help.CreateRandomPersonGame();

            personGamesRepositoryMock.Setup(r => r.PersonGameExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            personGamesRepositoryMock.Setup(r => r.GetPersonGameByIdAsync(It.IsAny<Guid>())).ReturnsAsync(personGame);
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PersonGameControllerCreate();

            var result = await controller.DeletePersonGame(It.IsAny<Guid>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private PersonGameController PersonGameControllerCreate()
        {
            return new PersonGameController(personGamesRepositoryMock.Object,
                gameRepositoryMock.Object, userRepositoryMock.Object,
                platformRepositoryMock.Object, reviewRepositoryMock.Object, 
                ratingRepositoryMock.Object, mapperMock.Object);
        }

        private PersonGameCreate CreateRandomPersonGameCreate()
        {
            return new PersonGameCreate
            {
                UserId = rand.Next(100),
                GameId = rand.Next(100),
                List = rand.Next(0, 1) == 0 ? "planned" : "completed",
            };
        }

        private PersonGameUpdate CreateRandomPersonGameUpdate()
        {
            return new PersonGameUpdate
            {
                Id = Guid.NewGuid(),
                Score = rand.Next(1, 10),
                List = rand.Next(0, 1) == 0 ? "planned" : "completed",
                Favourite = rand.Next(0, 1) != 0,
            };
        }
    }
}
