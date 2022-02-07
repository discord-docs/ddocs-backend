using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class AssetUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ContentType = table.Column<byte>(type: "smallint", nullable: false),
                    ThumbnailId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Assets_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    JWTRefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DiscordAuthentication",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordAuthentication", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    HeldAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Thumbnail = table.Column<string>(type: "text", nullable: true),
                    Deprecated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "VerifiedAuthors",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedAuthors", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AuthorEvent",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventsEventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorEvent", x => new { x.AuthorsId, x.EventsEventId });
                    table.ForeignKey(
                        name: "FK_AuthorEvent_Events_EventsEventId",
                        column: x => x.EventsEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Revised = table.Column<bool>(type: "boolean", nullable: false),
                    EventDraftDraftId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drafts",
                columns: table => new
                {
                    DraftId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    HeldAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Thumbnail = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drafts", x => x.DraftId);
                    table.ForeignKey(
                        name: "FK_Drafts_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    SummaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("PK_Summaries", x => x.SummaryId);
                    table.ForeignKey(
                        name: "FK_Summaries_Drafts_EventDraftDraftId",
                        column: x => x.EventDraftDraftId,
                        principalTable: "Drafts",
                        principalColumn: "DraftId");
                    table.ForeignKey(
                        name: "FK_Summaries_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ThumbnailId",
                table: "Assets",
                column: "ThumbnailId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorEvent_EventsEventId",
                table: "AuthorEvent",
                column: "EventsEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_EventDraftDraftId",
                table: "Authors",
                column: "EventDraftDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_AuthorId",
                table: "Drafts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_EventDraftDraftId",
                table: "Summaries",
                column: "EventDraftDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_EventId",
                table: "Summaries",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorEvent_Authors_AuthorsId",
                table: "AuthorEvent",
                column: "AuthorsId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Drafts_EventDraftDraftId",
                table: "Authors",
                column: "EventDraftDraftId",
                principalTable: "Drafts",
                principalColumn: "DraftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Authors_AuthorId",
                table: "Drafts");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Authentication");

            migrationBuilder.DropTable(
                name: "AuthorEvent");

            migrationBuilder.DropTable(
                name: "DiscordAuthentication");

            migrationBuilder.DropTable(
                name: "Summaries");

            migrationBuilder.DropTable(
                name: "VerifiedAuthors");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Drafts");
        }
    }
}
