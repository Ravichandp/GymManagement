using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymManagement.API.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class DataAddedtoCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Age", "City", "Email", "FirstName", "Gender", "LastName", "MiddleName", "Password", "UserName" },
                values: new object[,]
                {
                    { new Guid("4cdf5391-df19-445f-ae2d-0b1474acf277"), 32, "Hyderabad", "ravi@gmail.com", "Ravichand", "Male", "Prathipati", "Balaswami", "Ravi@123", "Ravichand" },
                    { new Guid("cf1efe89-f48d-4200-b0ec-df4a666a85bf"), 32, "Hyderabad", "sowji@gmail.com", "Sowjanya", "FeMale", "Kambhampati", "Ravichand", "Sowji@123", "Sowji" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: new Guid("4cdf5391-df19-445f-ae2d-0b1474acf277"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: new Guid("cf1efe89-f48d-4200-b0ec-df4a666a85bf"));
        }
    }
}
