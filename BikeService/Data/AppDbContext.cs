using Microsoft.EntityFrameworkCore;
using BikeService.Models;

namespace BikeService.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BikeService.db");
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacje
            modelBuilder.Entity<Supplier>()
                .HasMany(supplier => supplier.Bikes)
                .WithOne(bike => bike.Supplier)
                .HasForeignKey(bike => bike.SupplierID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>()
              .HasOne(o => o.Customer)
              .WithMany(c => c.Orders)
              .HasForeignKey(o => o.CustomerId)
              .OnDelete(DeleteBehavior.Cascade);

            // Order-Bike Relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Bike)
                .WithMany()
                .HasForeignKey(o => o.BikeId)
                .OnDelete(DeleteBehavior.Restrict);


            // Przykładowe dane
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Bike Suppliers Inc." },
                new Supplier { Id = 2, Name = "Premium Bike Parts" }
            );

            modelBuilder.Entity<Bike>().HasData(
                new Bike { Id = 1, Model = "Mountain King", Price = 999.99m, SupplierID = 1 },
                new Bike { Id = 2, Model = "Road Pro", Price = 1299.99m, SupplierID = 2 }
            );
        }
    }
}
