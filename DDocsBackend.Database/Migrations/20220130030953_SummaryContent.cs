using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class SummaryContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Summaries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Summaries",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Summaries");
        }
    }
}
