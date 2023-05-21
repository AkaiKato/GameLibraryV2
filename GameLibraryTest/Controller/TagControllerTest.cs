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
    public class TagControllerTest
    {
        private readonly Mock<ITagRepository> tagRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task TagController_GetTags_ReturnExpectedItems()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Tag>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomTag());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<TagDto>>(expectedItems);

            tagRepositoryMock.Setup(r => r.GetTagsAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<TagDto>>(expectedItems)).Returns(mappedExpected);

            var controller = TagControllerCreate();

            var result = await controller.GetTags();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<TagDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task TagController_GetTagById_ReturnNotFound()
        {
            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = TagControllerCreate();

            var result = await controller.GetTagById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task TagController_GetTagById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomTag();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<TagDto>(expectedItem);

            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<TagDto>(expectedItem)).Returns(mappedExpected);

            var controller = TagControllerCreate();

            var result = await controller.GetTagById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as TagDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task TagController_GetTagGames_ReturnNotFound()
        {
            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = TagControllerCreate();

            var result = await controller.GetTagGames(rand.Next(100), filter.Object, pagination.Object);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task TagController_GetTagGames_ReturnGameList()
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

            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            gameRepositoryMock.Setup(r => r.GetGamesByTagAsync(It.IsAny<int>(),
                It.IsAny<FilterParameters>(), It.IsAny<Pagination>())).ReturnsAsync(expectedPagedList);
            mapperMock.Setup(r => r.Map<List<GameSmallListDto>>(expectedPagedList)).Returns(expectedMapped);

            var controller = TagControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetTagGames(rand.Next(100), filter.Object, pagination.Object);

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<GameSmallListDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task TagController_CreateTag_ReturnBadRequest()
        {
            var expectedItem = help.CreateRandomTag();
            tagRepositoryMock.Setup(r => r.GetTagByNameAsync(It.IsAny<string>()))!.ReturnsAsync(expectedItem);

            var controller = TagControllerCreate();

            var result = await controller.CreateTag(CreateRandomTagCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task TagController_CreateTag_ReturnOk()
        {
            tagRepositoryMock.Setup(r => r.GetTagByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Tag)null!);

            var controller = TagControllerCreate();

            var result = await controller.CreateTag(CreateRandomTagCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task TagController_UpdateTagInfo_ReturnNotFound()
        {
            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = TagControllerCreate();

            var result = await controller.UpdateTagInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task TagController_UpdateTagInfo_ReturnBadRequest()
        {
            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            tagRepositoryMock.Setup(r => r.TagNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = TagControllerCreate();

            var result = await controller.UpdateTagInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GenreController_UpdateGenreInfo_ReturnOk()
        {
            var expectedItem = help.CreateRandomTag();
            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            tagRepositoryMock.Setup(r => r.TagNameAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = TagControllerCreate();

            var result = await controller.UpdateTagInfo(help.CreateRandomCommonUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GenreController_DeleteGenre_ReturnNotFound()
        {
            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = TagControllerCreate();

            var result = await controller.DeleteTag(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GenreController_DeleteGenre_ReturnOk()
        {
            var expectedItem = help.CreateRandomTag();

            tagRepositoryMock.Setup(r => r.TagExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            tagRepositoryMock.Setup(r => r.GetTagByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = TagControllerCreate();

            var result = await controller.DeleteTag(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private TagController TagControllerCreate()
        {
            return new TagController(tagRepositoryMock.Object,
                gameRepositoryMock.Object, mapperMock.Object);
        }

        private TagCreateDto CreateRandomTagCreateDto()
        {
            return new TagCreateDto
            {
                Name = help.RandomString(20),
            };
        }
    }
}
