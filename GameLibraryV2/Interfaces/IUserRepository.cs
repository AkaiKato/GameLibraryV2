using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserById(int userId);

        public User GetUserByNickname(string nickename);

        public IList<User> GetUsers();

        public IList<Friend> GetUserFriends(int userId);

        public IList<Role> GetUserRole(int userId);

        public string GetUserPicturePath(int userId);

        public bool UserExists(int userId);

        public bool HasNickname(string nickname);

        public bool HasEmail(string email);

        public bool CreateUser(User user);
    }
}
