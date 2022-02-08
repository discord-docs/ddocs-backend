using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class EventFix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftSummary");

            migrationBuilder.AddColumn<Guid>(
                name: "EventDraftDraftId",
                table: "Summaries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_EventDraftDraftId",
                table: "Summaries",
                column: "EventDraftDraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Drafts_EventDraftDraftId",
                table: "Summaries",
                column: "EventDraftDraftId",
                principalTable: "Drafts",
                principalColumn: "DraftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Drafts_EventDraftDraftId",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_Summaries_EventDraftDraftId",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "EventDraftDraftId",
                table: "Summaries");

            migrationBuilder.CreateTable(
                name: "DraftSummary",
                columns: table => new
                {
                    SummaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    EventDraftDraftId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsNew = table.Column<bool>(type: "boolean", nullable: false),
                    LastRevised = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftSummary", x => x.SummaryId);
                    table.ForeignKey(
                        name: "FK_DraftSummary_Drafts_EventDraftDraftId",
                        column: x => x.EventDraftDraftId,
                        principalTable: "Drafts",
                        principalColumn: "DraftId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftSummary_EventDraftDraftId",
                table: "DraftSummary",
                column: "EventDraftDraftId");
        }
    }
}
