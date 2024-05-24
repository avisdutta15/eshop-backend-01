using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eshop.Catalog.Data.Migrations.CatalogMigrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "domain",
                table: "catalog_types",
                columns: new[] { "id", "type" },
                values: new object[,]
                {
                    { 1, "T-Shirt" },
                    { 2, "Pant" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "domain",
                table: "catalog_types",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "domain",
                table: "catalog_types",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
