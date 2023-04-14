namespace GameLibraryV2.Dto.create
{
    public class ReviewCreateDto
    {
        public int UserId { get; set; }

        public int GameId { get; set; }

        public string Text { get; set; } = null!;

        public DateTime PublishDate { get; set; }
    }
}
