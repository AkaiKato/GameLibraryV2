using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class PictureControllerTest
    {
        private readonly Mock<IDeveloperRepository> developerRepositoryMock = new();
        private readonly Mock<IGameRepository> gameRepositoryMock = new();
        private readonly Mock<IPublisherRepository> publisherRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task PictureController_UploadDeveloperPicture_ReturnNotFound()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PictureControllerCreate();

            var result = await controller.UploadDeveloperPicture(rand.Next(100), file);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PictureController_UploadDeveloperMiniPicture_ReturnNotFound()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            developerRepositoryMock.Setup(r => r.DeveloperExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PictureControllerCreate();

            var result = await controller.UploadDeveloperMiniPicture(rand.Next(100), file);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PictureController_UploadGamePicture_ReturnNotFound()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            gameRepositoryMock.Setup(r => r.GameExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PictureControllerCreate();

            var result = await controller.UploadGamePicture(rand.Next(100), file);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PictureController_UploadPublisherPicture_ReturnNotFound()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            publisherRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PictureControllerCreate();

            var result = await controller.UploadPublisherPicture(rand.Next(100), file);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PictureController_UploadPublihserMiniPicture_ReturnNotFound()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            publisherRepositoryMock.Setup(r => r.PublisherExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PictureControllerCreate();

            var result = await controller.UploadPublihserMiniPicture(rand.Next(100), file);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task PictureController_UploadUserPicture_ReturnNotFound()
        {
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = PictureControllerCreate();

            var result = await controller.UploadUserPicture(rand.Next(100), file);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        private PictureController PictureControllerCreate()
        {
            return new PictureController(developerRepositoryMock.Object,
                gameRepositoryMock.Object, publisherRepositoryMock.Object,
                userRepositoryMock.Object);
        }
    }
}
