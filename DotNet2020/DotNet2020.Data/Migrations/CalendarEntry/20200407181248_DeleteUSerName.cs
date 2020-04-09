using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations.CalendarEntry
{
    public partial class DeleteUSerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEntries_AppIdentityUser_UserId",
                table: "CalendarEntries");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "CalendarEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEntries_AspNetUsers_UserId",
                table: "CalendarEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEntries_AspNetUsers_UserId",
                table: "CalendarEntries");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "CalendarEntries",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEntries_AppIdentityUser_UserId",
                table: "CalendarEntries",
                column: "UserId",
                principalTable: "AppIdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
