using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto.create
{
    public class GameCreateDto
    {

        public string Name { get; set; } = null!;

        public string PicturePath { get; set; } = "Def";

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public string? AgeRating { get; set; }

        public bool NSFW { get; set; }

        public string Type { get; set; } = null!;

        public string? AveragePlayTime { get; set; }

        public SystemRequirementsMinCreateDto SystemRequirementsMin { get; set; } = null!;

        public SystemRequirementsMaxCreateDto SystemRequirementsMax { get; set; } = null!;

    }
}
