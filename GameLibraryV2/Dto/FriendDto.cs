using GameLibraryV2.Dto.smallInfo;

namespace GameLibraryV2.Dto
{
    public class FriendDto
    {
        public int Id { get; set; }

        public UserSmallDto Friendu { get; set; } = null!;
    }
}
