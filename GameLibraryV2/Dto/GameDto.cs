﻿using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string PicturePath { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? Description { get; set; }

        public string? AgeRating { get; set; }

        public bool NSFW { get; set; }

        public string Type { get; set; } = null!;

        public string? AveragePlayTime { get; set; }

        public GameSmallDto? ParentGame { get; set; }

        public SystemRequirementsMin SystemRequirementsMin { get; set; } = null!;

        public SystemRequirementsMax SystemRequirementsMax { get; set; } = null!;

        public Rating Rating { get; set; } = null!;

        public IList<DLCDto>? DLCs { get; set; }

        public virtual IList<DeveloperDto> Developers { get; set; } = null!;

        public virtual IList<PublisherDto> Publishers { get; set; } = null!;

        public virtual IList<PlatformDto> Platforms { get; set; } = null!;

        public virtual IList<GenreDto> Genres { get; set; } = null!;

        public virtual IList<TagDto> Tags { get; set; } = null!;

    }
}
