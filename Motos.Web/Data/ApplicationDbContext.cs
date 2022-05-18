using Microsoft.EntityFrameworkCore;
using Motos.Web.Models;

namespace Motos.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Registry> Registries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Registry>()
                .HasIndex(t => t.Placa)
                .IsUnique();
        }

    }
}

