using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext context)
        {
            dataContext = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await dataContext.Users.Include(l => l.UserGames)
                .Include(ur => ur.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId)!;
        }

        public async Task<User> GetUserByNicknameAsync(string nickname)
        {
            return await dataContext.Users.Include(l => l.UserGames)
                .Include(ur => ur.UserRoles)
                .FirstOrDefaultAsync(u => u.Nickname.Trim() == nickname.Trim())!;
        }

        public async Task<string> GetUserPicturePathAsync(int userId)
        {
            return await dataContext.Users.Where(u => u.Id == userId)
                .Select(u => u.PicturePath).FirstOrDefaultAsync();
        }

        public async Task<IList<User>> GetUsersByRoleAsync(int roleId)
        {
            return await dataContext.Users.Where(u => u.UserRoles.Any(u => u.Id == roleId)).ToListAsync();
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await dataContext.Users.Select(u => new User
            {
                Id = u.Id,
                Email = u.Email,
                Nickname = u.Nickname,
                Age = u.Age,
                Gender = u.Gender,
                PicturePath= u.PicturePath,
                RegistrationdDate = u.RegistrationdDate,
                UserGames = u.UserGames,
                UserRoles = u.UserRoles.Select(t => new Role
                {
                    Id = t.Id,
                    RoleName = t.RoleName,
                }).ToList(),
            }).ToListAsync();
        }

        public async Task<bool> UserExistsByIdAsync(int userId)
        {
            return await dataContext.Users.AnyAsync(u => u.Id == userId);
        }

        public async Task<bool> UserExistsByNicknameAsync(string nickname)
        {
            return await dataContext.Users.AnyAsync(u => u.Nickname.Trim() == nickname.Trim());
        }

        public async Task<bool> HasNicknameAsync(string nickname)
        {
            return await dataContext.Users.AnyAsync(u => u.Nickname.Trim() == nickname.Trim());
        }

        public async Task<bool> HasEmailAsync(string email)
        {
            return await dataContext.Users.AnyAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower());
        }

        public async Task<bool> UserNicknameAlreadyInUserAsync(int userId, string nickname)
        {
            return await dataContext.Users.AnyAsync(u => u.Nickname.Trim() == nickname.Trim() && u.Id != userId);
        }

        public async Task<bool> UserEmailAlreadyInUseAsync(int userId, string email)
        {
            return await dataContext.Users.AnyAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower() && u.Id != userId);
        }

        public void CreateUser(User user)
        {
            user.UserRoles = new List<Role>() { dataContext.Roles.FirstOrDefault(r => r.RoleName.Trim().ToLower() == "user")!};
            dataContext.Add(user);
        }

        public void UpdateUser(User user)
        {
            dataContext.Update(user);
        }

        public void DeleteUser(User user)
        {
            dataContext.Remove(user);
        }

        public async Task SaveUserAsync()
        {
            await dataContext.SaveChangesAsync();
        }

    }
}
