using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Repositories
{
    public class AgeRatingRepository : IAgeRatingRepository
    {
        
        private readonly DataContext dataContext;

        public AgeRatingRepository(DataContext _dataContext)
        {
            dataContext = _dataContext;
        }

        public AgeRating GetAgeRatingById(int ageRatingId)
        {
            return dataContext.AgeRating.FirstOrDefault(a => a.Id == ageRatingId)!;
        }

        public AgeRating GetAgeRatingByName(string ageRatingName)
        {
            return dataContext.AgeRating.FirstOrDefault(a => a.Name == ageRatingName)!;
        }

        public IList<AgeRating> GetAgeRatings()
        {
            return dataContext.AgeRating.OrderBy(a => a.Id).ToList();
        }

        public bool AgeRatingExists(int ageRatingId)
        {
            return dataContext.AgeRating.Any(a => a.Id == ageRatingId);
        }

        public bool AgeRatingAlreadyExists(int ageRatingId, string ageRatingName)
        {
            return dataContext.AgeRating.Any(a => a.Name.Trim().ToLower() == ageRatingName.Trim().ToLower()
            && a.Id != ageRatingId);
        }

        //-------------------------------------------------------------------
        public bool CreateAgeRating(AgeRating ageRating)
        {
            dataContext.Add(ageRating);
            return Save();
        }

        public bool UpdateAgeRating(AgeRating ageRating)
        {
            dataContext.Update(ageRating);
            return Save();
        }

        public bool DeleteAgeRating(AgeRating ageRating)
        {
            dataContext.Remove(ageRating); 
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
