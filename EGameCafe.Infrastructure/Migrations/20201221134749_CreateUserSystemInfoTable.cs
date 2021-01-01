using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGameCafe.Infrastructure.Migrations
{
    public partial class CreateUserSystemInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSystemInfo",
                columns: table => new
                {
                    UserSystemInfoId = table.Column<string>(maxLength: 64, nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    RamManufacturer = table.Column<int>(nullable: false),
                    TotalRam = table.Column<int>(nullable: false),
                    GraphicCardManufacturer = table.Column<int>(nullable: false),
                    GraphicCardName = table.Column<string>(maxLength: 50, nullable: true),
                    CpuManufacturer = table.Column<int>(nullable: false),
                    CpuName = table.Column<string>(maxLength: 50, nullable: true),
                    CaseManufacturer = table.Column<int>(nullable: false),
                    CaseName = table.Column<string>(maxLength: 50, nullable: true),
                    PowerManufacturer = table.Column<int>(nullable: false),
                    PowerName = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSystemInfo", x => x.UserSystemInfoId);
                    table.ForeignKey(
                        name: "FK_UserSystemInfo_UserDetails_UserId",
                        column: x => x.UserId,
                        principalTable: "UserDetails",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSystemInfo_UserId",
                table: "UserSystemInfo",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSystemInfo");
        }
    }
}
