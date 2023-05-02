using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IReviewRepository 
    {
        public Review GetReviewById(int reviewId);

        public Review GetReviewByUserIdAndGameId(int userId, int gameId);

        public IList<Review> GetUserReviews(int gameId);

        public IList<Review> GetGameReviews(int gameId);

        public bool ReviewExists(int reviewId);

        public bool ReviewExists(int userId, int gameId);

        public bool CreateReview(Review review);

        public bool UpdateReview(Review review);

        public bool DeleteReview(Review review);
    }
}
