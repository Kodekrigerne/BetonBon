using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure
{
    public class BetonBonDbContext(DbContextOptions<BetonBonDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
