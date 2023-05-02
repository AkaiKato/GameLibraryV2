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

        public Review GetReviewById(int reviewId) 
        {
            return dataContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault()!;
        }

        public Review GetReviewByUserIdAndGameId(int userId, int gameId)
        {
            return dataContext.Reviews.Where(r => r.User.Id == userId && r.Game.Id == gameId).FirstOrDefault()!;
        }

        public IList<Review> GetUserReviews(int userId)
        {
            return dataContext.Reviews.Where(r => r.User.Id == userId).Select(x => new Review
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
            }).ToList();
        }

        public IList<Review> GetGameReviews(int gameId)
        {
            return dataContext.Reviews.Include(u => u.User).Where(d => d.Game.Id == gameId).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return dataContext.Reviews.Any(r => r.Id == reviewId);
        }

        public bool ReviewExists(int userId, int gameId)
        {
            return dataContext.Reviews.Any(r => r.User.Id == userId && r.Game.Id == gameId);
        }

        public bool CreateReview(Review review)
        {
            dataContext.Add(review);
            return Save();
        }

        public bool UpdateReview(Review review)
        {
            dataContext.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            dataContext.Remove(review);
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
