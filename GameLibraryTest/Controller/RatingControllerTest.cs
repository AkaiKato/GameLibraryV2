using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class RatingControllerTest
    {
        private readonly Mock<IRatingRepository> ratingRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task RatingController_GetAllRatings_ReturnExpectedItem()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Rating>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomRating());
            }

            ratingRepositoryMock.Setup(r => r.GetRatingsAsync())!.ReturnsAsync(expectedItems);

            var controller = RatingControllerCreate();

            var result = await controller.GetAllRatings();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<Rating>);
            verifyRes.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task RatingController_GetGameRating_ReturnNotFound_GameNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = RatingControllerCreate();

            var result = await controller.GetGameRating(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RatingController_GetGameRating_ReturnNotFound_RatingNotFound()
        {
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomGame());
            ratingRepositoryMock.Setup(r => r.RatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = RatingControllerCreate();

            var result = await controller.GetGameRating(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RatingController_GetGameRating_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomRating();
            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGameByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomGame());
            ratingRepositoryMock.Setup(r => r.RatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            ratingRepositoryMock.Setup(r => r.GetRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = RatingControllerCreate();

            var result = await controller.GetGameRating(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as Rating);
            verifyRes.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task RatingController_UpdateGameRating_ReturnNotFound()
        {
            ratingRepositoryMock.Setup(r => r.RatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = RatingControllerCreate();

            var result = await controller.UpdateGameRating(CreateRandomRatingUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RatingController_UpdateGameRating_ReturnOk()
        {
            ratingRepositoryMock.Setup(r => r.RatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            ratingRepositoryMock.Setup(r => r.GetRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomRating());
            var controller = RatingControllerCreate();

            var result = await controller.UpdateGameRating(CreateRandomRatingUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RatingController_DeleteGameRating_ReturnNotFound()
        {
            ratingRepositoryMock.Setup(r => r.RatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = RatingControllerCreate();

            var result = await controller.DeleteGameRating(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RatingController_DeleteGameRating_ReturnOk()
        {
            ratingRepositoryMock.Setup(r => r.RatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            ratingRepositoryMock.Setup(r => r.GetRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomRating());

            var controller = RatingControllerCreate();

            var result = await controller.DeleteGameRating(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RatingController_TotalRatingCalculation_ReturnOk()
        {
            ratingRepositoryMock.Setup(r => r.GetRatingsAsync())!.ReturnsAsync(new List<Rating>() { help.CreateRandomRating() });
           
            var controller = RatingControllerCreate();

            var result = await controller.TotalRatingCalculation();

            result.Should().BeOfType<OkObjectResult>();
        }

        private RatingController RatingControllerCreate()
        {
            return new RatingController(ratingRepositoryMock.Object,
                gameRepositoryMock.Object);
        }

        private RatingUpdate CreateRandomRatingUpdate()
        {
            return new RatingUpdate
            {
                RatingId = rand.Next(100),
                Rating = rand.Next(1, 10),
            };
        }
    }
}
