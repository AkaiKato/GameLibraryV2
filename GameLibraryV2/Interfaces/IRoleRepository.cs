using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IRoleRepository
    {
        public Role GetRoleById(int roleId);

        public Role GetRoleByName(string roleName);

        public IList<Role> GetRoles();

        public IList<Role> GetUserRole(int userId);

        public bool RoleExists(int roleId);

        public bool CreateRole(Role role);
    }
}
