namespace GameLibraryV2.Models
{
    public class Library
    {
        public int Id { get; set; }

        public virtual IList<PersonGame>? PersonGames { get; set; }
    }
}
