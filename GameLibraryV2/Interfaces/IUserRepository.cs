using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserById(int userId);

        public User GetUserByNickname(string nickename);

        public IList<User> GetUsers();

        public IList<User> GetUsersByRole(int roleId);

        public string GetUserPicturePath(int userId);

        public bool UserExists(int userId);

        public bool HasNickname(string nickname);

        public bool HasEmail(string email);

        public bool CreateUser(User user);

        public bool UpdateUser(User user);

        public bool DeleteUser(User user);
    }
}
