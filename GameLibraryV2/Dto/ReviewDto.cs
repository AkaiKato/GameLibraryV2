using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public UserSmallDto User { get; set; } = null!;

        public int Rating { get; set; }

        public string Text { get; set; } = null!;

        public DateTime PublishDate { get; set; }

        public int ReviewRating { get; set; }
    }
}
