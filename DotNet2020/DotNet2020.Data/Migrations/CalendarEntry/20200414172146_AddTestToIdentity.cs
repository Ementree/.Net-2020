using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNet2020.Data.Migrations.CalendarEntry
{
    public partial class AddTestToIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "Test",
            //    table: "AspNetUsers",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "AspNetUsers");
        }
    }
}
