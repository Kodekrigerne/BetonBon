using BetonBon.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure
{
    public class BetonBonDbContext(DbContextOptions<BetonBonDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);

                user.HasIndex(u => u.Username)
                    .IsUnique();

                user.OwnsOne(u => u.HashedPassword);

                user.Property(u => u.Id)
                    .ValueGeneratedNever();

                user.Property(u => u.Role)
                    .HasConversion<string>();
            });


        }
    }
}
