using GameLibraryV2.Dto.smallInfo;

namespace GameLibraryV2.Dto.Common
{
    public class PersonGameDto
    {
        public Guid Id { get; set; }

        public GameSmallListDto Game { get; set; } = null!;

        public int Score { get; set; } = -1;

        public string? Comment { get; set; }

        public string List { get; set; } = null!;

        public PlatformDto? PlayedPlatform { get; set; }

        public bool Favourite { get; set; }
    }
}
