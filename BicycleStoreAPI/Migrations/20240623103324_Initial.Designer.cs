﻿// <auto-generated />
using System;
using BicycleStoreAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BicycleStoreAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240623103324_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("BicycleStoreAPI.Models.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupSet")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsReserved")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("SupplierID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SupplierID");

                    b.ToTable("Bikes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Road Bike",
                            GroupSet = "Shimano Ultegra",
                            IsReserved = false,
                            Model = "Speedster 3000",
                            Price = 2599.99m,
                            SupplierID = 1
                        },
                        new
                        {
                            Id = 2,
                            Category = "Mountain Bike",
                            GroupSet = "SRAM Eagle",
                            IsReserved = true,
                            Model = "Spectral",
                            Price = 3299.00m,
                            SupplierID = 2
                        },
                        new
                        {
                            Id = 3,
                            Category = "Urban Bike",
                            GroupSet = "Shimano Nexus",
                            IsReserved = false,
                            Model = "Mistral",
                            Price = 899.50m,
                            SupplierID = 3
                        },
                        new
                        {
                            Id = 4,
                            Category = "E-Bike",
                            GroupSet = "Shiamno XT",
                            IsReserved = false,
                            Model = "Urta Hybrid",
                            Price = 6700.50m,
                            SupplierID = 4
                        },
                        new
                        {
                            Id = 5,
                            Category = "gravel Bike",
                            GroupSet = "Shimano GRX-400",
                            IsReserved = false,
                            Model = "Topstone",
                            Price = 3599.99m,
                            SupplierID = 5
                        },
                        new
                        {
                            Id = 6,
                            Category = "Road Bike",
                            GroupSet = "Shimano 105",
                            IsReserved = false,
                            Model = "Endurace",
                            Price = 2000.00m,
                            SupplierID = 2
                        },
                        new
                        {
                            Id = 7,
                            Category = "Mountain Bike",
                            GroupSet = "SRAM NX",
                            IsReserved = false,
                            Model = "Fuel EX",
                            Price = 3100.00m,
                            SupplierID = 1
                        },
                        new
                        {
                            Id = 8,
                            Category = "Mountain Bike",
                            GroupSet = "Shimano Deore",
                            IsReserved = false,
                            Model = "Rambler R9.2",
                            Price = 1200.00m,
                            SupplierID = 3
                        },
                        new
                        {
                            Id = 9,
                            Category = "E-Bike",
                            GroupSet = "Shimano Deore",
                            IsReserved = false,
                            Model = "Trail Neo",
                            Price = 3400.00m,
                            SupplierID = 5
                        },
                        new
                        {
                            Id = 10,
                            Category = "Road Bike",
                            GroupSet = "Shimano Dura-Ace",
                            IsReserved = false,
                            Model = "SuperSix EVO",
                            Price = 6000.00m,
                            SupplierID = 5
                        },
                        new
                        {
                            Id = 11,
                            Category = "Urban Bike",
                            GroupSet = "Shimano Altus",
                            IsReserved = false,
                            Model = "Quick CX 3",
                            Price = 800.00m,
                            SupplierID = 5
                        },
                        new
                        {
                            Id = 12,
                            Category = "Urban Bike",
                            GroupSet = "Shimano Nexus",
                            IsReserved = false,
                            Model = "Vintage",
                            Price = 450.00m,
                            SupplierID = 3
                        },
                        new
                        {
                            Id = 13,
                            Category = "Road Bike",
                            GroupSet = "Shimano Ultegra Di2",
                            IsReserved = false,
                            Model = "Filante SLR",
                            Price = 9500.00m,
                            SupplierID = 4
                        },
                        new
                        {
                            Id = 14,
                            Category = "Mountain Bike",
                            GroupSet = "SRAM GX",
                            IsReserved = true,
                            Model = "Mach 6",
                            Price = 5000.00m,
                            SupplierID = 4
                        },
                        new
                        {
                            Id = 15,
                            Category = "Urban Bike",
                            GroupSet = "Shimano Deore XT",
                            IsReserved = false,
                            Model = "Roadlite",
                            Price = 950.00m,
                            SupplierID = 2
                        },
                        new
                        {
                            Id = 16,
                            Category = "E-Bike",
                            GroupSet = "Shimano 105",
                            IsReserved = false,
                            Model = "Domane+ AL 5",
                            Price = 2800.00m,
                            SupplierID = 1
                        },
                        new
                        {
                            Id = 17,
                            Category = "Road Bike",
                            GroupSet = "Campagnolo chorus",
                            IsReserved = false,
                            Model = "Garda",
                            Price = 650.00m,
                            SupplierID = 4
                        },
                        new
                        {
                            Id = 18,
                            Category = "Mountain Bike",
                            GroupSet = "SRAM X01",
                            IsReserved = true,
                            Model = "Top Fuel 9.8",
                            Price = 6500.00m,
                            SupplierID = 1
                        },
                        new
                        {
                            Id = 19,
                            Category = "Road Bike",
                            GroupSet = "Shimano Tiagra",
                            IsReserved = false,
                            Model = "CAAD13",
                            Price = 2100.00m,
                            SupplierID = 5
                        },
                        new
                        {
                            Id = 20,
                            Category = "Gravel Bike",
                            GroupSet = "Shimano GRX-400",
                            IsReserved = false,
                            Model = "Aspre 2",
                            Price = 1500.00m,
                            SupplierID = 3
                        });
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BikeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OrderId");

                    b.HasIndex("BikeId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Trek"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Canyon"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Romet"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Willier"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Cannondale"
                        });
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Bike", b =>
                {
                    b.HasOne("BicycleStoreAPI.Models.Supplier", "Supplier")
                        .WithMany("Bikes")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Order", b =>
                {
                    b.HasOne("BicycleStoreAPI.Models.Bike", "Bike")
                        .WithMany()
                        .HasForeignKey("BikeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BicycleStoreAPI.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bike");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("BicycleStoreAPI.Models.Supplier", b =>
                {
                    b.Navigation("Bikes");
                });
#pragma warning restore 612, 618
        }
    }
}
