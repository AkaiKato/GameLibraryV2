using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto.Common
{
    public class PersonGameDto
    {
        public int Id { get; set; }

        public GameSmallListDto Game { get; set; } = null!;

        public int Score { get; set; } = -1;

        public string? Comment { get; set; }

        public string List { get; set; } = null!;

        public string? PlayedPlatform { get; set; }

        public bool Favourite { get; set; }
    }
}
