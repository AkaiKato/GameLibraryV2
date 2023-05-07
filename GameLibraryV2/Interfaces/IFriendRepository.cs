using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IFriendRepository
    {
        public Task<IList<Friend>> GetUserFriendsAsync(int userId);

        public void CreateFriend(Friend friend);

        public void DeleteFriend(Friend friend);

        public Task SaveFriendAsync();
    }
}
