using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations
{
    public partial class ResourceGroupType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ResourceTypes_ResourceTypeId",
                table: "Resources");

            migrationBuilder.DropTable(
                name: "ResourceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ResourceTypeId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ResourceTypeId",
                table: "Resources");

            migrationBuilder.AddColumn<int>(
                name: "ResourceGroupTypeId",
                table: "Resources",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ResourceGroupsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceGroupsTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceGroupTypeId",
                table: "Resources",
                column: "ResourceGroupTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ResourceGroupsTypes_ResourceGroupTypeId",
                table: "Resources",
                column: "ResourceGroupTypeId",
                principalTable: "ResourceGroupsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_ResourceGroupsTypes_ResourceGroupTypeId",
                table: "Resources");

            migrationBuilder.DropTable(
                name: "ResourceGroupsTypes");

            migrationBuilder.DropIndex(
                name: "IX_Resources_ResourceGroupTypeId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ResourceGroupTypeId",
                table: "Resources");

            migrationBuilder.AddColumn<int>(
                name: "ResourceTypeId",
                table: "Resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ResourceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceTypeId",
                table: "Resources",
                column: "ResourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_ResourceTypes_ResourceTypeId",
                table: "Resources",
                column: "ResourceTypeId",
                principalTable: "ResourceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
