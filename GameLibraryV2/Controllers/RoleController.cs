using AutoMapper;
using GameLibraryV2.Dto;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public RoleController(IRoleRepository _roleRepository, IMapper _mapper)
        {
            roleRepository = _roleRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IList<RoleDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetRoles()
        {
            var Roles = mapper.Map<List<RoleDto>>(roleRepository.GetRoles());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Roles));
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
                return NotFound();

            var Role = mapper.Map<RoleDto>(roleRepository.GetRoleById(roleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(Role));
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
                return NotFound();

            var User = mapper.Map<List<UserDto>>(roleRepository.GetUsersByRole(roleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Json(User));
        }

        /// <summary>
        /// Creates new Role
        /// </summary>
        /// <param name="roleCreate"></param>
        /// <returns></returns>
        [HttpPost]  
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRole([FromBody] RoleCreateDto roleCreate)
        {
            if (roleCreate == null)
                return BadRequest(ModelState);

            var role = roleRepository.GetRoleByName(roleCreate.RoleName);

            if (role != null)
            {
                ModelState.AddModelError("", "Role already exists");
                return StatusCode(422, ModelState);
            }

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
    }
}
