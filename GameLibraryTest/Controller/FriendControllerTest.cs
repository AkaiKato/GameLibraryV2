using AutoMapper;
using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class FriendControllerTest
    {
        private readonly Mock<IFriendRepository> friendRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IMapper> mapperMock = new();
        readonly Random rand = new();
        readonly Helper help = new();


        [Fact]
        public async Task FriendController_GetUserFriends_ReturnNotFound()
        {
            var expectedItem = help.CreateRandomUser();
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = FriendControllerCreate();

            var result = await controller.GetUserFriends(expectedItem.Id);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verifyRes = (okResult!.Value as string);
            verifyRes.Should().BeEquivalentTo($"Not found user with such id {expectedItem.Id}");
        }

        [Fact]
        public async Task FriendController_GetUserFriends_ReturnOk()
        {
            int numberItemsExpected = 10;
            var expectedItems = new List<Friend>();
            for (int i = 0; i < numberItemsExpected; i++)
            {
                expectedItems.Add(help.CreateRandomFriend());
            }

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();
            var mappedExpected = mapper.Map<List<FriendDto>>(expectedItems);

            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            friendRepositoryMock.Setup(r => r.GetUserFriendsAsync(It.IsAny<int>())).ReturnsAsync(expectedItems);
            mapperMock.Setup(r => r.Map<List<FriendDto>>(expectedItems)).Returns(mappedExpected);

            var controller = FriendControllerCreate();

            var result = await controller.GetUserFriends(It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var verifyRes = (okResult!.Value as List<FriendDto>);
            verifyRes.Should().BeEquivalentTo(mappedExpected);
        }
        
        [Fact]
        public async Task FriendController_AddFriend_ReturnNotFound_NotFoundUserWithId()
        {
            var friend = CreateRandomFriendUpdate();
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = FriendControllerCreate();

            var result = await controller.AddFriend(friend);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo($"Not found user with such id {friend.UserId}");
        }

        [Fact]
        public async Task FriendController_AddFriend_ReturnNotFound_NotFoundFriendWithId()
        {
            var friend = CreateRandomFriendUpdate();
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(friend.UserId))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(friend.FriendId))!.ReturnsAsync(false);

            var controller = FriendControllerCreate();

            var result = await controller.AddFriend(friend);

            result.Should().BeOfType<NotFoundObjectResult>();
            var okResult = result as NotFoundObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo($"Not found user with such id {friend.FriendId}");
        }

        [Fact]
        public async Task FriendController_AddFriend_ReturnBadRequest()
        {
            var friend = CreateRandomFriendUpdate();
            var userExpected = help.CreateRandomUser();
            var friendExpected = new List<Friend>(){ help.CreateRandomFriend() };
            foreach (var item in friendExpected)
            {
                item.Friendu.Id = friend.FriendId;
            }
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(friend.UserId))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(friend.FriendId))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(userExpected);
            friendRepositoryMock.Setup(r => r.GetUserFriendsAsync(It.IsAny<int>())).ReturnsAsync(friendExpected);

            var controller = FriendControllerCreate();

            var result = await controller.AddFriend(friend);

            result.Should().BeOfType<BadRequestObjectResult>();
            var okResult = result as BadRequestObjectResult;
            var verify = (okResult!.Value as string);
            verify.Should().BeEquivalentTo("Already friends");
        }

        [Fact]
        public async Task FriendController_AddFriend_ReturnOk()
        {
            var friend = CreateRandomFriendUpdate();
            var userExpected = help.CreateRandomUser();
            var friendExpected = new List<Friend>() { help.CreateRandomFriend() };
            foreach (var item in friendExpected)
            {
                if(item.Friendu.Id == friend.FriendId)
                {
                    item.Friendu.Id++;
                }
                
            }
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(friend.UserId))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.UserExistsByIdAsync(friend.FriendId))!.ReturnsAsync(true);
            userRepositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(userExpected);
            friendRepositoryMock.Setup(r => r.GetUserFriendsAsync(It.IsAny<int>())).ReturnsAsync(friendExpected);

            var controller = FriendControllerCreate();

            var result = await controller.AddFriend(friend);

            result.Should().BeOfType<OkObjectResult>();
        }
        

        [Fact]
        public async Task FriendController_DeleteFriend_ReturnNotFound()
        {
            friendRepositoryMock.Setup(r => r.FriendExists(It.IsAny<int>())).ReturnsAsync(false);

            var controller = FriendControllerCreate();

            var result = await controller.DeleteFriend(It.IsAny<int>());

            result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public async Task DLCController_DeleteGameDlc_ReturnOk()
        {
            var expectedItem = help.CreateRandomFriend();

            friendRepositoryMock.Setup(r => r.FriendExists(It.IsAny<int>())).ReturnsAsync(true);
            friendRepositoryMock.Setup(r => r.GetFriendAsync(It.IsAny<int>())).ReturnsAsync(expectedItem);

            var controller = FriendControllerCreate();

            var result = await controller.DeleteFriend  (It.IsAny<int>());

            result.Should().BeOfType<OkObjectResult>();
        }

        private FriendController FriendControllerCreate()
        {
            return new FriendController(friendRepositoryMock.Object,
                userRepositoryMock.Object, mapperMock.Object);
        }

        private FriendUpdate CreateRandomFriendUpdate()
        {
            return new FriendUpdate
            {
                UserId = rand.Next(100),
                FriendId = rand.Next(100),
            };
        }
    }
}
