using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGameCafe.Infrastructure.Migrations
{
    public partial class CreateGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamingGroups",
                columns: table => new
                {
                    GamingGroupGroupId = table.Column<string>(maxLength: 64, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(maxLength: 50, nullable: false),
                    GroupType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamingGroups", x => x.GamingGroupGroupId);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    GroupMemberId = table.Column<string>(maxLength: 64, nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.GroupMemberId);
                    table.ForeignKey(
                        name: "FK_GroupMembers_GamingGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GamingGroups",
                        principalColumn: "GamingGroupGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMembers",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "GamingGroups");
        }
    }
}
