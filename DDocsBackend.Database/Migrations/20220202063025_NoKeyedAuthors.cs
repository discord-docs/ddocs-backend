using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class NoKeyedAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Events_EventId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_EventId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorEvent",
                columns: table => new
                {
                    AuthorsUserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EventsEventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorEvent", x => new { x.AuthorsUserId, x.EventsEventId });
                    table.ForeignKey(
                        name: "FK_AuthorEvent_Authors_AuthorsUserId",
                        column: x => x.AuthorsUserId,
                        principalTable: "Authors",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorEvent_Events_EventsEventId",
                        column: x => x.EventsEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorEvent_EventsEventId",
                table: "AuthorEvent",
                column: "EventsEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorEvent");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Authors",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_EventId",
                table: "Authors",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Events_EventId",
                table: "Authors",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");
        }
    }
}
