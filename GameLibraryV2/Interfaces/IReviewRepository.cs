using GameLibraryV2.Models;

namespace GameLibraryV2.Interfaces
{
    public interface IReviewRepository 
    {
        public IList<Review> GetGameReviews(int gameId);

        public bool CreateReview(Review review);
    }
}
