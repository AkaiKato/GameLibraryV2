using GameLibraryV2.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryV2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        public DbSet<AgeRating> AgeRating { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<DLC> DLCs { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<PersonGame> PersonGames { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<SystemRequirements> SystemRequirements { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.HasCollation("Latin1_General_CS_AS_KS_WS");
            modelBuilder.Entity<Game>().HasMany(g => g.DLCs).WithOne(pg => pg.ParentGame).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>().HasMany(u => u.UserFriends).WithOne(g => g.User).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
