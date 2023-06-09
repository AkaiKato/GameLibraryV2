﻿namespace GameLibraryV2.Models.Common
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int Age { get; set; }

        public string Gender { get; set; } = null!;

        public string PicturePath { get; set; } = null!;

        public DateOnly RegistrationdDate { get; set; }

        public virtual IList<PersonGame>? UserGames { get; set; }

        public virtual IList<Role> UserRoles { get; set; } = null!;

        public virtual IList<Friend>? UserFriends { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
