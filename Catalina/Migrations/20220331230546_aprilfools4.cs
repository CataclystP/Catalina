using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalina.Migrations
{
    public partial class aprilfools4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminRoleIDsSerialised",
                table: "GuildProperties",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRoleIDsSerialised",
                table: "GuildProperties");
        }
    }
}
