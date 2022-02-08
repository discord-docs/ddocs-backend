using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class EventFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Drafts_EventDraftDraftId",
                table: "Summaries");

            migrationBuilder.RenameColumn(
                name: "EventDraftDraftId",
                table: "Summaries",
                newName: "DraftId");

            migrationBuilder.RenameIndex(
                name: "IX_Summaries_EventDraftDraftId",
                table: "Summaries",
                newName: "IX_Summaries_DraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Drafts_DraftId",
                table: "Summaries",
                column: "DraftId",
                principalTable: "Drafts",
                principalColumn: "DraftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Drafts_DraftId",
                table: "Summaries");

            migrationBuilder.RenameColumn(
                name: "DraftId",
                table: "Summaries",
                newName: "EventDraftDraftId");

            migrationBuilder.RenameIndex(
                name: "IX_Summaries_DraftId",
                table: "Summaries",
                newName: "IX_Summaries_EventDraftDraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Drafts_EventDraftDraftId",
                table: "Summaries",
                column: "EventDraftDraftId",
                principalTable: "Drafts",
                principalColumn: "DraftId");
        }
    }
}
