namespace GameLibraryV2.Dto.Create
{
    public class PersonGameCreate
    {
        public int UserId { get; set; }

        public int GameId { get; set; }

        public string List { get; set; } = null!;
    }
}
