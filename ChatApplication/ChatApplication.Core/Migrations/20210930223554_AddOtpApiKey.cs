using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplication.Core.Migrations
{
    public partial class AddOtpApiKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtpApiKey",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpApiKey",
                table: "User");
        }
    }
}
