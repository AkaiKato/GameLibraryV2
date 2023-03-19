using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IRoleRepository
    {
        public Role GetRoleById(int roleId);

        public Role GetRoleByName(string roleName);

        public IList<Role> GetRoles();

        public IList<User> GetUsersByRole(int roleId);

        public bool RoleExists(int roleId);
    }
}
