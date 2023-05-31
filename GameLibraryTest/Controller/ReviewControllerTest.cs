using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class ReviewControllerTest
    {
        private readonly Mock<IReviewRepository> reviewRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPersonGamesRepository> personRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task ReviewController_CreateGameReview_ReturnNotFound_GameNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = ReviewControllerCreate();

            var result = await controller.CreateGameReview(CreateRandomReviewCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ReviewController_CreateGameReview_ReturnNotFound_UserNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = ReviewControllerCreate();

            var result = await controller.CreateGameReview(CreateRandomReviewCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ReviewController_CreateGameReview_ReturnNotFound_PersonGameNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personRepositoryMock.Setup(r => r.PersonGameExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var controller = ReviewControllerCreate();

            var result = await controller.CreateGameReview(CreateRandomReviewCreateDto());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ReviewController_CreateGameReview_ReturnOk()
        {
            var reviewCreated = CreateRandomReviewCreateDto();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var expectedMapped = mapper.Map<Review>(reviewCreated);

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personRepositoryMock.Setup(r => r.PersonGameExistsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            personRepositoryMock.Setup(r => r.GetPersonGameByUserIdAndGameIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(help.CreateRandomPersonGame());
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomUser());
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomGame());
            mapperMock.Setup(r => r.Map<Review>(reviewCreated)).Returns(expectedMapped);

            var controller = ReviewControllerCreate();

            var result = await controller.CreateGameReview(reviewCreated);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ReviewController_UpdateGameReview_ReturnNotFound()
        {
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);
            
            var controller = ReviewControllerCreate();

            var result = await controller.UpdateGameReview(CreateRandomReviewUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ReviewController_UpdateGameReview_ReturnOk()
        {
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomReview());

            var controller = ReviewControllerCreate();

            var result = await controller.UpdateGameReview(CreateRandomReviewUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ReviewController_UpdateRaviewRating_ReturnNotFound()
        {
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = ReviewControllerCreate();

            var result = await controller.UpdateRaviewRating(CreateRandomReviewRatingUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ReviewController_UpdateRaviewRating_ReturnOk()
        {
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomReview());

            var controller = ReviewControllerCreate();

            var result = await controller.UpdateRaviewRating(CreateRandomReviewRatingUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ReviewController_DeleteGameReview_ReturnNotFound()
        {
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = ReviewControllerCreate();

            var result = await controller.DeleteGameReview(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ReviewController_DeleteGameReview_ReturnOk()
        {
            reviewRepositoryMock.Setup(r => r.ReviewExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            reviewRepositoryMock.Setup(r => r.GetReviewByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomReview());

            var controller = ReviewControllerCreate();

            var result = await controller.DeleteGameReview(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private ReviewController ReviewControllerCreate()
        {
            return new ReviewController(reviewRepositoryMock.Object,
                gameRepositoryMock.Object, userRepositoryMock.Object,
                personRepositoryMock.Object, mapperMock.Object);
        }

        private ReviewCreateDto CreateRandomReviewCreateDto()
        {
            return new ReviewCreateDto
            {
                UserId = rand.Next(100),
                GameId = rand.Next(100),
                PublishDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };
        }

        private ReviewUpdate CreateRandomReviewUpdate()
        {
            return new ReviewUpdate
            {
                Id = rand.Next(100),
                Text = help.RandomString(20),
            };
        }

        private ReviewRatingUpdate CreateRandomReviewRatingUpdate()
        {
            return new ReviewRatingUpdate
            {
                Id = rand.Next(100),
                ReviewRating = rand.Next(-1, 1),
            };
        }
    }
}
