using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IReviewRepository 
    {
        public Task<Review> GetReviewByIdAsync(int reviewId);

        public Task<Review> GetReviewByUserIdAndGameIdAsync(int userId, int gameId);

        public Task<IList<Review>> GetUserReviewsAsync(int gameId);

        public Task<IList<Review>> GetGameReviewsAsync(int gameId);

        public Task<bool> ReviewExistsAsync(int reviewId);

        public Task<bool> ReviewExistsAsync(int userId, int gameId);

        public void CreateReview(Review review);

        public void UpdateReview(Review review);

        public void DeleteReview(Review review);

        public Task SaveReviewAsync();
    }
}
