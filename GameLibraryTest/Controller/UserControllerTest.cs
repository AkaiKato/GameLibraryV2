using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Statistic;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class UserControllerTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IRoleRepository> roleRepositoryMock = new();
        private readonly Mock<IFriendRepository> friendRepositoryMock = new();
        private readonly Mock<IPersonGamesRepository> personGamesRepositoryMock = new();
        private readonly Mock<IReviewRepository> reviewRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();

        [Fact]
        public async Task UserController_GetUsers_ReturnExpectedItems()
        {
            var expectedItems = new List<User>() { help.CreateRandomUser() };

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var expectedMapped = mapper.Map<List<UserDto>>(expectedItems);

            userRepositoryMock.Setup(r => r.GetUsersAsync())!.ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<UserDto>>(expectedItems)).Returns(expectedMapped);

            var controller = UserControllerCreate();

            var result = await controller.GetUsers();

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = (okRes!.Value as List<UserDto>);
            verify.Should().BeEquivalentTo(expectedMapped);
        }

        [Fact]
        public async Task UserController_GetUserById_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = UserControllerCreate();

            var result = await controller.GetUserById(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UserController_GetUserById_ReturnExpectedItems()
        {
            var userc = help.CreateRandomUser();
            var FavouriteGames = new List<Game>() { help.CreateRandomGame() };
            var PublisherStatistic = new List<CountStatistic>() { help.CreateRandomCountStatistic() };
            var TagStatistic = new List<CountStatistic>() { help.CreateRandomCountStatistic() };
            var DeveloperStatistic = new List<CountStatistic>() { help.CreateRandomCountStatistic() };
            var PlatformStatistic = new List<CountStatistic>() { help.CreateRandomCountStatistic() };
            var userRev = new List<Review>() { help.CreateRandomReview() };

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var User = mapper.Map<UserDto>(userc);
            var UserReviews = mapper.Map<List<ReviewDto>>(userRev);

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(userc);
            personGamesRepositoryMock.Setup(r => r.GetPersonFavouriteGameAsync(It.IsAny<int>())).ReturnsAsync(FavouriteGames);
            personGamesRepositoryMock.Setup(r => r.GetPersonPublisherStatisticAsync(It.IsAny<int>())).ReturnsAsync(PublisherStatistic);
            personGamesRepositoryMock.Setup(r => r.GetPersonTagStatisticAsync(It.IsAny<int>())).ReturnsAsync(TagStatistic);
            personGamesRepositoryMock.Setup(r => r.GetPersonDeveloperStatisticAsync(It.IsAny<int>())).ReturnsAsync(DeveloperStatistic);
            personGamesRepositoryMock.Setup(r => r.GetPersonPlatformStatisticAsync(It.IsAny<int>())).ReturnsAsync(PlatformStatistic);
            reviewRepositoryMock.Setup(r => r.GetUserReviewsAsync(It.IsAny<int>())).ReturnsAsync(userRev);
            mapperMock.Setup(r => r.Map<UserDto>(userc)).Returns(User);
            mapperMock.Setup(r => r.Map<List<ReviewDto>>(userRev)).Returns(UserReviews);

            var userPageExp = new
            {
                User,
                FavouriteGames,
                PublisherStatistic,
                TagStatistic,
                DeveloperStatistic,
                PlatformStatistic,
                UserReviews,
            };

            var controller = UserControllerCreate();

            var result = await controller.GetUserById(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = okRes!.Value;
            verify.Should().BeEquivalentTo(userPageExp);
        }

        [Fact]
        public async Task UserController_GetUserRole_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = UserControllerCreate();

            var result = await controller.GetUserRole(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UserController_GetUserRole_ReturnExpectedItem()
        {
            var roleList = new List<Role>() { help.CreateRandomRole()};
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var mappedRoleList = mapper.Map<List<RoleDto>>(roleList);

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            roleRepositoryMock.Setup(r => r.GetUserRoleAsync(It.IsAny<int>()))!.ReturnsAsync(roleList);
            mapperMock.Setup(r => r.Map<List<RoleDto>>(roleList)).Returns(mappedRoleList);

            var controller = UserControllerCreate();

            var result = await controller.GetUserRole(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
            var okRes = result as OkObjectResult;
            var verify = okRes!.Value as List<RoleDto>;
            verify.Should().BeEquivalentTo(mappedRoleList);
        }

        [Fact]
        public async Task UserController_UpdateUserInfo_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = UserControllerCreate();

            var result = await controller.UpdateUserInfo(CreateRandomUserUpdate());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UserController_UpdateUserInfo_ReturnBadRequest_EmailAlreadyInUse()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserEmailAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = UserControllerCreate();

            var result = await controller.UpdateUserInfo(CreateRandomUserUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UserController_UpdateUserInfo_ReturnBadRequest_NicknameAlredyInUse()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserEmailAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            userRepositoryMock.Setup(r => r.UserNicknameAlreadyInUserAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            var controller = UserControllerCreate();

            var result = await controller.UpdateUserInfo(CreateRandomUserUpdate());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UserController_UpdateUserInfo_ReturnOk()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserEmailAlreadyInUseAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            userRepositoryMock.Setup(r => r.UserNicknameAlreadyInUserAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(help.CreateRandomUser());

            var controller = UserControllerCreate();

            var result = await controller.UpdateUserInfo(CreateRandomUserUpdate());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_DeleteUser_ReturnNotFound()
        {
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = UserControllerCreate();

            var result = await controller.DeleteUser(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UserController_DeleteUser_ReturnOk()
        {
            var user = help.CreateRandomUser();
            user.PicturePath = $"\\Images\\userPicture\\Def.jpg";
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            friendRepositoryMock.Setup(r => r.GetUserFriendsAsync(It.IsAny<int>())).ReturnsAsync(new List<Friend>());

            var controller = UserControllerCreate();

            var result = await controller.DeleteUser(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }


        private UserController UserControllerCreate()
        {
            return new UserController(userRepositoryMock.Object,
                roleRepositoryMock.Object, friendRepositoryMock.Object,
                personGamesRepositoryMock.Object, reviewRepositoryMock.Object,
                mapperMock.Object);
        }

        private UserUpdate CreateRandomUserUpdate()
        {
            return new UserUpdate
            {
                Id = rand.Next(100),
                Email = help.RandomString(20) + "@mail.ru",
                Password = help.RandomString(7),
                Nickname = help.RandomString(5),
                Age = rand.Next(15, 99),
                Gender = rand.Next(0, 1) == 0 ? "male" : "female",
            };
        }
    }
}
