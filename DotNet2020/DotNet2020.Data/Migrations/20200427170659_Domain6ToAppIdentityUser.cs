using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class Domain6ToAppIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Resources");

            migrationBuilder.AddColumn<string>(
                name: "AppIdentityUserId",
                table: "Resources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_AppIdentityUserId",
                table: "Resources",
                column: "AppIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_AspNetUsers_AppIdentityUserId",
                table: "Resources",
                column: "AppIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_AspNetUsers_AppIdentityUserId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_AppIdentityUserId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "AppIdentityUserId",
                table: "Resources");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Resources",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Resources",
                type: "text",
                nullable: true);
        }
    }
}
