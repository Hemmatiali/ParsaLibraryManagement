using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParsaLibraryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GenderSeedData_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "GenderId", "Code", "Title" },
                values: new object[,]
                {
                    { (byte)1, "M", "Male" },
                    { (byte)2, "F", "Female" },
                    { (byte)3, "RNS", "Rather Not Say" },
                    { (byte)4, "MXD", "Mixed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: (byte)4);
        }
    }
}
