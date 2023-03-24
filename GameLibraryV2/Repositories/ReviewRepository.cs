﻿using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dataContext;

        public ReviewRepository(DataContext context)
        {
            dataContext = context;
        }

        public IList<Review> GetGameReviews(int gameId)
        {
            return dataContext.Reviews.Include(u => u.User).Where(d => d.Game.Id == gameId).ToList();
        }

        public bool CreateReview(Review review)
        {
            dataContext.Add(review);
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
