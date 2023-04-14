using System.ComponentModel.DataAnnotations;

namespace GameLibraryV2.Dto.Common
{
    public class UserLogin
    {
        [StringLength(50, MinimumLength = 2)]
        public string Nickname { get; set; } = null!;

        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
