namespace GameLibraryV2.Dto
{
    public class PublisherDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string PicturePath { get; set; } = "Def";

        public string MiniPicturePath { get; set; } = "Def";
    }
}
