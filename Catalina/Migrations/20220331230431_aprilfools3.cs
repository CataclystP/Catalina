using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalina.Migrations
{
    public partial class aprilfools3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRoleIDsSerialised",
                table: "GuildProperties");

            migrationBuilder.DropColumn(
                name: "CommandChannelsSerialised",
                table: "GuildProperties");

            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "GuildProperties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminRoleIDsSerialised",
                table: "GuildProperties",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CommandChannelsSerialised",
                table: "GuildProperties",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "GuildProperties",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
