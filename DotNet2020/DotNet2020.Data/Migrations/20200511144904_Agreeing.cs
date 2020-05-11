using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class Agreeing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "AgreeingId",
                table: "AbstractCalendarEntries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vacation_AgreeingId",
                table: "AbstractCalendarEntries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbstractCalendarEntries_AgreeingId",
                table: "AbstractCalendarEntries",
                column: "AgreeingId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractCalendarEntries_Vacation_AgreeingId",
                table: "AbstractCalendarEntries",
                column: "Vacation_AgreeingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractCalendarEntries_Employee_AgreeingId",
                table: "AbstractCalendarEntries",
                column: "AgreeingId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractCalendarEntries_Employee_Vacation_AgreeingId",
                table: "AbstractCalendarEntries",
                column: "Vacation_AgreeingId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractCalendarEntries_Employee_AgreeingId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_AbstractCalendarEntries_Employee_Vacation_AgreeingId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropIndex(
                name: "IX_AbstractCalendarEntries_AgreeingId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropIndex(
                name: "IX_AbstractCalendarEntries_Vacation_AgreeingId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropColumn(
                name: "AgreeingId",
                table: "AbstractCalendarEntries");

            migrationBuilder.DropColumn(
                name: "Vacation_AgreeingId",
                table: "AbstractCalendarEntries");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }
    }
}
