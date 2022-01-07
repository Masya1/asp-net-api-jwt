using Microsoft.EntityFrameworkCore;

namespace Auth.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e => e.HasIndex(u => u.Email).IsUnique());
        }
    }
}