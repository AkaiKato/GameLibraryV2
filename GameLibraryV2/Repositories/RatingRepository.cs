using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext dataContext;

        public RatingRepository(DataContext context)
        {
            dataContext = context;
        }

        public Rating GetRatingById(int ratingId)
        {
            return dataContext.Ratings.Where(r => r.Id == ratingId).FirstOrDefault()!;
        }

        public IList<Rating> GetRatings()
        {
            return dataContext.Ratings.ToList();
        }

        public void Remove(Rating rating, int number)
        {
            switch (number)
            {
                case 1:
                    rating.NumberOfOne--;
                    break;
                case 2:
                    rating.NumberOfTwo--;
                    break;
                case 3:
                    rating.NumberOfThree--;
                    break;
                case 4:
                    rating.NumberOfFour--;
                    break;
                case 5:
                    rating.NumberOfFive--;
                    break;
                case 6:
                    rating.NumberOfSix--;
                    break;
                case 7:
                    rating.NumberOfSeven--;
                    break;
                case 8:
                    rating.NumberOfEight--;
                    break;
                case 9:
                    rating.NumberOfNine--;
                    break;
                case 0:
                    rating.NumberOfTen--;
                    break;
            }
        }

        public void Add(Rating rating, int number)
        {
            switch (number)
            {
                case 1:
                    rating.NumberOfOne++;
                    break;
                case 2:
                    rating.NumberOfTwo++;
                    break;
                case 3:
                    rating.NumberOfThree++;
                    break;
                case 4:
                    rating.NumberOfFour++;
                    break;
                case 5:
                    rating.NumberOfFive++;
                    break;
                case 6:
                    rating.NumberOfSix++;
                    break;
                case 7:
                    rating.NumberOfSeven++;
                    break;
                case 8:
                    rating.NumberOfEight++;
                    break;
                case 9:
                    rating.NumberOfNine++;
                    break;
                case 0:
                    rating.NumberOfTen++;
                    break;
            }
        }

        public bool RatingExists(int ratingId)
        {
            return dataContext.Ratings.Any(r => r.Id == ratingId);
        }

        public bool SaveRating(Rating rating)
        {
            dataContext.Add(rating);
            return Save();
        }

        public bool UpdateRating(Rating rating)
        {
            dataContext.Update(rating);
            return Save();
        }

        public bool DeleteRating(Rating rating)
        {
            dataContext.Remove(rating);
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
