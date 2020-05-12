using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class ChangeEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Employee");
        }
    }
}
