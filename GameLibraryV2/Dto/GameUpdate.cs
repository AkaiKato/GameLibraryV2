using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto
{
    public class GameUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public string? AgeRating { get; set; }

        public bool NSFW { get; set; }

        public string? AveragePlayTime { get; set; }

        public SystemRequirementsMin SystemRequirementsMin { get; set; } = null!;

        public SystemRequirementsMax SystemRequirementsMax { get; set; } = null!;

        public IList<GameSmallDto>? DLCs { get; set; }

        public virtual IList<DeveloperSmallDto> Developers { get; set; } = null!;

        public virtual IList<PublisherSmallDto> Publishers { get; set; } = null!;

        public virtual IList<PlatformSmallDto> Platforms { get; set; } = null!;

        public virtual IList<GenreSmallDto> Genres { get; set; } = null!;

        public virtual IList<TagSmallDto> Tags { get; set; } = null!;
    }
}
