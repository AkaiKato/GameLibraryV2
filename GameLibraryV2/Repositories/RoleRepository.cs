using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext dataContext;

        public RoleRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await dataContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId)!;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await dataContext.Roles.FirstOrDefaultAsync(r => r.RoleName.Trim().ToLower() == roleName.Trim().ToLower());
        }

        public async Task<IList<Role>> GetRolesAsync()
        {
            return await dataContext.Roles.OrderBy(r => r.Id).ToListAsync();
        }
        public async Task<IList<Role>> GetUserRoleAsync(int userId)
        {
            return await dataContext.Roles.Where(u => u.RoleUsers!.Any(u => u.Id == userId)).ToListAsync();
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await dataContext.Roles.AnyAsync(r => r.Id == roleId);
        }

        //--------------------------------------------

        public void CreateRole(Role role)
        {
            dataContext.Add(role);
        }

        public async Task SaveRoleAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
