using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations
{
    public partial class addComplexity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompetencesModelId",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComplexityId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestionComplexity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionComplexity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CompetencesModelId",
                table: "Questions",
                column: "CompetencesModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ComplexityId",
                table: "Questions",
                column: "ComplexityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Competences_CompetencesModelId",
                table: "Questions",
                column: "CompetencesModelId",
                principalTable: "Competences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionComplexity_ComplexityId",
                table: "Questions",
                column: "ComplexityId",
                principalTable: "QuestionComplexity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Competences_CompetencesModelId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionComplexity_ComplexityId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionComplexity");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CompetencesModelId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ComplexityId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CompetencesModelId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ComplexityId",
                table: "Questions");
        }
    }
}
