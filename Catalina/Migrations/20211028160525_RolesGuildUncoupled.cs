using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalina.Migrations
{
    public partial class RolesGuildUncoupled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.AlterColumn<ulong>(
                name: "userID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");

            migrationBuilder.AlterColumn<ulong>(
                name: "guildID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned");

            migrationBuilder.AlterColumn<ulong>(
                name: "roleID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<ulong>(
                name: "ID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Roles");

            migrationBuilder.AlterColumn<ulong>(
                name: "userID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "roleID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "guildID",
                table: "Roles",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "roleID");
        }
    }
}
