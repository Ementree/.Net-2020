using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations
{
    public partial class updateComplexity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionComplexity_ComplexityId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ComplexityId",
                table: "Questions");

            migrationBuilder.AlterColumn<long>(
                name: "ComplexityId",
                table: "Questions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ComplexityId",
                table: "Questions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ComplexityId",
                table: "Questions",
                column: "ComplexityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionComplexity_ComplexityId",
                table: "Questions",
                column: "ComplexityId",
                principalTable: "QuestionComplexity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
