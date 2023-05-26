using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static GameLibraryV2.Helper.Enums;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemRequirementsController : Controller
    {
        private readonly ISystemRequirements systemRequirements;

        public SystemRequirementsController(ISystemRequirements _systemRequirements)
        {
            systemRequirements = _systemRequirements;
        }

        [HttpPut("updateSystemRequirements")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateSystemRequirements([FromBody] SystemRequirements systemReq)
        {
            if (!await systemRequirements.SystemRequirementsExists(systemReq.Id))
                return NotFound("Not found systemRequirements with such Id");

            var sys = await systemRequirements.GetSystemRequirementsAsync(systemReq.Id);

            sys.Type = systemReq.Type;
            sys.OC = systemReq.OC;
            sys.Processor = systemReq.Processor;
            sys.RAM = systemReq.RAM;
            sys.VideoCard = systemReq.VideoCard;
            sys.DirectX = systemReq.DirectX;
            sys.Ethernet = systemReq.Ethernet;
            sys.HardDriveSpace = systemReq.HardDriveSpace;
            sys.Additional = systemReq.Additional;

            systemRequirements.UpdateSystemRequirements(sys);
            await systemRequirements.SaveSystemRequirementsAsync();

            return Ok("Successfully updated");
        }

    }
}
