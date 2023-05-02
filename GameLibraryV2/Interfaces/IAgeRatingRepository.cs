using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IAgeRatingRepository
    {
        public AgeRating GetAgeRatingById(int ageRatingId);

        public AgeRating GetAgeRatingByName(string ageRatingName);

        public IList<AgeRating> GetAgeRatings();

        public bool AgeRatingExists(int ageRatingId);

        public bool AgeRatingAlreadyExists(int ageRatingId, string ageRatingName);

        public bool CreateAgeRating(AgeRating ageRating);

        public bool UpdateAgeRating(AgeRating ageRating);

        public bool DeleteAgeRating(AgeRating ageRating);
    }
}
