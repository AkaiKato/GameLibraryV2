using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
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

        public async Task<Review> GetReviewByIdAsync(int reviewId) 
        {
            return await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId)!;
        }

        public async Task<Review> GetReviewByUserIdAndGameIdAsync(int userId, int gameId)
        {
            return await dataContext.Reviews.FirstOrDefaultAsync(r => r.User.Id == userId && r.Game.Id == gameId)!;
        }

        public async Task<IList<Review>> GetUserReviewsAsync(int userId)
        {
            return await dataContext.Reviews.Where(r => r.User.Id == userId).Select(x => new Review
            {
                Id = x.Id,
                User = new User
                {
                    Id = x.User.Id,
                    Nickname = x.User.Nickname,
                    PicturePath = x.User.PicturePath,
                },
                Game = new Game
                {
                    Id = x.Game.Id,
                    Name = x.Game.Name,
                },
                Rating = x.Rating,
                Text = x.Text,
                PublishDate = x.PublishDate,
                ReviewRating = x.ReviewRating,
            }).ToListAsync();
        }

        public async Task<IList<Review>> GetGameReviewsAsync(int gameId)
        {
            return await dataContext.Reviews.Include(u => u.User).Include(g => g.Game).Where(d => d.Game.Id == gameId).ToListAsync();
        }

        public async Task<bool> ReviewExistsAsync(int reviewId)
        {
            return await dataContext.Reviews.AnyAsync(r => r.Id == reviewId);
        }

        public async Task<bool> ReviewExistsAsync(int userId, int gameId)
        {
            return await dataContext.Reviews.AnyAsync(r => r.User.Id == userId && r.Game.Id == gameId);
        }

        public void CreateReview(Review review)
        {
            dataContext.Add(review);
        }

        public void UpdateReview(Review review)
        {
            dataContext.Update(review);
        }

        public void DeleteReview(Review review)
        {
            dataContext.Remove(review);
        }

        public async Task SaveReviewAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
