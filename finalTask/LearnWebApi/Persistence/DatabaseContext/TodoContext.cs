using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Persistence.DatabaseContext
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todo { get; set; }
        public DbSet<TodoDetail> TodoDetail { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors();
        }
    }
}
