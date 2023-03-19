using GameLibraryV2.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<DLC> DLCs { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Library> Libraries { get; set; }

        public DbSet<PersonGame> PersonGames { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<SystemRequirementsMin> SystemRequirementsMin { get; set; }

        public DbSet<SystemRequirementsMax> SystemRequirementsMax { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>().HasMany(g => g.DLCs).WithOne(pg => pg.ParentGame).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>().HasMany(u => u.UserFriends).WithOne(g => g.User).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
