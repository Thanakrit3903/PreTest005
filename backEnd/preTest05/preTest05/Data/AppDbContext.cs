using Microsoft.EntityFrameworkCore;
using preTest05.Model;

namespace preTest05.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Queues> queues => Set<Queues>();
    }
}
