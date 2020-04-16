using Microsoft.EntityFrameworkCore.Migrations;

namespace HenryKam.SlackWhereIs.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tags",
                schema: "app",
                table: "location",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_location_tags",
                schema: "app",
                table: "location",
                column: "tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_location_tags",
                schema: "app",
                table: "location");

            migrationBuilder.DropColumn(
                name: "tags",
                schema: "app",
                table: "location");
        }
    }
}
