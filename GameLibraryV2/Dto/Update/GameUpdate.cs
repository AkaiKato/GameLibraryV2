using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Dto.smallInfo;

namespace GameLibraryV2.Dto.Update
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

        public SystemRequirementsMinCreateDto SystemRequirementsMin { get; set; } = null!;

        public SystemRequirementsMaxCreateDto SystemRequirementsMax { get; set; } = null!;

        public virtual IList<JustIdDto> Developers { get; set; } = null!;

        public virtual IList<JustIdDto> Publishers { get; set; } = null!;

        public virtual IList<JustIdDto> Platforms { get; set; } = null!;

        public virtual IList<JustIdDto> Genres { get; set; } = null!;

        public virtual IList<JustIdDto> Tags { get; set; } = null!;
    }
}
