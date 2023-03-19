namespace GameLibraryV2.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string PicturePath { get; set; } = null!;

        public string MiniPicturePath { get; set; } = null!;

        public virtual IList<Game>? PublisherGames { get; set; }
    }
}
