using System.ComponentModel.DataAnnotations;

namespace GameLibraryV2.Dto.Update
{
    public class ReviewRatingUpdate
    {
        public int Id { get; set; }

        [Range (-1,1)]
        public int ReviewRating { get; set; }
    }
}
