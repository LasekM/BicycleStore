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
                new Supplier { Id = 1, Name = "Trek" },
                new Supplier { Id = 2, Name = "Canyon" },
                new Supplier { Id = 3, Name = "Romet" },
                new Supplier { Id = 4, Name = "Willier" },
                new Supplier { Id = 5, Name = "Cannondale" }
            );

            modelBuilder.Entity<Bike>().HasData(
                new Bike
                {
                    Id = 1,
                    Model = "Speedster 3000",
                    Category = "Road Bike",
                    GroupSet = "Shimano Ultegra",
                    Price = 2599.99m,
                    SupplierID = 1,
                    IsReserved = true
                },
                new Bike
                {
                    Id = 2,
                    Model = "Spectral",
                    Category = "Mountain Bike",
                    GroupSet = "SRAM Eagle",
                    Price = 3299.00m,
                    SupplierID = 2,
                    IsReserved = true
                },
                new Bike
                {
                    Id = 3,
                    Model = "Mistral",
                    Category = "Urban Bike",
                    GroupSet = "Shimano Nexus",
                    Price = 899.50m,
                    SupplierID = 3,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 4,
                    Model = "Urta Hybrid",
                    Category = "E-Bike",
                    GroupSet = "Shiamno XT",
                    Price = 6700.50m,
                    SupplierID = 4,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 5,
                    Model = "Topstone",
                    Category = "gravel Bike",
                    GroupSet = "Shimano GRX-400",
                    Price = 3599.99m,
                    SupplierID = 5,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 6,
                    Model = "Endurace",
                    Category = "Road Bike",
                    GroupSet = "Shimano 105",
                    Price = 2000.00m,
                    SupplierID = 2,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 7,
                    Model = "Fuel EX",
                    Category = "Mountain Bike",
                    GroupSet = "SRAM NX",
                    Price = 3100.00m,
                    SupplierID = 1,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 8,
                    Model = "Rambler R9.2",
                    Category = "Mountain Bike",
                    GroupSet = "Shimano Deore",
                    Price = 1200.00m,
                    SupplierID = 3,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 9,
                    Model = "Trail Neo",
                    Category = "E-Bike",
                    GroupSet = "Shimano Deore",
                    Price = 3400.00m,
                    SupplierID = 5,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 10,
                    Model = "SuperSix EVO",
                    Category = "Road Bike",
                    GroupSet = "Shimano Dura-Ace",
                    Price = 6000.00m,
                    SupplierID = 5,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 11,
                    Model = "Quick CX 3",
                    Category = "Urban Bike",
                    GroupSet = "Shimano Altus",
                    Price = 800.00m,
                    SupplierID = 5,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 12,
                    Model = "Vintage",
                    Category = "Urban Bike",
                    GroupSet = "Shimano Nexus",
                    Price = 450.00m,
                    SupplierID = 3,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 13,
                    Model = "Filante SLR",
                    Category = "Road Bike",
                    GroupSet = "Shimano Ultegra Di2",
                    Price = 9500.00m,
                    SupplierID = 4,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 14,
                    Model = "Mach 6",
                    Category = "Mountain Bike",
                    GroupSet = "SRAM GX",
                    Price = 5000.00m,
                    SupplierID = 4,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 15,
                    Model = "Roadlite",
                    Category = "Urban Bike",
                    GroupSet = "Shimano Deore XT",
                    Price = 950.00m,
                    SupplierID = 2,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 16,
                    Model = "Domane+ AL 5",
                    Category = "E-Bike",
                    GroupSet = "Shimano 105",
                    Price = 2800.00m,
                    SupplierID = 1,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 17,
                    Model = "Garda",
                    Category = "Road Bike",
                    GroupSet = "Campagnolo chorus",
                    Price = 650.00m,
                    SupplierID = 4,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 18,
                    Model = "Top Fuel 9.8",
                    Category = "Mountain Bike",
                    GroupSet = "SRAM X01",
                    Price = 6500.00m,
                    SupplierID = 1,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 19,
                    Model = "CAAD13",
                    Category = "Road Bike",
                    GroupSet = "Shimano Tiagra",
                    Price = 2100.00m,
                    SupplierID = 5,
                    IsReserved = false
                },
                new Bike
                {
                    Id = 20,
                    Model = "Aspre 2",
                    Category = "Gravel Bike",
                    GroupSet = "Shimano GRX-400",
                    Price = 1500.00m,
                    SupplierID = 3,
                    IsReserved = false
                }
           );

            modelBuilder.Entity<Customer>().HasData(
                            new Customer { CustomerId = 1, LastName = "admin" },
                            new Customer { CustomerId = 2, LastName = "user" }
                        );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, BikeId = 1, CustomerId = 1, OrderDate = DateTime.Now, UserName = "admin" },
                new Order { OrderId = 2, BikeId = 2, CustomerId = 2, OrderDate = DateTime.Now, UserName = "user" }
            );
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
