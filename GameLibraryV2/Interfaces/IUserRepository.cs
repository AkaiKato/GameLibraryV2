using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(int userId);

        public Task<User> GetUserByNicknameAsync(string nickename);

        public Task<IList<User>> GetUsersAsync();

        public Task<IList<User>> GetUsersByRoleAsync(int roleId);

        public Task<string> GetUserPicturePathAsync(int userId);

        public Task<bool> UserExistsByIdAsync(int userId);

        public Task<bool> UserExistsByNicknameAsync(string Nickname);

        public Task<bool> HasNicknameAsync(string nickname);

        public Task<bool> HasEmailAsync(string email);

        public Task<bool> UserNicknameAlreadyInUserAsync(int userId, string nickname);

        public Task<bool> UserEmailAlreadyInUseAsync(int userId, string email);

        public void CreateUser(User user);

        public void UpdateUser(User user);

        public void DeleteUser(User user);

        public Task SaveUserAsync();
    }
}
