using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Statistic;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class StatisticsControllerTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPersonGamesRepository> personGameRepositoryMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task StatisticsController_GetUserPublisherStatistics_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserPublisherStatistics(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task StatisticsController_GetUserPublisherStatistics_ReturnExpectedItems()
        {
            var expectedItems = new List<CountStatistic> { help.CreateRandomCountStatistic() };
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGameRepositoryMock.Setup(r => r.GetPersonPublisherStatisticAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserPublisherStatistics(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<CountStatistic>);
            verify.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task StatisticsController_GetUserTagStatistics_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserTagStatistics(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task StatisticsController_GetUserTagStatistics_ReturnExpectedItems()
        {
            var expectedItems = new List<CountStatistic> { help.CreateRandomCountStatistic() };
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGameRepositoryMock.Setup(r => r.GetPersonTagStatisticAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserTagStatistics(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<CountStatistic>);
            verify.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task StatisticsController_GetUserDeveloperStatistics_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserDeveloperStatistics(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task StatisticsController_GetUserDeveloperStatistics_ReturnExpectedItems()
        {
            var expectedItems = new List<CountStatistic> { help.CreateRandomCountStatistic() };
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGameRepositoryMock.Setup(r => r.GetPersonDeveloperStatisticAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserDeveloperStatistics(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<CountStatistic>);
            verify.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task StatisticsController_GetUserPlatformStatistics_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserPlatformStatistics(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task StatisticsController_GetUserPlatformStatistics_ReturnExpectedItems()
        {
            var expectedItems = new List<CountStatistic> { help.CreateRandomCountStatistic() };
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGameRepositoryMock.Setup(r => r.GetPersonPlatformStatisticAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserPlatformStatistics(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<CountStatistic>);
            verify.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task StatisticsController_GetUserGenreStatistics_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserGenreStatistics(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task StatisticsController_GetUserGenreStatistics_ReturnExpectedItems()
        {
            var expectedItems = new List<CountStatistic> { help.CreateRandomCountStatistic() };
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            personGameRepositoryMock.Setup(r => r.GetPersonGenreStatisticAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);

            var controller = StatisticsControllerCreate();

            var result = await controller.GetUserGenreStatistics(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<CountStatistic>);
            verify.Should().BeEquivalentTo(expectedItems);
        }

        private StatisticsController StatisticsControllerCreate()
        {
            return new StatisticsController(userRepositoryMock.Object,
                personGameRepositoryMock.Object);
        }
    }
}
