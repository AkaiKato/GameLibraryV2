using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role> GetRoleByIdAsync(int roleId);

        public Task<Role> GetRoleByNameAsync(string roleName);

        public Task<IList<Role>> GetRolesAsync();

        public Task<IList<Role>> GetUserRoleAsync(int userId);

        public Task<bool> RoleExistsAsync(int roleId);

        public void CreateRole(Role role);

        public Task SaveRoleAsync();
    }
}
