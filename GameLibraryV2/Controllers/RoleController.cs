using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public RoleController(IRoleRepository _roleRepository, IUserRepository _userRepository,IMapper _mapper)
        {
            roleRepository = _roleRepository;
            userRepository = _userRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("roleAll")]
        public async Task<IActionResult> GetRoles()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Roles = mapper.Map<List<RoleDto>>(await roleRepository.GetRolesAsync());

            return Ok(Roles);
        }

        /// <summary>
        /// Return specified role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(int roleId)
        {
            if (!await roleRepository.RoleExistsAsync(roleId))
                return NotFound($"Not found role with such id {roleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Role = mapper.Map<RoleDto>(await roleRepository.GetRoleByIdAsync(roleId));

            return Ok(Role);
        }


        /// <summary>
        /// Return all users by specified role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}/users")]
        public async Task<IActionResult> GetRoleUsers(int roleId)
        {
            if (!await roleRepository.RoleExistsAsync(roleId))
                return NotFound($"Not found role with such id {roleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var User = mapper.Map<List<UserDto>>(await userRepository.GetUsersByRoleAsync(roleId));

            return Ok(User);
        }

        /// <summary>
        /// Add specified role to user
        /// </summary>
        /// <param name="addRole"></param>
        /// <returns></returns>
        [HttpPut("addRole")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddRole([FromBody] RoleUpdate addRole)
        {
            if(addRole == null)
                return BadRequest(ModelState);

            if (!await userRepository.UserExistsByIdAsync(addRole.UserId))
                return NotFound($"Not found user with such id {addRole.UserId}");

            if (!await roleRepository.RoleExistsAsync(addRole.RoleId))
                return NotFound($"Not found role with such id {addRole.RoleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.GetUserByIdAsync(addRole.UserId);
            var role = await roleRepository.GetRoleByIdAsync(addRole.RoleId);
            user.UserRoles.Add(role);

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            return Ok("Successfully added");
        }

        /// <summary>
        /// Creates new Role
        /// </summary>
        /// <param name="roleCreate"></param>
        /// <returns></returns>
        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest(ModelState);

            var role = await roleRepository.GetRoleByNameAsync(roleCreate.RoleName);

            if (role != null)
                return BadRequest("Role already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleMap = mapper.Map<Role>(roleCreate);

            roleRepository.CreateRole(roleMap);
            await roleRepository.SaveRoleAsync();

            return Ok("Successfully created");
        }

        /// <summary>
        /// Delete specified role of user 
        /// </summary>
        /// <param name="deleteRole"></param>
        /// <returns></returns>
        [HttpDelete("deleteUserRole")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleUpdate deleteRole)
        {
            if (deleteRole == null)
                return BadRequest(ModelState);

            if (!await userRepository.UserExistsByIdAsync(deleteRole.UserId))
                return NotFound($"Not found user with such id {deleteRole.UserId}");

            if (!await roleRepository.RoleExistsAsync(deleteRole.RoleId))
                return NotFound($"Not found user with such id {deleteRole.RoleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userRepository.GetUserByIdAsync(deleteRole.UserId);
            var fusers = (await roleRepository.GetUserRoleAsync(deleteRole.UserId)).ToList();

            if (fusers.Count <= 1)
                return BadRequest("User can't have less then one role Or user doesn't have this role");

            fusers.RemoveAll(x => x.Id == deleteRole.RoleId);

            user.UserRoles = fusers;

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            return Ok("Successfully deleted");
        }
    }
}
