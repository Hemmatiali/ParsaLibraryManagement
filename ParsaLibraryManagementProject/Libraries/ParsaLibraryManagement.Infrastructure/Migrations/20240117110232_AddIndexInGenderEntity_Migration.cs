using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsaLibraryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexInGenderEntity_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
