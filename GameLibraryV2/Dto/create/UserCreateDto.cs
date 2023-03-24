using System.ComponentModel.DataAnnotations;

namespace GameLibraryV2.Dto.registry
{
    public class UserCreateDto
    {
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        public string Email { get; set; } = null!;

        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        [Range(14, 100)]
        public int Age { get; set; }
        
        public string Gender { get; set; } = null!;

        public string PictureName { get; set; } = "Def.jpg";

        public DateTime RegistrationdDate { get; set; }

    }
}
