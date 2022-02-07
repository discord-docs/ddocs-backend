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
    [Migration("20220207085704_UserProfiles2")]
    partial class UserProfiles2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthorEvent", b =>
                {
                    b.Property<Guid>("AuthorsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EventsEventId")
                        .HasColumnType("uuid");

                    b.HasKey("AuthorsId", "EventsEventId");

                    b.HasIndex("EventsEventId");

                    b.ToTable("AuthorEvent");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Asset", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<byte>("ContentType")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ThumbnailId")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ThumbnailId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Authentication", b =>
                {
                    b.Property<decimal>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("JWTRefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("RefreshExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId");

                    b.ToTable("Authentication");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EventDraftDraftId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Revised")
                        .HasColumnType("boolean");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("EventDraftDraftId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.DiscordOAuthAuthentication", b =>
                {
                    b.Property<decimal>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("AccessToken")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("DiscordAuthentication");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.DiscordUserPfp", b =>
                {
                    b.Property<decimal>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("AssetId")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("AssetId")
                        .IsUnique();

                    b.ToTable("UserProfiles");
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

                    b.Property<string>("Thumbnail")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.EventDraft", b =>
                {
                    b.Property<Guid>("DraftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("HeldAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("DraftId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Drafts");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Summary", b =>
                {
                    b.Property<Guid>("SummaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<Guid?>("EventDraftDraftId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsNew")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("LastRevised")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("SummaryId");

                    b.HasIndex("EventDraftDraftId");

                    b.HasIndex("EventId");

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.VerifiedAuthor", b =>
                {
                    b.Property<decimal>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("AvatarId")
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("VerifiedAuthors");
                });

            modelBuilder.Entity("AuthorEvent", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DDocsBackend.Data.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Asset", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.Asset", "Thumbnail")
                        .WithMany()
                        .HasForeignKey("ThumbnailId");

                    b.Navigation("Thumbnail");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Author", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.EventDraft", null)
                        .WithMany("Contributors")
                        .HasForeignKey("EventDraftDraftId");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.DiscordUserPfp", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.Asset", "Asset")
                        .WithOne()
                        .HasForeignKey("DDocsBackend.Data.Models.DiscordUserPfp", "AssetId");

                    b.Navigation("Asset");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.EventDraft", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Summary", b =>
                {
                    b.HasOne("DDocsBackend.Data.Models.EventDraft", null)
                        .WithMany("Summaries")
                        .HasForeignKey("EventDraftDraftId");

                    b.HasOne("DDocsBackend.Data.Models.Event", null)
                        .WithMany("Summaries")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.Event", b =>
                {
                    b.Navigation("Summaries");
                });

            modelBuilder.Entity("DDocsBackend.Data.Models.EventDraft", b =>
                {
                    b.Navigation("Contributors");

                    b.Navigation("Summaries");
                });
#pragma warning restore 612, 618
        }
    }
}
