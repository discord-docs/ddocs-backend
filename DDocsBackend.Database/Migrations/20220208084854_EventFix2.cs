using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class EventFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Drafts_DraftId",
                table: "Summaries");

            migrationBuilder.DropIndex(
                name: "IX_Summaries_DraftId",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "DraftId",
                table: "Summaries");

            migrationBuilder.CreateTable(
                name: "DraftSummary",
                columns: table => new
                {
                    SummaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsNew = table.Column<bool>(type: "boolean", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    LastRevised = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventDraftDraftId = table.Column<Guid>(type: "uuid", nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftSummary");

            migrationBuilder.AddColumn<Guid>(
                name: "DraftId",
                table: "Summaries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_DraftId",
                table: "Summaries",
                column: "DraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Drafts_DraftId",
                table: "Summaries",
                column: "DraftId",
                principalTable: "Drafts",
                principalColumn: "DraftId");
        }
    }
}
