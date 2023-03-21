namespace GameLibraryV2.Dto.create
{
    public class DeveloperCreateDto
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string PicturePath { get; set; } = "Def";

        public string MiniPicturePath { get; set; } = "Def";
    }
}
