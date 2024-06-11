using BicycleStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BicycleStore.DbContext
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BikeShop.db");
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Przykładowe dane
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, Name = "Bike Suppliers Inc.", ContactEmail = "contact@bikesuppliers.com" },
                new Supplier { SupplierId = 2, Name = "Premium Bike Parts", ContactEmail = "info@premiumbikeparts.com" }
            );

            modelBuilder.Entity<Bike>().HasData(
                new Bike { BikeId = 1, Model = "Mountain King", Brand = "TrailBlazer", Price = 999.99m, SupplierId = 1 },
                new Bike { BikeId = 2, Model = "Road Pro", Brand = "Speedster", Price = 1299.99m, SupplierId = 2 }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, BikeId = 1, CustomerId = 1, OrderDate = new DateTime(2023, 1, 1) },
                new Order { OrderId = 2, BikeId = 2, CustomerId = 2, OrderDate = new DateTime(2023, 2, 15) }
            );

            modelBuilder.Entity<Bike>()
                .HasMany(bike => bike.Orders)
                .WithOne(order => order.Bike)
                .HasForeignKey(order => order.BikeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.Orders)
                .WithOne(order => order.Customer)
                .HasForeignKey(order => order.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Użytkownicy i role
            string ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_ID = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "admin",
                NormalizedName = "ADMIN",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            var admin = new IdentityUser
            {
                Id = ADMIN_ID,
                Email = "adam@wsei.edu.pl",
                EmailConfirmed = true,
                UserName = "adam@wsei.edu.pl",
                NormalizedUserName = "ADAM@WSEI.EDU.PL",
                NormalizedEmail = "ADAM@WSEI.EDU.PL"
            };

            PasswordHasher<IdentityUser> ph = new PasswordHasher<IdentityUser>();
            admin.PasswordHash = ph.HashPassword(admin, "1234abcd!@#$ABCD");

            modelBuilder.Entity<IdentityUser>().HasData(admin);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            string USER_ROLE_ID = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "user",
                NormalizedName = "USER",
                Id = USER_ROLE_ID,
                ConcurrencyStamp = USER_ROLE_ID
            });

            string USER_ID = Guid.NewGuid().ToString();

            var user = new IdentityUser
            {
                Id = USER_ID,
                Email = "user@wsei.edu.pl",
                EmailConfirmed = true,
                UserName = "user@wsei.edu.pl",
                NormalizedUserName = "USER@WSEI.EDU.PL",
                NormalizedEmail = "USER@WSEI.EDU.PL"
            };

            user.PasswordHash = ph.HashPassword(user, "userPassword123");

            modelBuilder.Entity<IdentityUser>().HasData(user);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = USER_ROLE_ID,
                UserId = USER_ID
            });
        }


    }
}
