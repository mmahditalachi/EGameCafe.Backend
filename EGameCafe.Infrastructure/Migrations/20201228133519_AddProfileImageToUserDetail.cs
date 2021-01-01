using Microsoft.EntityFrameworkCore.Migrations;

namespace EGameCafe.Infrastructure.Migrations
{
    public partial class AddProfileImageToUserDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "UserDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "UserDetails");
        }
    }
}
