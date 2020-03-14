using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.Worker
{
    public partial class workersUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PreviousWorkPlaces",
                table: "Workers",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string[]>(
                name: "PreviousWorkPlaces",
                table: "Workers",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
