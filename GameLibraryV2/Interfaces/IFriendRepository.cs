using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IFriendRepository
    {
        public IList<Friend> GetUserFriends(int userId);

        public bool CreateFriend(Friend friend);

        public bool DeleteFriend(Friend friend);
    }
}
