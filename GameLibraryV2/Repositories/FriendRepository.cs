using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly DataContext dataContext;

        public FriendRepository(DataContext context)
        {
            dataContext = context;
        }
        public IList<Friend> GetUserFriends(int userId)
        {
            return dataContext.Friends.Include(u => u.Friendu).Where(f => f.User.Id == userId).ToList();
        }

        public bool CreateFriend(Friend friend)
        {
            dataContext.Add(friend);
            return Save();
        }

        public bool DeleteFriend(Friend friend)
        {
            dataContext.Remove(friend);
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
