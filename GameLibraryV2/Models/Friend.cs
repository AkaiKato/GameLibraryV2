using System.ComponentModel.DataAnnotations.Schema;

namespace GameLibraryV2.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public User User { get; set; } = null!;

        public User Friendu { get; set; } = null!;
    }
}
