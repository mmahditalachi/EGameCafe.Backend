using Microsoft.EntityFrameworkCore.Migrations;

namespace EGameCafe.Infrastructure.Migrations
{
    public partial class AddColumnSharingLinkToGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SharingLink",
                table: "GamingGroups",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SharingLink",
                table: "GamingGroups");
        }
    }
}
