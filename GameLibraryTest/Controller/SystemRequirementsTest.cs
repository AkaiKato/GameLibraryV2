using FluentAssertions;
using GameLibraryV2.Controllers;
using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameLibraryTest.Controller
{
    public class SystemRequirementsTest
    {
        private readonly Mock<ISystemRequirements> systemRequirementsRepositoryMock = new();
        readonly Helper help = new();

        [Fact]
        public async Task SystemRequirementsController_UpdateSystemRequirements_ReturnNotFound()
        {
            systemRequirementsRepositoryMock.Setup(r => r.SystemRequirementsExists(It.IsAny<int>()))!.ReturnsAsync(false);

            var controller = SystemRequirementsControllerCreate();

            var result = await controller.UpdateSystemRequirements(help.CreateSystemRequirements());

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task SystemRequirementsController_UpdateSystemRequirements_ReturnOk()
        {
            systemRequirementsRepositoryMock.Setup(r => r.SystemRequirementsExists(It.IsAny<int>()))!.ReturnsAsync(true);
            systemRequirementsRepositoryMock.Setup(r => r.GetSystemRequirementsAsync(It.IsAny<int>())).ReturnsAsync(help.CreateSystemRequirements());

            var controller = SystemRequirementsControllerCreate();

            var result = await controller.UpdateSystemRequirements(help.CreateSystemRequirements());

            result.Should().BeOfType<OkObjectResult>();
        }

        private SystemRequirementsController SystemRequirementsControllerCreate()
        {
            return new SystemRequirementsController(systemRequirementsRepositoryMock.Object);
        }
    }
}
