using Microsoft.EntityFrameworkCore.Migrations;

namespace HenryKam.SlackWhereIs.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location_type",
                schema: "app",
                table: "location");

            migrationBuilder.AlterColumn<string>(
                name: "map_image_url",
                schema: "app",
                table: "location",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                schema: "app",
                table: "location",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email_address",
                schema: "app",
                table: "location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                schema: "app",
                table: "location");

            migrationBuilder.DropColumn(
                name: "email_address",
                schema: "app",
                table: "location");

            migrationBuilder.AlterColumn<string>(
                name: "map_image_url",
                schema: "app",
                table: "location",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "location_type",
                schema: "app",
                table: "location",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
