﻿// <auto-generated />
using System;
using DDocsBackend.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DDocsBackend.Data.Migrations
{
    [DbContext(typeof(DDocsContext))]
    [Migration("20220130022936_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DDocsBackend.Data.Models.Author", b =>
                {
                    b.Property<decimal>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Revised")
                        .HasColumnType("boolean");

                    b.HasKey("UserId");

                    b.HasIndex("EventId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deprecated")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("HeldAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Summary", b =>
                {
                    b.Property<Guid>("SummaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsNew")
                        .HasColumnType("boolean");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("SummaryId");

                    b.HasIndex("EventId");

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Author", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.Event", null)
                        .WithMany("Authors")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Summary", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.Event", null)
                        .WithMany("Summaries")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Event", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Summaries");
                });
#pragma warning restore 612, 618
        }
    }
}
