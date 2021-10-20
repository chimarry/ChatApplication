using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatApplication.Core.Migrations
{
    public partial class Refactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaliciousAttacks_User_UserId",
                table: "MaliciousAttacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaliciousAttacks",
                table: "MaliciousAttacks");

            migrationBuilder.DropIndex(
                name: "IX_MaliciousAttacks_UserId",
                table: "MaliciousAttacks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MaliciousAttacks");

            migrationBuilder.RenameTable(
                name: "MaliciousAttacks",
                newName: "MaliciousAttackRecord");

            migrationBuilder.AddColumn<string>(
                name: "Attacker",
                table: "MaliciousAttackRecord",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaliciousAttackRecord",
                table: "MaliciousAttackRecord",
                column: "MaliciousAttackRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MaliciousAttackRecord",
                table: "MaliciousAttackRecord");

            migrationBuilder.DropColumn(
                name: "Attacker",
                table: "MaliciousAttackRecord");

            migrationBuilder.RenameTable(
                name: "MaliciousAttackRecord",
                newName: "MaliciousAttacks");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MaliciousAttacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaliciousAttacks",
                table: "MaliciousAttacks",
                column: "MaliciousAttackRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_MaliciousAttacks_UserId",
                table: "MaliciousAttacks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaliciousAttacks_User_UserId",
                table: "MaliciousAttacks",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
