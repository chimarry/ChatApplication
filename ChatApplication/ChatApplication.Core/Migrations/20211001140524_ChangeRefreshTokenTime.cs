using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplication.Core.Migrations
{
    public partial class ChangeRefreshTokenTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "RefreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RefreshToken",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresOn",
                table: "RefreshToken",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "ExpiresOn",
                table: "RefreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "RefreshToken",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "RefreshToken",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
