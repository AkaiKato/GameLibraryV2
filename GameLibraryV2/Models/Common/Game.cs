namespace GameLibraryV2.Models.Common
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string PicturePath { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public AgeRating AgeRating { get; set; } = null!;

        public bool NSFW { get; set; }

        public string Type { get; set; } = null!;

        public double? AveragePlayTime { get; set; }

        public Game? ParentGame { get; set; }

        public SystemRequirementsMin SystemRequirementsMin { get; set; } = null!;

        public SystemRequirementsMax SystemRequirementsMax { get; set; } = null!;

        public Rating Rating { get; set; } = null!;

        public IList<Review>? Reviews { get; set; }

        public IList<DLC>? DLCs { get; set; }

        public virtual IList<Developer> Developers { get; set; } = null!;

        public virtual IList<Publisher> Publishers { get; set; } = null!;

        public virtual IList<Platform> Platforms { get; set; } = null!;

        public virtual IList<Genre> Genres { get; set; } = null!;

        public virtual IList<Tag> Tags { get; set; } = null!;
    }
}
