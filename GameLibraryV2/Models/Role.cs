namespace GameLibraryV2.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; } = null!;

        public virtual IList<User>? RoleUsers { get; set; }
    }
}
