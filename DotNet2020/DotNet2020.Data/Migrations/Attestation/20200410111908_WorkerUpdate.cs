using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.Attestation
{
    public partial class WorkerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "Workers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Workers",
                nullable: true);

            migrationBuilder.AddColumn<List<long>>(
                name: "TestedCompetences",
                table: "Attestations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "TestedCompetences",
                table: "Attestations");
        }
    }
}
