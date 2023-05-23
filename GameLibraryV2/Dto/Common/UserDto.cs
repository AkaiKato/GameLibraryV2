using GameLibraryV2.Models;

namespace GameLibraryV2.Dto.Common
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        public int Age { get; set; }

        public string Gender { get; set; } = null!;

        public string PicturePath { get; set; } = "Def.jpg";

        public DateOnly RegistrationdDate { get; set; }

        public virtual IList<RoleDto> UserRoles { get; set; } = null!;
    }
}
