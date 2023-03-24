namespace GameLibraryV2.Dto.create
{
    public class ReviewCreateDto
    {
        public int Rating { get; set; }

        public string Text { get; set; } = null!;

        public DateTime PublishDate { get; set; }

        public int ReviewRating { get; set; }
    }
}
