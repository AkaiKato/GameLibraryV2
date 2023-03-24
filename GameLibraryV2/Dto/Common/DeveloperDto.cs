namespace GameLibraryV2.Dto.Common
{
    public class DeveloperDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string PicturePath { get; set; } = "Def.jpg";

        public string MiniPicturePath { get; set; } = "Def.jpg";
    }
}
