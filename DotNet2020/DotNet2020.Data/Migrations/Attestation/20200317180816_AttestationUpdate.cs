using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.Attestation
{
    public partial class AttestationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<long>>(
                name: "CompetencesId",
                table: "Attestations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkerId",
                table: "Attestations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetencesId",
                table: "Attestations");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Attestations");
        }
    }
}
