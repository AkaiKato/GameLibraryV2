using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class AgeRatingRepository : IAgeRatingRepository
    {
        
        private readonly DataContext dataContext;

        public AgeRatingRepository(DataContext _dataContext)
        {
            dataContext = _dataContext;
        }

        public async Task<AgeRating> GetAgeRatingByIdAsync(int ageRatingId)
        {
            return await dataContext.AgeRating.FirstOrDefaultAsync(a => a.Id == ageRatingId);
        }

        public async Task<AgeRating> GetAgeRatingByNameAsync(string ageRatingName)
        {
            return await dataContext.AgeRating.FirstOrDefaultAsync(a => a.Name == ageRatingName);
        }

        public async Task<IList<AgeRating>> GetAgeRatingsAsync()
        {
            return await dataContext.AgeRating.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> AgeRatingExistsAsync(int ageRatingId)
        {
            return await dataContext.AgeRating.AnyAsync(a => a.Id == ageRatingId);
        }

        public async Task<bool> AgeRatingAlreadyExistsAsync(int ageRatingId, string ageRatingName)
        {
            return await dataContext.AgeRating.
                AnyAsync(a => a.Name.Trim().ToLower() == ageRatingName.Trim().ToLower() && a.Id != ageRatingId);
        }

        //-------------------------------------------------------------------
        public void CreateAgeRating(AgeRating ageRating)
        {
            dataContext.Add(ageRating);
        }

        public void UpdateAgeRating(AgeRating ageRating)
        {
            dataContext.Update(ageRating);
        }

        public void DeleteAgeRating(AgeRating ageRating)
        {
            dataContext.Remove(ageRating); 
        }

        public async Task SaveAgeRatingAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
