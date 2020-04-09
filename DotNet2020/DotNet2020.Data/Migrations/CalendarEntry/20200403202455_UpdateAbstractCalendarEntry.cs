using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.CalendarEntry
{
    public partial class UpdateAbstractCalendarEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CalendarEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppIdentityUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppIdentityUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_UserId",
                table: "CalendarEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEntries_AppIdentityUser_UserId",
                table: "CalendarEntries",
                column: "UserId",
                principalTable: "AppIdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEntries_AppIdentityUser_UserId",
                table: "CalendarEntries");

            migrationBuilder.DropTable(
                name: "AppIdentityUser");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEntries_UserId",
                table: "CalendarEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CalendarEntries");
        }
    }
}
