using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalina.Migrations
{
    public partial class RolesGuilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "guildID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guildID",
                table: "Roles");
        }
    }
}
