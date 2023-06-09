﻿using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
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

        public async Task<IList<Friend>> GetUserFriendsAsync(int userId)
        {
            return await dataContext.Friends.Include(u => u.Friendu).Where(f => f.User.Id == userId).ToListAsync();
        }

        public async Task<bool> FriendExists(int connectionId)
        {
            return await dataContext.Friends.AnyAsync(f => f.Id == connectionId);
        }

        public async Task<Friend> GetFriendAsync(int connectionId)
        {
            return await dataContext.Friends.FirstOrDefaultAsync(f => f.Id == connectionId);
        }

        public void CreateFriend(Friend friend)
        {
            dataContext.Add(friend);
        }

        public void DeleteFriend(Friend friend)
        {
            dataContext.Remove(friend);
        }

        public async Task SaveFriendAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
