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
            //Dane//

            base.OnModelCreating(modelBuilder);




            //Relacje
            modelBuilder.Entity<Supplier>()
               .HasMany(supplier => supplier.Bikes)
               .WithOne(bike => bike.Supplier)
               .HasForeignKey(bike => bike.SupplierID)
               .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Order>()
           .HasOne(o => o.Customer)
           .WithMany(c => c.Orders)
           .HasForeignKey(o => o.CustomerId);


            // Przykładowe dane
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Bike Suppliers Inc." },
                new Supplier { Id = 2, Name = "Premium Bike Parts" }
            );

            modelBuilder.Entity<Bike>().HasData(
                new Bike { Id = 1, Model = "Mountain King", Price = 999.99m, SupplierID = 1 },
                new Bike { Id = 2, Model = "Road Pro", Price = 1299.99m, SupplierID = 2 }
            );


            modelBuilder.Entity<Customer>().HasData(
               new Customer { CustomerId = 1, LastName = "Smith" },
               new Customer { CustomerId = 2, LastName = "Johnson" }
           );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, BikeId = 1, CustomerId = 1, OrderDate = DateTime.Now },
                new Order { OrderId = 2, BikeId = 2, CustomerId = 2, OrderDate = DateTime.Now }
            );






            //Uzytkownicy//

            string ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_ID = Guid.NewGuid().ToString();

            // dodanie roli administratora
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "admin",
                NormalizedName = "ADMIN",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            // utworzenie administratora jako użytkownika
            var admin = new IdentityUser
            {
                Id = ADMIN_ID,
                Email = "adam@wsei.edu.pl",
                EmailConfirmed = true,
                UserName = "adam@wsei.edu.pl",
                NormalizedUserName = "ADAM@WSEI.EDU.PL",
                NormalizedEmail = "ADAM@WSEI.EDU.PL"
            };

            // haszowanie hasła
            PasswordHasher<IdentityUser> ph = new PasswordHasher<IdentityUser>();
            admin.PasswordHash = ph.HashPassword(admin, "1234abcd!@#$ABCD");

            // zapisanie użytkownika
            modelBuilder.Entity<IdentityUser>().HasData(admin);

            // przypisanie roli administratora użytkownikowi
            modelBuilder.Entity<IdentityUserRole<string>>()
            .HasData(new IdentityUserRole<string>
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
                UserName = "user",
                NormalizedUserName = "USER",
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
