using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class Authentication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscordOAuthAuthentication",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordOAuthAuthentication", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    JWTRefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DiscordAuthenticationUserId = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Authentication_DiscordOAuthAuthentication_DiscordAuthentica~",
                        column: x => x.DiscordAuthenticationUserId,
                        principalTable: "DiscordOAuthAuthentication",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authentication_DiscordAuthenticationUserId",
                table: "Authentication",
                column: "DiscordAuthenticationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authentication");

            migrationBuilder.DropTable(
                name: "DiscordOAuthAuthentication");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Events");
        }
    }
}
