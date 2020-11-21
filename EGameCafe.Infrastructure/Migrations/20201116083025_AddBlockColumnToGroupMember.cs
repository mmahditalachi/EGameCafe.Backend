using Microsoft.EntityFrameworkCore.Migrations;

namespace EGameCafe.Infrastructure.Migrations
{
    public partial class AddBlockColumnToGroupMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Block",
                table: "GroupMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "GroupMembers");
        }
    }
}
