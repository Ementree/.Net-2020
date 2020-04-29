using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class qwertyFixv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificWorkerCompetences_Employee_WorkerId1",
                table: "SpecificWorkerCompetences");

            migrationBuilder.DropIndex(
                name: "IX_SpecificWorkerCompetences_WorkerId1",
                table: "SpecificWorkerCompetences");

            migrationBuilder.DropColumn(
                name: "WorkerId1",
                table: "SpecificWorkerCompetences");

            migrationBuilder.AlterColumn<int>(
                name: "WorkerId",
                table: "SpecificWorkerCompetences",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificWorkerCompetences_Employee_WorkerId",
                table: "SpecificWorkerCompetences",
                column: "WorkerId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificWorkerCompetences_Employee_WorkerId",
                table: "SpecificWorkerCompetences");

            migrationBuilder.AlterColumn<long>(
                name: "WorkerId",
                table: "SpecificWorkerCompetences",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "WorkerId1",
                table: "SpecificWorkerCompetences",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecificWorkerCompetences_WorkerId1",
                table: "SpecificWorkerCompetences",
                column: "WorkerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificWorkerCompetences_Employee_WorkerId1",
                table: "SpecificWorkerCompetences",
                column: "WorkerId1",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
