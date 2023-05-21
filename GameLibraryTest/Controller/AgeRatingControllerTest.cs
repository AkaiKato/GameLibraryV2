using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GameLibraryV2.Helper;
using Microsoft.AspNetCore.Http;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Create;

namespace GameLibraryTest.Controller
{
    public class AgeRatingControllerTest
    {
        private readonly Mock<IAgeRatingRepository> ageRatingRepositoryStub = new();
        private readonly Mock<IGameRepository> gameRepositoryStub = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new ();
        readonly Helper help = new();

        [Fact]
        public async Task AgeRatingController_GetAgeRating_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<AgeRating>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomAgeRating());
            }
            ageRatingRepositoryStub.Setup(r => r.GetAgeRatingsAsync()).ReturnsAsync(expectedItems);
            
            var controller = AgeRatingControllerCreate();

            var result = await controller.GetAgeRatings();

            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<AgeRating>);
            verifyRes.Should().BeEquivalentTo(expectedItems);
        }

        [Fact]
        public async Task AgeRatingController_GetAgeRatingById_ReturnNotFound()
        {
            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = AgeRatingControllerCreate();

            var result = await controller.GetAgeRatingById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_GetAgeRatingById_ReturnExpectedItem()
        {
            var ageRating = help.CreateRandomAgeRating();

            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            ageRatingRepositoryStub.Setup(r => r.GetAgeRatingByIdAsync(It.IsAny<int>()))!.ReturnsAsync(ageRating);

            var controller = AgeRatingControllerCreate();

            var result = await controller.GetAgeRatingById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as AgeRating);
            verify.Should().BeEquivalentTo(ageRating);
        }


        [Fact]
        public async Task AgeRatingController_GetAgeRatingGames_ReturnNotFound()
        {
            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = AgeRatingControllerCreate();

            var result = await controller.GetAgeRatingGames(rand.Next(100), filter.Object, pagination.Object);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_GetAgeRatingGames_ReturnGameList()
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

            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryStub.Setup(r => r.GetGamesByAgeRatingAsync(It.IsAny<int>(), 
                It.IsAny<FilterParameters>(), It.IsAny<Pagination>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = AgeRatingControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetAgeRatingGames(rand.Next(100), filter.Object, pagination.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task AgeRatingController_CreateAgeRating_ReturnBadRequest()
        {
            var expectedItem = help.CreateRandomAgeRating();
            ageRatingRepositoryStub.Setup(r => r.GetAgeRatingByNameAsync(It.IsAny<string>()))!.ReturnsAsync(expectedItem);

            var controller = AgeRatingControllerCreate();

            var result = await controller.CreateAgeRating(CreateRandomAgeRatingCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_CreateAgeRating_ReturnOk()
        {
            ageRatingRepositoryStub.Setup(r => r.GetAgeRatingByNameAsync(It.IsAny<string>()))!.ReturnsAsync((AgeRating)null);

            var controller = AgeRatingControllerCreate();

            var result = await controller.CreateAgeRating(CreateRandomAgeRatingCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_UpdateAgeRating_ReturnNotFound()
        {
            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = AgeRatingControllerCreate();

            var result = await controller.UpdateAgeRating(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_UpdateAgeRating_ReturnBadRequest()
        {
            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            ageRatingRepositoryStub.Setup(r => r.AgeRatingAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = AgeRatingControllerCreate();

            var result = await controller.UpdateAgeRating(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_UpdateAgeRating_ReturnOk()
        {
            var expectedItem = help.CreateRandomAgeRating();
            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            ageRatingRepositoryStub.Setup(r => r.AgeRatingAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            ageRatingRepositoryStub.Setup(r => r.GetAgeRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = AgeRatingControllerCreate();

            var result = await controller.UpdateAgeRating(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_DeleteAgeRating_ReturnNotFound()
        {
            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = AgeRatingControllerCreate();

            var result = await controller.DeleteAgeRating(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AgeRatingController_DeleteAgeRating_ReturnOk()
        {
            var expectedItem = help.CreateRandomAgeRating();

            ageRatingRepositoryStub.Setup(r => r.AgeRatingExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            ageRatingRepositoryStub.Setup(r => r.GetAgeRatingByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = AgeRatingControllerCreate();

            var result = await controller.DeleteAgeRating(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private AgeRatingController AgeRatingControllerCreate()
        {
            return new AgeRatingController(ageRatingRepositoryStub.Object, 
                gameRepositoryStub.Object, mapperMock.Object);
        }
       
        private AgeRatingCreateDto CreateRandomAgeRatingCreateDto()
        {
            return new AgeRatingCreateDto
            {
                Name = help.RandomString(20),
            };
        }
    }
}
