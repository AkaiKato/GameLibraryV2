using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IRatingRepository
    {
        public Task<IList<Rating>> GetRatingsAsync();

        public Task<Rating> GetRatingByIdAsync(int ratingId);

        public void Remove(Rating rating, int number);

        public void Add(Rating rating, int number);

        public Task<bool> RatingExistsAsync(int ratingId);

        public void SaveRating(Rating rating);

        public void UpdateRating(Rating rating);
        
        public void DeleteRating(Rating rating);

        public Task SaveRatingAsync();
    }
}
