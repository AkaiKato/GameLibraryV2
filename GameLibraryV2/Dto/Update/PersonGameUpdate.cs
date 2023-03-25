using GameLibraryV2.Dto.Common;

namespace GameLibraryV2.Dto.Update
{
    public class PersonGameUpdate
    {
        public Guid Id { get; set; }

        public int Score { get; set; } = -1;

        public string? Comment { get; set; }

        public string List { get; set; } = null!;

        public JustIdDto? PlayedPlatform { get; set; }

        public bool Favourite { get; set; }
    }
}
