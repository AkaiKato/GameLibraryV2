using GameLibraryV2.Dto.Common;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto.create
{
    public class GameCreateDto
    {
        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public AgeRating AgeRating { get; set; } = null!;

        public bool NSFW { get; set; }

        public string Type { get; set; } = null!;

        public double? AveragePlayTime { get; set; }

        public SystemRequirementsMinCreateDto SystemRequirementsMin { get; set; } = null!;

        public SystemRequirementsMaxCreateDto SystemRequirementsMax { get; set; } = null!;

        public virtual IList<JustIdDto> Developers { get; set; } = null!;

        public virtual IList<JustIdDto> Publishers { get; set; } = null!;

        public virtual IList<JustIdDto> Platforms { get; set; } = null!;

        public virtual IList<JustIdDto> Genres { get; set; } = null!;

        public virtual IList<JustIdDto> Tags { get; set; } = null!;

    }
}
