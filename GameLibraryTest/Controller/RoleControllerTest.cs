using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class RoleControllerTest
    {
        private readonly Mock<IRoleRepository> roleRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task RoleController_GetRoles_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Role>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomRole());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<RoleDto>>(expectedItems);

            roleRepositoryMock.Setup(r => r.GetRolesAsync()).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<RoleDto>>(expectedItems)).Returns(mappedExpected);

            var controller = RoleControllerCreate();

            var result = await controller.GetRoles();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<RoleDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task RoleController_GetRoleById_ReturnNotFound()
        {
            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = RoleControllerCreate();

            var result = await controller.GetRoleById(rand.Next(100));

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RoleController_GetRoleById_ReturnExpectedItem()
        {
            var expectedItem = help.CreateRandomRole();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<RoleDto>(expectedItem);

            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            roleRepositoryMock.Setup(r => r.GetRoleByIdAsync(It.IsAny<int>()))!.ReturnsAsync(expectedItem);
            mapperMock.Setup(r => r.Map<RoleDto>(expectedItem)).Returns(mappedExpected);

            var controller = RoleControllerCreate();

            var result = await controller.GetRoleById(rand.Next(100));

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var verify = (okResult!.Value as RoleDto);
            verify.Should().BeEquivalentTo(mappedExpected);
        }

        [Fact]
        public async Task RoleController_GetRoleUsers_ReturnNotFound()
        {
            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>())).ReturnsAsync(false);
            var filter = new Mock<FilterParameters>();
            var pagination = new Mock<Pagination>();

            var controller = RoleControllerCreate();

            var result = await controller.GetRoleUsers(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RoleController_GetRoleUsers_ReturnExpectedList()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<User>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomUser());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var expectedMapped = mapper.Map<List<UserDto>>(expectedItems);

            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUsersByRoleAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<UserDto>>(expectedItems)).Returns(expectedMapped);

            var controller = RoleControllerCreate();

            var result = await controller.GetRoleUsers(It.IsAny<int>());

            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<UserDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task RoleController_AddRole_ReturnNotFound_UserNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = RoleControllerCreate();

            var result = await controller.AddRole(CreateRandomRoleUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RoleController_AddRole_ReturnNotFound_RoleNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = RoleControllerCreate();

            var result = await controller.AddRole(CreateRandomRoleUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RoleController_AddRole_ReturnOk()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomUser());
            roleRepositoryMock.Setup(r => r.GetRoleByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomRole());

            var controller = RoleControllerCreate();

            var result = await controller.AddRole(CreateRandomRoleUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RoleController_CreateRole_ReturnBadRequest()
        {
            roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(It.IsAny<string>()))!.ReturnsAsync(help.CreateRandomRole());

            var controller = RoleControllerCreate();

            var result = await controller.CreateRole(CreateRandomRoleCreateDto());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RoleController_CreateRole_ReturnOk()
        {
            roleRepositoryMock.Setup(r => r.GetRoleByNameAsync(It.IsAny<string>()))!.ReturnsAsync((Role)null!);

            var controller = RoleControllerCreate();

            var result = await controller.CreateRole(CreateRandomRoleCreateDto());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RoleController_DeleteRole_ReturnNotFound_UserNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = RoleControllerCreate();

            var result = await controller.DeleteRole(CreateRandomRoleUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RoleController_DeleteRole_ReturnNotFound_RoleNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = RoleControllerCreate();

            var result = await controller.DeleteRole(CreateRandomRoleUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Fact]
        public async Task RoleController_DeleteRole_ReturnOk()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            roleRepositoryMock.Setup(r => r.RoleExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomUser());
            roleRepositoryMock.Setup(r => r.GetUserRoleAsync(It.IsAny<int>()))
                .ReturnsAsync(
                new List<Role>() 
                { 
                    help.CreateRandomRole(), 
                    help.CreateRandomRole(), 
                });

            var controller = RoleControllerCreate();

            var result = await controller.DeleteRole(CreateRandomRoleUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        private RoleController RoleControllerCreate()
        {
            return new RoleController(roleRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);
        }

        private RoleUpdate CreateRandomRoleUpdate()
        {
            return new RoleUpdate
            {
                UserId = rand.Next(100),
                RoleId = rand.Next(100),
            };
        }

        private RoleCreateDto CreateRandomRoleCreateDto()
        {
            return new RoleCreateDto
            {
                RoleName = help.RandomString(20),
            };
        }
    }
}
