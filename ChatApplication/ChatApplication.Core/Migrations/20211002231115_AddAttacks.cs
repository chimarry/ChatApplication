using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplication.Core.Migrations
{
    public partial class AddAttacks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaliciousAttacks",
                columns: table => new
                {
                    MaliciousAttackRecordId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Details = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    AttemptedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaliciousAttacks", x => x.MaliciousAttackRecordId);
                    table.ForeignKey(
                        name: "FK_MaliciousAttacks_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaliciousAttacks_UserId",
                table: "MaliciousAttacks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaliciousAttacks");
        }
    }
}
