using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Motos.Web.Models;

namespace Motos.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    //public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Registry> Registries { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceImage> ServicesImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(t => t.Name).IsUnique();
            modelBuilder.Entity<Position>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Registry>()
                .HasIndex(t => t.Placa)
                .IsUnique();

            modelBuilder.Entity<Service>().HasIndex(t => t.Name).IsUnique();
        }

    }
}

