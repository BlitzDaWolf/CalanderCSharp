using CalanderCSharp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalanderCSharp.Context
{
    public class CalanderContext : DbContext
    {
        public CalanderContext()
        {

        }

        public CalanderContext(DbContextOptions<CalanderContext> a) : base(a)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CalanderEvent> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
