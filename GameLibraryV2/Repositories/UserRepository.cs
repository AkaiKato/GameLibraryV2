using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
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

        public User GetUserById(int userId)
        {
            return dataContext.Users.Include(l => l.UserGames).Include(ur => ur.UserRoles).Where(u => u.Id == userId).FirstOrDefault()!;
        }

        public User GetUserByNickname(string nickname)
        {
            return dataContext.Users.Include(l => l.UserGames).Include(ur => ur.UserRoles).Where(u => u.Nickname.Trim() == nickname.Trim()).FirstOrDefault()!;
        }

        public string GetUserPicturePath(int userId)
        {
            return dataContext.Users.Where(u => u.Id == userId).Select(u => u.PicturePath).
                FirstOrDefault()!;
        }

        public IList<User> GetUsersByRole(int roleId)
        {
            return dataContext.Users.Where(u => u.UserRoles.Any(u => u.Id == roleId)).ToList();
        }

        public IList<User> GetUsers()
        {
            return dataContext.Users.Select(u => new User
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
            }).ToList();
        }

        public bool UserExists(int userId)
        {
            return dataContext.Users.Any(u => u.Id == userId);
        }

        public bool HasNickname(string nickname)
        {
            return dataContext.Users.Any(u => u.Nickname.Trim() == nickname.Trim());
        }

        public bool HasEmail(string email)
        {
            return dataContext.Users.Any(u => u.Email.Trim().ToLower() == email.Trim().ToLower());
        }

        public bool UserNicknameAlreadyInUser(int userId, string nickname)
        {
            return dataContext.Users.Any(u => u.Nickname.Trim() == nickname.Trim() && u.Id != userId);
        }

        public bool UserEmailAlreadyInUse(int userId, string email)
        {
            return dataContext.Users.Any(u => u.Email.Trim().ToLower() == email.Trim().ToLower() && u.Id != userId);
        }

        public bool CreateUser(User user)
        {
            user.UserRoles = new List<Role>() { dataContext.Roles.FirstOrDefault(r => r.RoleName.Trim().ToLower() == "user")!};
            dataContext.Add(user);
            return Save();
        }

        public bool UpdateUser(User user)
        {
            dataContext.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            dataContext.Remove(user);
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
