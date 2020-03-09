using Microsoft.EntityFrameworkCore.Migrations;

namespace HenryKam.SlackWhereIs.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "floor",
                schema: "app",
                table: "location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "floor",
                schema: "app",
                table: "location");
        }
    }
}
