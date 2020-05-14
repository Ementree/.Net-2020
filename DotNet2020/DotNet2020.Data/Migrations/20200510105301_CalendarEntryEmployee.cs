using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class CalendarEntryEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractCalendarEntries_AspNetUsers_UserId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropIndex(
                name: "IX_AbstractCalendarEntries_UserId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AbstractCalendarEntries");

            migrationBuilder.AddColumn<int>(
                name: "CalendarEmployeeId",
                table: "AbstractCalendarEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AbstractCalendarEntries_CalendarEmployeeId",
                table: "AbstractCalendarEntries",
                column: "CalendarEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractCalendarEntries_EmployeeCalendar_CalendarEmployeeId",
                table: "AbstractCalendarEntries",
                column: "CalendarEmployeeId",
                principalTable: "EmployeeCalendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractCalendarEntries_EmployeeCalendar_CalendarEmployeeId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropIndex(
                name: "IX_AbstractCalendarEntries_CalendarEmployeeId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropColumn(
                name: "CalendarEmployeeId",
                table: "AbstractCalendarEntries");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AbstractCalendarEntries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbstractCalendarEntries_UserId",
                table: "AbstractCalendarEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractCalendarEntries_AspNetUsers_UserId",
                table: "AbstractCalendarEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
