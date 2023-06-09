﻿using System.ComponentModel.DataAnnotations;

namespace GameLibraryV2.Dto.registry
{
    public class UserCreateDto
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        [StringLength(50, MinimumLength = 2)]
        public string Nickname { get; set; } = null!;

        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Range(14, 100)]
        public int Age { get; set; }

        public string Gender { get; set; } = null!;
    }
}
