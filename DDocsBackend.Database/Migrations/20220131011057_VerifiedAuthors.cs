using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class VerifiedAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscordAuthentication_Authentication_UserId",
                table: "DiscordAuthentication");

            migrationBuilder.CreateTable(
                name: "VerifiedAuthors",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedAuthors", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerifiedAuthors");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscordAuthentication_Authentication_UserId",
                table: "DiscordAuthentication",
                column: "UserId",
                principalTable: "Authentication",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
