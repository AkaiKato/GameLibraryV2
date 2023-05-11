using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Models.Common;

namespace GameLibraryV2.Dto.Update
{
    public class GameUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public JustIdDto AgeRating { get; set; } = null!;

        public bool NSFW { get; set; }

        public float? AveragePlayTime { get; set; }

        public virtual IList<JustIdDto> Developers { get; set; } = null!;

        public virtual IList<JustIdDto> Publishers { get; set; } = null!;

        public virtual IList<JustIdDto> Platforms { get; set; } = null!;

        public virtual IList<JustIdDto> Genres { get; set; } = null!;

        public virtual IList<JustIdDto> Tags { get; set; } = null!;
    }
}
