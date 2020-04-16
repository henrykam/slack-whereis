using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HenryKam.SlackWhereIs.Migrations
{
    public partial class update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_location_tags",
                schema: "app",
                table: "location");

            migrationBuilder.DropColumn(
                name: "tags",
                schema: "app",
                table: "location");

            migrationBuilder.CreateTable(
                name: "tag",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "locationtag",
                schema: "app",
                columns: table => new
                {
                    location_id = table.Column<long>(nullable: false),
                    tag_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locationtag", x => new { x.location_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_location_tag_location_location_id",
                        column: x => x.location_id,
                        principalSchema: "app",
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_location_tag_tag_tag_id",
                        column: x => x.tag_id,
                        principalSchema: "app",
                        principalTable: "tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_location_tag_tag_id",
                schema: "app",
                table: "locationtag",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_value",
                schema: "app",
                table: "tag",
                column: "value",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "locationtag",
                schema: "app");

            migrationBuilder.DropTable(
                name: "tag",
                schema: "app");

            migrationBuilder.AddColumn<string>(
                name: "tags",
                schema: "app",
                table: "location",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_location_tags",
                schema: "app",
                table: "location",
                column: "tags");
        }
    }
}
