using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class update_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "teachers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "students",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "teachers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "students",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
