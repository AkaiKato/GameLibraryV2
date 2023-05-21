namespace GameLibraryV2.Dto.create
{
    public class GameCreateDto
    {
        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public int AgeRating { get; set; }

        public bool NSFW { get; set; }

        public string Type { get; set; } = null!;

        public double? AveragePlayTime { get; set; }

        public IList<SystemRequirementsCreateDto> SystemRequirements { get; set; } = null!;

        public virtual IList<int> Developers { get; set; } = null!;

        public virtual IList<int> Publishers { get; set; } = null!;

        public virtual IList<int> Platforms { get; set; } = null!;

        public virtual IList<int> Genres { get; set; } = null!;

        public virtual IList<int> Tags { get; set; } = null!;

    }
}
