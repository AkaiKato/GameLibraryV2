namespace GameLibraryV2.Models.Common
{
    public class Platform
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual IList<Game>? PlatformGames { get; set; }
    }
}
