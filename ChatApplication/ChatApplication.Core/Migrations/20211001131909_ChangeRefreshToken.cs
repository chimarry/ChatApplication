using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplication.Core.Migrations
{
    public partial class ChangeRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "RefreshToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "RefreshToken",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
