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
using Microsoft.Extensions.Logging;
using GameLibraryV2.Helper;
using GameLibraryV2.Dto.Update;

namespace GameLibraryTest.Controller
{
    public class GameControllerTest
    {
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IDeveloperRepository> developerRepositoryMock = new();
        private readonly Mock<IPublisherRepository> publisherRepositoryMock = new();
        private readonly Mock<IGenreRepository> genreRepositoryMock = new();
        private readonly Mock<ITagRepository> tagRepositoryMock = new();
        private readonly Mock<IPlatformRepository> platformRepositoryMock = new();
        private readonly Mock<IPersonGamesRepository> personGameRepositoryMock = new();
        private readonly Mock<IReviewRepository> reviewRepositoryMock = new();
        private readonly Mock<IRatingRepository> ratingRepositoryMock = new();
        private readonly Mock<IDLCRepository> dlcRepositoryMock = new();
        private readonly Mock<IAgeRatingRepository> ageRatingRepositoryMock = new();
        private readonly Mock<ISystemRequirements> systemRequirementsRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        private readonly Mock<ILogger<Game>> loggerMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task GameController_GetGames_ReturnGameList()
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


            gameRepositoryMock.Setup(r => r.GetGamesAsync(It.IsAny<FilterParameters>(), 
                It.IsAny<Pagination>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = GameControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetGames(filter.Object, pagination.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task GameController_GetGameById_ReturnNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.GetGameById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_GetGameById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomGame();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<GameDto>(expectedItem);

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<GameDto>(expectedItem)).Returns(mappedExpected);

            var controller = GameControllerCreate();

            var result = await controller.GetGameById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as GameDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task GameController_GetGameReview_ReturnNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.GetGameReview(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_GetGameReview_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Review>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomReview());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<ReviewDto>>(expectedItems);

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            mapperMock.Setup(r => r.Map<List<ReviewDto>>(expectedItems)).Returns(mappedExpected);
            reviewRepositoryMock.Setup(r => r.GetGameReviewsAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItems);

            var controller = GameControllerCreate();

            var result = await controller.GetGameReview(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as List<ReviewDto>);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnBadRequest_GameNameAlreadyExists()
        {
            var expectedItem = help.CreateRandomGame();
            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>())).ReturnsAsync(expectedItem);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnNotFound_NotFoundAgeRating()
        {
            var expectedItem = help.CreateRandomGame();
            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnNotFound_NotFoundDeveloper()
        {
            var expectedItem = help.CreateRandomGame();

            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Developer)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnNotFound_NotFoundPublisher()
        {
            var expectedItem = help.CreateRandomGame();
            var developerItem = help.CreateRandomDeveloper();

            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Publisher)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnNotFound_NotFoundPlatform()
        {
            var expectedItem = help.CreateRandomGame();
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();

            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Platform)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnNotFound_NotFoundGenre()
        {
            var expectedItem = help.CreateRandomGame();
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();
            var platformItem = help.CreateRandomPlatform();

            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(platformItem);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Genre)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnNotFound_NotFoundTag()
        {
            var expectedItem = help.CreateRandomGame();
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();
            var platformItem = help.CreateRandomPlatform();
            var genreItem = help.CreateRandomGenre();

            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(platformItem);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync(genreItem);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Tag)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_CreateGame_ReturnOk()
        {
            var expectedItem = CreateRandomGameCreateDto();
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();
            var platformItem = help.CreateRandomPlatform();
            var genreItem = help.CreateRandomGenre();
            var tagItem = help.CreateRandomTag();
            var ageRatingItem = help.CreateRandomAgeRating();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<Game>(expectedItem);

            gameRepositoryMock.Setup(r => r.GetGameByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Game)null!);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(platformItem);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync(genreItem);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))!.ReturnsAsync(tagItem);
            ageRatingRepositoryMock.Setup(r => r.GetAgeRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(ageRatingItem);
            mapperMock.Setup(r => r.Map<Game>(expectedItem)).Returns(mappedExpected);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(expectedItem);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(CreateRandomGameUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_UpdateGame_ReturnBadRequest_GameNameAlreadyUse()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(CreateRandomGameUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound_NotFoundAgeRating()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(CreateRandomGameUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound_NotFoundDeveloper()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Developer)null!);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(CreateRandomGameUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound_NotFoundPublisher()
        {
            var developerItem = help.CreateRandomDeveloper();

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Publisher)null!);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(CreateRandomGameUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound_NotFoundPlatform()
        {
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Platform)null!);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(CreateRandomGameUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound_NotFoundGenre()
        {
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();
            var platformItem = help.CreateRandomPlatform();

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(platformItem);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Genre)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnNotFound_NotFoundTag()
        {
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();
            var platformItem = help.CreateRandomPlatform();
            var genreItem = help.CreateRandomGenre();

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(platformItem);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync(genreItem);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))!.ReturnsAsync((Tag)null!);

            var controller = GameControllerCreate();

            var result = await controller.CreateGame(CreateRandomGameCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GameController_UpdateGame_ReturnOk()
        {
            var expectedItem = CreateRandomGameUpdate();
            var expGame = help.CreateRandomGame();
            var developerItem = help.CreateRandomDeveloper();
            var publisherItem = help.CreateRandomPublisher();
            var platformItem = help.CreateRandomPlatform();
            var genreItem = help.CreateRandomGenre();
            var tagItem = help.CreateRandomTag();
            var ageRatingItem = help.CreateRandomAgeRating();

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GameNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryMock.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            developerRepositoryMock.Setup(r => r.GetDeveloperByIdAsync(It.IsAny<int>()))!.ReturnsAsync(developerItem);
            publisherRepositoryMock.Setup(r => r.GetPublisherByIdAsync(It.IsAny<int>()))!.ReturnsAsync(publisherItem);
            platformRepositoryMock.Setup(r => r.GetPlatformByIdAsync(It.IsAny<int>()))!.ReturnsAsync(platformItem);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync(genreItem);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))!.ReturnsAsync(tagItem);
            ageRatingRepositoryMock.Setup(r => r.GetAgeRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(ageRatingItem);
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(expGame);

            var controller = GameControllerCreate();

            var result = await controller.UpdateGameInfo(expectedItem);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GameController_DeleteGame_ReturnNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.DeleteGame(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_DeleteGame_ReturnOk()
        {
            var expectedItem = help.CreateRandomGame();

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = GameControllerCreate();

            var result = await controller.DeleteGame(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GameController_GetUserFavouriteGames_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GameControllerCreate();

            var result = await controller.GetUserFavouriteGames(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GameController_GetUserFavouriteGames_ReturnOk()
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

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedItems)).Returns(mappedExpected);

            var controller = GameControllerCreate();

            var result = await controller.GetUserFavouriteGames(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private GameController GameControllerCreate()
        {
            return new GameController(gameRepositoryMock.Object,
                developerRepositoryMock.Object, publisherRepositoryMock.Object,
                genreRepositoryMock.Object, tagRepositoryMock.Object,
                platformRepositoryMock.Object, personGameRepositoryMock.Object,
                reviewRepositoryMock.Object, ratingRepositoryMock.Object,
                dlcRepositoryMock.Object, ageRatingRepositoryMock.Object,
                systemRequirementsRepositoryMock.Object, userRepositoryMock.Object,
                loggerMock.Object, mapperMock.Object);
        }

        private GameCreateDto CreateRandomGameCreateDto()
        {
            return new GameCreateDto
            {
                Name = help.RandomString(50),
                Status = rand.Next(0, 1) == 0 ? "released" : "announsed",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
                Description = help.RandomString(50),
                AgeRating = rand.Next(100),
                NSFW = false,
                Type = rand.Next(0, 1) == 0 ? "game" : "dlc",
                SystemRequirements = new List<SystemRequirementsCreateDto>() { help.CreateSystemRequirementsCreateDto() },
                Developers = new List<int>() { rand.Next(100) },
                Publishers = new List<int>() { rand.Next(100) },
                Platforms = new List<int> { rand.Next(100) },
                Genres = new List<int> { rand.Next(100) },
                Tags = new List<int> { rand.Next(100) },
            };
        }

        private GameUpdate CreateRandomGameUpdate()
        {
            return new GameUpdate
            {
                Id = rand.Next(100),
                Name = help.RandomString(50),
                Status = rand.Next(0, 1) == 0 ? "released" : "announsed",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
                Description = help.RandomString(50),
                AgeRating = rand.Next(100),
                NSFW = false,
                Developers = new List<int>() { rand.Next(100) },
                Publishers = new List<int>() { rand.Next(100) },
                Platforms = new List<int> { rand.Next(100) },
                Genres = new List<int> { rand.Next(100) },
                Tags = new List<int> { rand.Next(100) },
            };
        }
    }
}
