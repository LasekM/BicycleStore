using BicycleStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BicycleStoreAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Bike.db");
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Dane //

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
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Bike)
                .WithMany()
                .HasForeignKey(o => o.BikeId);

            // Przykładowe dane
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Bike Suppliers Inc." },
                new Supplier { Id = 2, Name = "Premium Bike Parts" }
            );

            modelBuilder.Entity<Bike>().HasData(
               new Bike { Id = 1, Model = "Mountain King", Price = 999.99m, SupplierID = 1, IsReserved = false },
               new Bike { Id = 2, Model = "Road Pro", Price = 1299.99m, SupplierID = 2, IsReserved = false }
           );

            // modelBuilder.Entity<Customer>().HasData(
            //     new Customer { CustomerId = 1, LastName = "Smith" },
            //     new Customer { CustomerId = 2, LastName = "Johnson" }
            // );

            // modelBuilder.Entity<Order>().HasData(
            //     new Order { OrderId = 1, BikeId = 1, CustomerId = 1, OrderDate = DateTime.Now },
            //     new Order { OrderId = 2, BikeId = 2, CustomerId = 2, OrderDate = DateTime.Now }
            // );
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
