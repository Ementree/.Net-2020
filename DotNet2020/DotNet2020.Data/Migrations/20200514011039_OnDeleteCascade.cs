using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class OnDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCalendar_Employee_EmployeeId",
                table: "EmployeeCalendar");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCalendar_EmployeeId",
                table: "EmployeeCalendar");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCalendar_EmployeeId",
                table: "EmployeeCalendar",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCalendar_Employee_EmployeeId",
                table: "EmployeeCalendar",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCalendar_Employee_EmployeeId",
                table: "EmployeeCalendar");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCalendar_EmployeeId",
                table: "EmployeeCalendar");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCalendar_EmployeeId",
                table: "EmployeeCalendar",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCalendar_Employee_EmployeeId",
                table: "EmployeeCalendar",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
