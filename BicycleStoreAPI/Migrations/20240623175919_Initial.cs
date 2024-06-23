using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BicycleStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    GroupSet = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: false),
                    IsReserved = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bikes_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BikeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Bikes_BikeId",
                        column: x => x.BikeId,
                        principalTable: "Bikes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "LastName" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "user" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Trek" },
                    { 2, "Canyon" },
                    { 3, "Romet" },
                    { 4, "Willier" },
                    { 5, "Cannondale" }
                });

            migrationBuilder.InsertData(
                table: "Bikes",
                columns: new[] { "Id", "Category", "GroupSet", "IsReserved", "Model", "Price", "SupplierID" },
                values: new object[,]
                {
                    { 1, "Road Bike", "Shimano Ultegra", true, "Speedster 3000", 2599.99m, 1 },
                    { 2, "Mountain Bike", "SRAM Eagle", true, "Spectral", 3299.00m, 2 },
                    { 3, "Urban Bike", "Shimano Nexus", false, "Mistral", 899.50m, 3 },
                    { 4, "E-Bike", "Shiamno XT", false, "Urta Hybrid", 6700.50m, 4 },
                    { 5, "gravel Bike", "Shimano GRX-400", false, "Topstone", 3599.99m, 5 },
                    { 6, "Road Bike", "Shimano 105", false, "Endurace", 2000.00m, 2 },
                    { 7, "Mountain Bike", "SRAM NX", false, "Fuel EX", 3100.00m, 1 },
                    { 8, "Mountain Bike", "Shimano Deore", false, "Rambler R9.2", 1200.00m, 3 },
                    { 9, "E-Bike", "Shimano Deore", false, "Trail Neo", 3400.00m, 5 },
                    { 10, "Road Bike", "Shimano Dura-Ace", false, "SuperSix EVO", 6000.00m, 5 },
                    { 11, "Urban Bike", "Shimano Altus", false, "Quick CX 3", 800.00m, 5 },
                    { 12, "Urban Bike", "Shimano Nexus", false, "Vintage", 450.00m, 3 },
                    { 13, "Road Bike", "Shimano Ultegra Di2", false, "Filante SLR", 9500.00m, 4 },
                    { 14, "Mountain Bike", "SRAM GX", false, "Mach 6", 5000.00m, 4 },
                    { 15, "Urban Bike", "Shimano Deore XT", false, "Roadlite", 950.00m, 2 },
                    { 16, "E-Bike", "Shimano 105", false, "Domane+ AL 5", 2800.00m, 1 },
                    { 17, "Road Bike", "Campagnolo chorus", false, "Garda", 650.00m, 4 },
                    { 18, "Mountain Bike", "SRAM X01", false, "Top Fuel 9.8", 6500.00m, 1 },
                    { 19, "Road Bike", "Shimano Tiagra", false, "CAAD13", 2100.00m, 5 },
                    { 20, "Gravel Bike", "Shimano GRX-400", false, "Aspre 2", 1500.00m, 3 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "BikeId", "CustomerId", "OrderDate", "UserName" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2024, 6, 23, 19, 59, 19, 780, DateTimeKind.Local).AddTicks(8455), "admin" },
                    { 2, 2, 2, new DateTime(2024, 6, 23, 19, 59, 19, 780, DateTimeKind.Local).AddTicks(8491), "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_SupplierID",
                table: "Bikes",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BikeId",
                table: "Orders",
                column: "BikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
