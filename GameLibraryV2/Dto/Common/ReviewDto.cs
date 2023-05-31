using GameLibraryV2.Dto.smallInfo;

namespace GameLibraryV2.Dto.Common
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public UserSmallDto User { get; set; } = null!;

        public GameSmallDto Game { get; set; } = null!;

        public int Rating { get; set; }

        public string Text { get; set; } = null!;

        public DateOnly PublishDate { get; set; }

        public int ReviewRating { get; set; }
    }
}
