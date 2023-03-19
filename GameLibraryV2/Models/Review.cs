namespace GameLibraryV2.Models
{
    public class Review
    {
        public int Id { get; set; }

        public Game Game { get; set; } = null!;

        public User User { get; set; } = null!;

        public int Rating { get; set; }

        public string Text { get; set; } = null!;

        public DateTime PublishDate { get; set; }

        public int ReviewRating { get; set; }
    }
}
