using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;

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

        public IList<User> GetUsersByRole(int roleId)
        {
            return dataContext.Users.Where(u => u.UserRoles.Any(u => u.Id == roleId)).ToList();
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
