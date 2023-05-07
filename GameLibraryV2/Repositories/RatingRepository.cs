using GameLibraryV2.Data;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext dataContext;

        public RatingRepository(DataContext context)
        {
            dataContext = context;
        }

        public Task<Rating> GetRatingByIdAsync(int ratingId)
        {
            return dataContext.Ratings.FirstOrDefaultAsync(r => r.Id == ratingId)!;
        }

        public async Task<IList<Rating>> GetRatingsAsync()
        {
            return await dataContext.Ratings.ToListAsync();
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

        public async Task<bool> RatingExistsAsync(int ratingId)
        {
            return await dataContext.Ratings.AnyAsync(r => r.Id == ratingId);
        }

        public void SaveRating(Rating rating)
        {
            dataContext.Add(rating);
        }

        public void UpdateRating(Rating rating)
        {
            dataContext.Update(rating);
        }

        public void DeleteRating(Rating rating)
        {
            dataContext.Remove(rating);
        }

        public async Task SaveRatingAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
