using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class GenreControllerTest
    {
        private readonly Mock<IGenreRepository> genreRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task GenreController_GetGenres_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Genre>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomGenre());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<GenreDto>>(expectedItems);

            genreRepositoryMock.Setup(r => r.GetGenresAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<GenreDto>>(expectedItems)).Returns(mappedExpected);

            var controller = GenreControllerCreate();

            var result = await controller.GetGenres();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<GenreDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }
        
        [Fact]
        public async Task GenreController_GetGenreById_ReturnNotFound()
        {
            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = GenreControllerCreate();

            var result = await controller.GetGenreById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_GetGenreById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomGenre();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<GenreDto>(expectedItem);

            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<GenreDto>(expectedItem)).Returns(mappedExpected);

            var controller = GenreControllerCreate();

            var result = await controller.GetGenreById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as GenreDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task GenreController_GetGenreGames_ReturnNotFound()
        {
            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = GenreControllerCreate();

            var result = await controller.GetGenreGames(rand.Next(100), filter.Object, pagination.Object);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GenreController_GetGenreGames_ReturnGameList()
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

            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGamesByGenreAsync(It.IsAny<int>(),
                It.IsAny<FilterParameters>(), It.IsAny<Pagination>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = GenreControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetGenreGames(rand.Next(100), filter.Object, pagination.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }
        
        [Fact]
        public async Task GenreController_CreateGenre_ReturnBadRequest()
        {
            var expectedItem = help.CreateRandomGenre();
            genreRepositoryMock.Setup(r => r.GetGenreByNameAsync(It.IsAny<string>()))!.ReturnsAsync(expectedItem);

            var controller = GenreControllerCreate();

            var result = await controller.CreateGenre(CreateRandomGenreCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_CreateGenre_ReturnOk()
        {
            genreRepositoryMock.Setup(r => r.GetGenreByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Genre)null!);

            var controller = GenreControllerCreate();

            var result = await controller.CreateGenre(CreateRandomGenreCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_UpdateGenreInfo_ReturnNotFound()
        {
            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GenreControllerCreate();

            var result = await controller.UpdateGenreInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_UpdateGenreInfo_ReturnBadRequest()
        {
            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            genreRepositoryMock.Setup(r => r.GenreNameAlredyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = GenreControllerCreate();

            var result = await controller.UpdateGenreInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_UpdateGenreInfo_ReturnOk()
        {
            var expectedItem = help.CreateRandomGenre();
            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            genreRepositoryMock.Setup(r => r.GenreNameAlredyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = GenreControllerCreate();

            var result = await controller.UpdateGenreInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_DeleteGenre_ReturnNotFound()
        {
            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = GenreControllerCreate();

            var result = await controller.DeleteGenre(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task GenreController_DeleteGenre_ReturnOk()
        {
            var expectedItem = help.CreateRandomGenre();

            genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = GenreControllerCreate();

            var result = await controller.DeleteGenre(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private GenreController GenreControllerCreate()
        {
            return new GenreController(genreRepositoryMock.Object,
                gameRepositoryMock.Object, mapperMock.Object);
        }

        private GenreCreateDto CreateRandomGenreCreateDto()
        {
            return new GenreCreateDto
            {
                Name = help.RandomString(20),
            };
        }
    }
}
