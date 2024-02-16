using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParsaLibraryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBorrowedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "BorrowedBooks");

         

            migrationBuilder.AddColumn<DateTime>(
                name: "BackEndDate",
                table: "BorrowedBooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateBorrowed",
                table: "BorrowedBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackEndDate",
                table: "BorrowedBooks");

            migrationBuilder.DropColumn(
                name: "StartDateBorrowed",
                table: "BorrowedBooks");

           
            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "BorrowedBooks",
                type: "bit",
                nullable: false,
                defaultValueSql: "((0))");
        }
    }
}
