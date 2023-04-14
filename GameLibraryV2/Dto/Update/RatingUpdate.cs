using System.ComponentModel.DataAnnotations;

namespace GameLibraryV2.Dto.Update
{
    public class RatingUpdate
    {
        public int RatingId { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }
    }
}
