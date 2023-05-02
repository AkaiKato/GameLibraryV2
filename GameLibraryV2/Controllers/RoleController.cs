using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
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
        [ProducesResponseType(200, Type = typeof(IList<RoleDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetRoles()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Roles = mapper.Map<List<RoleDto>>(roleRepository.GetRoles());

            return Ok(Roles);
        }

        /// <summary>
        /// Return specified role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [ProducesResponseType(200, Type = typeof(RoleDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPublisherById(int roleId)
        {
            if (!roleRepository.RoleExists(roleId))
                return NotFound($"Not found role with such id {roleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Role = mapper.Map<RoleDto>(roleRepository.GetRoleById(roleId));

            return Ok(Role);
        }


        /// <summary>
        /// Return all users by specified role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("{roleId}/users")]
        [ProducesResponseType(200, Type = typeof(IList<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetRoleUsers(int roleId)
        {
            if (!roleRepository.RoleExists(roleId))
                return NotFound($"Not found role with such id {roleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var User = mapper.Map<List<UserDto>>(userRepository.GetUsersByRole(roleId));

            return Ok(User);
        }

        /// <summary>
        /// Add specified role to user
        /// </summary>
        /// <param name="addRole"></param>
        /// <returns></returns>
        [HttpPut("addRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddRole([FromBody] RoleUpdate addRole)
        {
            if (!userRepository.UserExistsById(addRole.UserId))
                return NotFound($"Not found user with such id {addRole.UserId}");

            if (!roleRepository.RoleExists(addRole.RoleId))
                return NotFound($"Not found role with such id {addRole.RoleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(addRole.UserId);
            var role = roleRepository.GetRoleById(addRole.RoleId);
            user.UserRoles.Add(role);

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added");
        }

        /// <summary>
        /// Creates new Role
        /// </summary>
        /// <param name="roleCreate"></param>
        /// <returns></returns>
        [HttpPost("createRole")]  
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRole([FromBody] RoleCreateDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest(ModelState);

            var role = roleRepository.GetRoleByName(roleCreate.RoleName);

            if (role != null)
                return BadRequest("Role already exists");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleMap = mapper.Map<Role>(roleCreate);

            if (!roleRepository.CreateRole(roleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Delete specified role of user 
        /// </summary>
        /// <param name="deleteRole"></param>
        /// <returns></returns>
        [HttpDelete("deleteUserRole")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteRole([FromBody] RoleUpdate deleteRole)
        {
            if (!userRepository.UserExistsById(deleteRole.UserId))
                return NotFound($"Not found user with such id {deleteRole.UserId}");

            if (!roleRepository.RoleExists(deleteRole.RoleId))
                return NotFound($"Not found user with such id {deleteRole.RoleId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = userRepository.GetUserById(deleteRole.UserId);
            var fusers = roleRepository.GetUserRole(deleteRole.UserId).ToList();

            if (fusers.Count <= 1)
                return BadRequest("User can't have less then one role Or user doesn't have this role");

            fusers.RemoveAll(x => x.Id == deleteRole.RoleId);

            user.UserRoles = fusers;

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
