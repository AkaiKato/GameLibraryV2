namespace GameLibraryV2.Models
{
    public class PersonGame
    {
        public int Id { get; set; }

        public Library Library { get; set; } = null!;

        public Game Game { get; set; } = null!;

        public int Score { get; set; } = -1;

        public string? Comment { get; set; }

        public string List { get; set; } = null!;

        public string? PlayedPlatform { get; set; }

        public bool Favourite { get; set; }
    }
}
