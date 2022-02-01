﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class SummarysAndEvents2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Events_EventId",
                table: "Summaries");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Summaries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Summaries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Events_EventId",
                table: "Summaries",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summaries_Events_EventId",
                table: "Summaries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Summaries");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "Summaries",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Summaries_Events_EventId",
                table: "Summaries",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");
        }
    }
}
