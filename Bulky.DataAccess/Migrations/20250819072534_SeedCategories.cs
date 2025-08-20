using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "DisplayOrder", "IsHidden", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, "Fiction", null },
                    { 2, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, "Science Fiction", null },
                    { 3, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, "Mystery", null },
                    { 4, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, "Romance", null },
                    { 5, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, "Biography", null },
                    { 6, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, "Children's Books", null },
                    { 7, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, "History", null },
                    { 8, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, "Fantasy", null },
                    { 9, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 9, true, "Horror", new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
