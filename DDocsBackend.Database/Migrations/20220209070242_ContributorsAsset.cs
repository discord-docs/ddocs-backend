using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class ContributorsAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteContributors_ProfilePictureId",
                table: "SiteContributors");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContributors_ProfilePictureId",
                table: "SiteContributors",
                column: "ProfilePictureId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteContributors_ProfilePictureId",
                table: "SiteContributors");

            migrationBuilder.CreateIndex(
                name: "IX_SiteContributors_ProfilePictureId",
                table: "SiteContributors",
                column: "ProfilePictureId");
        }
    }
}
