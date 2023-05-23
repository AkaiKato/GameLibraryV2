using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Dto.smallInfo
{
    public class GameSmallListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string PicturePath { get; set; } = "Def.jpg";

        public string Status { get; set; } = null!;

        public DateOnly ReleaseDate { get; set; }

        public string? Description { get; set; }

        public bool NSFW { get; set; }

        public string Type { get; set; } = null!;

        public Rating Rating { get; set; } = null!;

        public virtual IList<DeveloperSmallDto> Developers { get; set; } = null!;

        public virtual IList<PublisherSmallDto> Publishers { get; set; } = null!;

        public virtual IList<PlatformSmallDto> Platforms { get; set; } = null!;

        public virtual IList<GenreSmallDto> Genres { get; set; } = null!;

        public virtual IList<TagSmallDto> Tags { get; set; } = null!;
    }
}
