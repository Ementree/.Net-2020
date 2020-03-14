using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.Competences
{
    public partial class competencesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Questions",
                table: "Competences",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Questions",
                table: "Competences");
        }
    }
}
