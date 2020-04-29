using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DotNet2020.Data.Migrations
{
    public partial class qwerty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberOfAsk = table.Column<int>(nullable: false),
                    IsSkipped = table.Column<bool>(nullable: false),
                    IsRight = table.Column<bool>(nullable: false),
                    Commentary = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Attestations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkerId = table.Column<long>(nullable: false),
                    GotCompetences = table.Column<List<long>>(nullable: true),
                    Problems = table.Column<string>(nullable: true),
                    NextMoves = table.Column<string>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    IdsTestedCompetences = table.Column<List<long>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attestations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competences",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Competence = table.Column<string>(nullable: true),
                    Content = table.Column<List<string>>(nullable: true),
                    Questions = table.Column<List<string>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Grade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttestationAnswer",
                columns: table => new
                {
                    AttestationId = table.Column<long>(nullable: false),
                    AnswerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttestationAnswer", x => new { x.AttestationId, x.AnswerId });
                    table.ForeignKey(
                        name: "FK_AttestationAnswer_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttestationAnswer_Attestations_AttestationId",
                        column: x => x.AttestationId,
                        principalTable: "Attestations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeCompetences",
                columns: table => new
                {
                    GradeId = table.Column<long>(nullable: false),
                    CompetenceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeCompetences", x => new { x.GradeId, x.CompetenceId });
                    table.ForeignKey(
                        name: "FK_GradeCompetences_Competences_CompetenceId",
                        column: x => x.CompetenceId,
                        principalTable: "Competences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeCompetences_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 255, nullable: true),
                    PositionId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Salary = table.Column<double>(nullable: true),
                    Bonus = table.Column<double>(nullable: true),
                    Commentary = table.Column<string>(nullable: true),
                    PreviousWorkPlaces = table.Column<string>(nullable: true),
                    Experience = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpecificWorkerCompetences",
                columns: table => new
                {
                    WorkerId = table.Column<long>(nullable: false),
                    CompetenceId = table.Column<long>(nullable: false),
                    WorkerId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificWorkerCompetences", x => new { x.WorkerId, x.CompetenceId });
                    table.ForeignKey(
                        name: "FK_SpecificWorkerCompetences_Competences_CompetenceId",
                        column: x => x.CompetenceId,
                        principalTable: "Competences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificWorkerCompetences_Employee_WorkerId1",
                        column: x => x.WorkerId1,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttestationAnswer_AnswerId",
                table: "AttestationAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PositionId",
                table: "Employee",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeCompetences_CompetenceId",
                table: "GradeCompetences",
                column: "CompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificWorkerCompetences_CompetenceId",
                table: "SpecificWorkerCompetences",
                column: "CompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificWorkerCompetences_WorkerId1",
                table: "SpecificWorkerCompetences",
                column: "WorkerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employee_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employee_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AttestationAnswer");

            migrationBuilder.DropTable(
                name: "GradeCompetences");

            migrationBuilder.DropTable(
                name: "SpecificWorkerCompetences");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Attestations");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Competences");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
