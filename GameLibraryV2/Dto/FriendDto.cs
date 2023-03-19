using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Models;

namespace GameLibraryV2.Dto
{
    public class FriendDto
    {
        public int Id { get; set; }

        public UserSmallDto Friendu { get; set; } = null!;
    }
}
