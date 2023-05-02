namespace GameLibraryV2.Models.Common
{
    public class PersonGame
    {
        public Guid Id { get; set; }

        public User User { get; set; } = null!;

        public Game Game { get; set; } = null!;

        public int Score { get; set; } = -1;

        public string? Comment { get; set; }

        public string List { get; set; } = null!;

        public Platform? PlayedPlatform { get; set; }

        public bool Favourite { get; set; }
    }
}
