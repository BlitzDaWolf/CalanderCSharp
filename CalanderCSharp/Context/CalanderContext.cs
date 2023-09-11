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

        public DbSet<CalanderEvent> Events { get; set; }
    }
}
