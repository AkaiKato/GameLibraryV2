using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Interfaces
{
    public interface IAgeRatingRepository
    {
        public Task<AgeRating> GetAgeRatingByIdAsync(int ageRatingId);

        public Task<AgeRating> GetAgeRatingByNameAsync(string ageRatingName);

        public Task<IList<AgeRating>> GetAgeRatingsAsync();

        public Task<bool> AgeRatingExistsAsync(int ageRatingId);

        public Task<bool> AgeRatingAlreadyExistsAsync(int ageRatingId, string ageRatingName);

        public void CreateAgeRating(AgeRating ageRating);

        public void UpdateAgeRating(AgeRating ageRating);

        public void DeleteAgeRating(AgeRating ageRating);

        public Task SaveAgeRatingAsync();
    }
}
