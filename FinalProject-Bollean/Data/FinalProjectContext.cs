using FinalProject_Bollean.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_Bollean.Data
{
    public class FinalProjectContext : DbContext
    {
        public FinalProjectContext(DbContextOptions<FinalProjectContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User-Post relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // Post-Comment relationship
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        }
    }
}
