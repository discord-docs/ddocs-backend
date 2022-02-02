using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    public partial class LastRevised : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastRevised",
                table: "Summaries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRevised",
                table: "Summaries");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,");
        }
    }
}
