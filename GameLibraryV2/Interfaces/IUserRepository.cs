using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserById(int userId);

        public User GetUserByNickname(string nickename);

        public IList<User> GetUsers();

        public IList<User> GetUsersByRole(int roleId);

        public string GetUserPicturePath(int userId);

        public bool UserExistsById(int userId);

        public bool UserExistsByNickname(string Nickname);

        public bool HasNickname(string nickname);

        public bool HasEmail(string email);

        public bool UserNicknameAlreadyInUser(int userId, string nickname);

        public bool UserEmailAlreadyInUse(int userId, string email);

        public bool CreateUser(User user);

        public bool UpdateUser(User user);

        public bool DeleteUser(User user);
    }
}
