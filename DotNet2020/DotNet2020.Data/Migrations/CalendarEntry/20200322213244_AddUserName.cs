using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.CalendarEntry
{
    public partial class AddUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CalendarEntries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "CalendarEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CalendarEntries");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "CalendarEntries");
        }
    }
}
