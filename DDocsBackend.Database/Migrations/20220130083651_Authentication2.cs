using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class Authentication2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authentication_DiscordOAuthAuthentication_DiscordAuthentica~",
                table: "Authentication");

            migrationBuilder.DropIndex(
                name: "IX_Authentication_DiscordAuthenticationUserId",
                table: "Authentication");

            migrationBuilder.DropColumn(
                name: "DiscordAuthenticationUserId",
                table: "Authentication");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordOAuthAuthentication_Authentication_UserId",
                table: "DiscordOAuthAuthentication",
                column: "UserId",
                principalTable: "Authentication",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordOAuthAuthentication_Authentication_UserId",
                table: "DiscordOAuthAuthentication");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscordAuthenticationUserId",
                table: "Authentication",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authentication_DiscordAuthenticationUserId",
                table: "Authentication",
                column: "DiscordAuthenticationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authentication_DiscordOAuthAuthentication_DiscordAuthentica~",
                table: "Authentication",
                column: "DiscordAuthenticationUserId",
                principalTable: "DiscordOAuthAuthentication",
                principalColumn: "UserId");
        }
    }
}
