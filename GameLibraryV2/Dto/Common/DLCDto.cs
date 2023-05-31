using GameLibraryV2.Dto.smallInfo;

namespace GameLibraryV2.Dto.Common
{
    public class DLCDto
    {
        public int Id { get; set; }
        public GameSmallDto DLCGame { get; set; } = null!;
    }
}
