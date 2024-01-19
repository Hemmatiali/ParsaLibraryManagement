using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParsaLibraryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseGenderTitleLengthAndGenderSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Genders",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

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

            migrationBuilder.CreateIndex(
                name: "IX_Genders_Code",
                table: "Genders",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Genders_Code",
                table: "Genders");

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

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Genders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);
        }
    }
}
