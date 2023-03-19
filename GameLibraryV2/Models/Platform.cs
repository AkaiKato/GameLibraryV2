namespace GameLibraryV2.Models
{
    public class Platform
    {
        public int Id { get; set; }
   
        public string Name { get; set; } = null!;
    
        public string? Description { get; set; }
    
        public virtual IList<Game>? PlatformGames { get; set; }
    }
}
