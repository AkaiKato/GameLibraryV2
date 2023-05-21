using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
namespace GameLibraryTest.Controller
{
    public class AuthControllerTest
    {
        private readonly Mock<IUserRepository> userRepositoryStub = new();
        private readonly Mock<IRoleRepository> roleRepositoryStub = new();
        private readonly Mock<IConfiguration> configurationStub = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task AuthController_Register_ReturnBadRequest_EmailAlreadyRegistred()
        {
            userRepositoryStub.Setup(r => r.HasEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            var controller = AuthControllerCreate();

            var result = await controller.Register(CreateRandomUserCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
            var okResult = result as BadRequestObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo("User with Email already registrated");
        }

        [Fact]
        public async Task AuthController_Register_ReturnBadRequest_NicknameAlreadyExists()
        {
            userRepositoryStub.Setup(r => r.HasEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            userRepositoryStub.Setup(r => r.HasNicknameAsync(It.IsAny<string>())).ReturnsAsync(true);

            var controller = AuthControllerCreate();

            var result = await controller.Register(CreateRandomUserCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
            var okResult = result as BadRequestObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo("User with this Nickname already exists");
        }

        [Fact]
        public async Task AuthController_Register_ReturnOk()
        {
            userRepositoryStub.Setup(r => r.HasEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            userRepositoryStub.Setup(r => r.HasNicknameAsync(It.IsAny<string>())).ReturnsAsync(false);

            var controller = AuthControllerCreate();

            var result = await controller.Register(CreateRandomUserCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthController_Login_ReturnNotFound()
        {
            var expectedItem = CreateRandomUserLogin();

            userRepositoryStub.Setup(r => r.UserExistsByNicknameAsync(It.IsAny<string>())).ReturnsAsync(false);

            var controller = AuthControllerCreate();

            var result = await controller.Login(expectedItem);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo($"Not found user with such nickname {expectedItem.Nickname}");
        }

        [Fact]
        public async Task AuthController_Login_ReturnOk()
        {
            var expectedItem = help.CreateRandomUser();
            var randomUserLogin = new UserLogin()
            {
                Nickname = expectedItem.Nickname,
                Password = expectedItem.Password
            };
            expectedItem.Password = BCrypt.Net.BCrypt.HashPassword(expectedItem.Password);
            configurationStub.Setup(r => r.GetSection(It.IsAny<string>()).Value).Returns("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            userRepositoryStub.Setup(r => r.UserExistsByNicknameAsync(It.IsAny<string>())).ReturnsAsync(true);
            userRepositoryStub.Setup(r => r.GetUserByNicknameAsync(It.IsAny<string>())).ReturnsAsync(expectedItem);
    
            var controller = AuthControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Login(randomUserLogin);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthController_RefreshToken_ReturnNotFound()
        {
            var expectedItem = help.CreateRandomUser();

            userRepositoryStub.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = AuthControllerCreate();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.RefreshToken(expectedItem.Id);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo($"Not found user with such id {expectedItem.Id}");
        }

        private AuthController AuthControllerCreate()
        {
            return new AuthController(userRepositoryStub.Object,
                roleRepositoryStub.Object, configurationStub.Object);
        }

        private UserCreateDto CreateRandomUserCreateDto()
        {
            return new UserCreateDto
            {
                Email = help.RandomString(10) + "@mail.com",
                Nickname = help.RandomString(5),
                Password = help.RandomString(10),
                Age = rand.Next(15, 100),
                Gender = rand.Next(0, 1) == 0 ? "male" : "female"
            };
        }

        private UserLogin CreateRandomUserLogin()
        {
            return new UserLogin
            {
                Nickname = help.RandomString(5),
                Password = help.RandomString(10),
            };
        }
    }
}
