namespace GameLibraryV2.Models.Common
{
    public class DLC
    {
        public int Id { get; set; }

        public Game ParentGame { get; set; } = null!;

        public Game DLCGame { get; set; } = null!;
    }
}
