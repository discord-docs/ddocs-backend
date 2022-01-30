using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class DiscordOAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordOAuthAuthentication_Authentication_UserId",
                table: "DiscordOAuthAuthentication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordOAuthAuthentication",
                table: "DiscordOAuthAuthentication");

            migrationBuilder.RenameTable(
                name: "DiscordOAuthAuthentication",
                newName: "DiscordAuthentication");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordAuthentication",
                table: "DiscordAuthentication",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordAuthentication_Authentication_UserId",
                table: "DiscordAuthentication",
                column: "UserId",
                principalTable: "Authentication",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordAuthentication_Authentication_UserId",
                table: "DiscordAuthentication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscordAuthentication",
                table: "DiscordAuthentication");

            migrationBuilder.RenameTable(
                name: "DiscordAuthentication",
                newName: "DiscordOAuthAuthentication");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscordOAuthAuthentication",
                table: "DiscordOAuthAuthentication",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordOAuthAuthentication_Authentication_UserId",
                table: "DiscordOAuthAuthentication",
                column: "UserId",
                principalTable: "Authentication",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
