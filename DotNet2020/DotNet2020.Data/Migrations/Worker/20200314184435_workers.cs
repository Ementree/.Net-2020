using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations.Worker
{
    public partial class workers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Salary = table.Column<double>(nullable: false),
                    Bonus = table.Column<double>(nullable: false),
                    Commentary = table.Column<string>(nullable: true),
                    Competences = table.Column<string[]>(nullable: true),
                    PreviousWorkPlaces = table.Column<string[]>(nullable: true),
                    Experience = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
