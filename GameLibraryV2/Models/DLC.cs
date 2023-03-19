namespace GameLibraryV2.Models
{
    public class DLC
    {
        public int Id { get; set; }

        public Game ParentGame { get; set; } = null!;

        public Game DLCGame { get; set; } = null!;
    }
}
