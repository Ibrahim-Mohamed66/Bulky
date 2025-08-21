using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "CreatedAt", "DisplayOrder", "IsHidden", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Tech City", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, "Tech Solution", "6669990000", "12121", "IL", "123 Tech St", null },
                    { 2, "Vid City", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, "Vivid Books", "7779990000", "66666", "IL", "999 Vid St", null },
                    { 3, "Lala land", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, "Readers Club", "1113335555", "99999", "NY", "999 Main St", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
