using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.Attestation
{
    public partial class AttestationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetencesId",
                table: "Attestations");

            migrationBuilder.DropColumn(
                name: "TestedCompetences",
                table: "Attestations");

            migrationBuilder.AlterColumn<long>(
                name: "WorkerId",
                table: "Attestations",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<List<long>>(
                name: "GotCompetences",
                table: "Attestations",
                nullable: true);

            migrationBuilder.AddColumn<List<long>>(
                name: "IdsTestedCompetences",
                table: "Attestations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GotCompetences",
                table: "Attestations");

            migrationBuilder.DropColumn(
                name: "IdsTestedCompetences",
                table: "Attestations");

            migrationBuilder.AlterColumn<long>(
                name: "WorkerId",
                table: "Attestations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<List<long>>(
                name: "CompetencesId",
                table: "Attestations",
                type: "bigint[]",
                nullable: true);

            migrationBuilder.AddColumn<List<long>>(
                name: "TestedCompetences",
                table: "Attestations",
                type: "bigint[]",
                nullable: true);
        }
    }
}
