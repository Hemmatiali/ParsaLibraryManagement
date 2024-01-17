using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsaLibraryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectNavigationProperties3_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksCategories_BooksCategories_RefCategoryId",
                table: "BooksCategories");

            migrationBuilder.DropIndex(
                name: "IX_BooksCategories_RefCategoryId",
                table: "BooksCategories");

            migrationBuilder.DropColumn(
                name: "RefCategoryId",
                table: "BooksCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "RefCategoryId",
                table: "BooksCategories",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksCategories_RefCategoryId",
                table: "BooksCategories",
                column: "RefCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksCategories_BooksCategories_RefCategoryId",
                table: "BooksCategories",
                column: "RefCategoryId",
                principalTable: "BooksCategories",
                principalColumn: "CategoryId");
        }
    }
}
