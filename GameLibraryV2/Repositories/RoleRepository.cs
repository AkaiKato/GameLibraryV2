using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext dataContext;

        public RoleRepository(DataContext context)
        {
            dataContext = context;
        }

        public Role GetRoleById(int roleId)
        {
            return dataContext.Roles.Where(r => r.Id == roleId).FirstOrDefault()!;
        }

        public Role GetRoleByName(string roleName)
        {
            return dataContext.Roles.Where(r => r.RoleName.Trim().ToLower() == roleName.Trim().ToLower()).FirstOrDefault()!;
        }

        public IList<Role> GetRoles()
        {
            return dataContext.Roles.OrderBy(r => r.Id).ToList();
        }
        public IList<Role> GetUserRole(int userId)
        {
            return dataContext.Roles.Where(u => u.RoleUsers!.Any(u => u.Id == userId)).ToList();
        }

        public bool RoleExists(int roleId)
        {
            return dataContext.Roles.Any(r => r.Id == roleId);
        }

        //--------------------------------------------

        public bool CreateRole(Role role)
        {
            dataContext.Add(role);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            //var saved = 1;
            return saved > 0 ? true : false;
        }
    }
}
