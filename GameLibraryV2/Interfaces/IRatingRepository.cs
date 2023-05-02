using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IRatingRepository
    {
        public IList<Rating> GetRatings();

        public Rating GetRatingById(int ratingId);

        public void Remove(Rating rating, int number);

        public void Add(Rating rating, int number);

        public bool RatingExists(int ratingId);

        public bool SaveRating(Rating rating);

        public bool UpdateRating(Rating rating);
        
        public bool DeleteRating(Rating rating);
    }
}
