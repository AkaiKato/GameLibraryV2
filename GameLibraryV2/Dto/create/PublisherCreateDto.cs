namespace GameLibraryV2.Dto.create
{
    public class PublisherCreateDto
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string PicturePath { get; set; } = "Def.jpg";

        public string MiniPicturePath { get; set; } = "Def.jpg";
    }
}
