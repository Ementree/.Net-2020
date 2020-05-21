using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations
{
    public partial class updateReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Questions",
                table: "Competences");

            migrationBuilder.CreateTable(
                name: "CompetenceQuestions",
                columns: table => new
                {
                    CompetenceId = table.Column<long>(nullable: false),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenceQuestions", x => new { x.CompetenceId, x.QuestionId });
                });

            migrationBuilder.CreateTable(
                name: "GradeToGrade",
                columns: table => new
                {
                    GradeId = table.Column<long>(nullable: false),
                    NextGradeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeToGrade", x => new { x.GradeId, x.NextGradeId });
                });

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

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Question = table.Column<string>(nullable: true),
                    ComplexityId = table.Column<long>(nullable: false),
                    CompetencesModelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Competences_CompetencesModelId",
                        column: x => x.CompetencesModelId,
                        principalTable: "Competences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    IssueFilter = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    IssueId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    CreatorName = table.Column<string>(nullable: true),
                    AssigneeName = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    EstimatedTime = table.Column<int>(nullable: true),
                    SpentTime = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.IssueId);
                    table.ForeignKey(
                        name: "FK_Issue_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkItem",
                columns: table => new
                {
                    WorkItemId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IssueId = table.Column<int>(nullable: false),
                    SpentTime = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItem", x => x.WorkItemId);
                    table.ForeignKey(
                        name: "FK_WorkItem_Issue_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issue",
                        principalColumn: "IssueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Issue_ReportId",
                table: "Issue",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CompetencesModelId",
                table: "Questions",
                column: "CompetencesModelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItem_IssueId",
                table: "WorkItem",
                column: "IssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenceQuestions");

            migrationBuilder.DropTable(
                name: "GradeToGrade");

            migrationBuilder.DropTable(
                name: "QuestionComplexity");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "WorkItem");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.AddColumn<List<string>>(
                name: "Questions",
                table: "Competences",
                type: "text[]",
                nullable: true);
        }
    }
}
