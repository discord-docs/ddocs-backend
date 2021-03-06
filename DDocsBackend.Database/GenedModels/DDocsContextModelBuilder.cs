// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace DDocsBackend.Data.Cached
{
    public partial class DDocsContextModel
    {
        partial void Initialize()
        {
            var authorEvent = AuthorEventEntityType.Create(this);
            var admin = AdminEntityType.Create(this);
            var asset = AssetEntityType.Create(this);
            var authentication = AuthenticationEntityType.Create(this);
            var author = AuthorEntityType.Create(this);
            var discordOAuthAuthentication = DiscordOAuthAuthenticationEntityType.Create(this);
            var discordUserPfp = DiscordUserPfpEntityType.Create(this);
            var @event = EventEntityType.Create(this);
            var eventDraft = EventDraftEntityType.Create(this);
            var siteContributor = SiteContributorEntityType.Create(this);
            var summary = SummaryEntityType.Create(this);
            var verifiedAuthor = VerifiedAuthorEntityType.Create(this);

            AuthorEventEntityType.CreateForeignKey1(authorEvent, author);
            AuthorEventEntityType.CreateForeignKey2(authorEvent, @event);
            AssetEntityType.CreateForeignKey1(asset, asset);
            AuthorEntityType.CreateForeignKey1(author, eventDraft);
            DiscordUserPfpEntityType.CreateForeignKey1(discordUserPfp, asset);
            EventDraftEntityType.CreateForeignKey1(eventDraft, author);
            SiteContributorEntityType.CreateForeignKey1(siteContributor, asset);
            SummaryEntityType.CreateForeignKey1(summary, eventDraft);
            SummaryEntityType.CreateForeignKey2(summary, @event);

            AuthorEntityType.CreateSkipNavigation1(author, @event, authorEvent);
            EventEntityType.CreateSkipNavigation1(@event, author, authorEvent);

            AuthorEventEntityType.CreateAnnotations(authorEvent);
            AdminEntityType.CreateAnnotations(admin);
            AssetEntityType.CreateAnnotations(asset);
            AuthenticationEntityType.CreateAnnotations(authentication);
            AuthorEntityType.CreateAnnotations(author);
            DiscordOAuthAuthenticationEntityType.CreateAnnotations(discordOAuthAuthentication);
            DiscordUserPfpEntityType.CreateAnnotations(discordUserPfp);
            EventEntityType.CreateAnnotations(@event);
            EventDraftEntityType.CreateAnnotations(eventDraft);
            SiteContributorEntityType.CreateAnnotations(siteContributor);
            SummaryEntityType.CreateAnnotations(summary);
            VerifiedAuthorEntityType.CreateAnnotations(verifiedAuthor);

            AddAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            AddAnnotation("ProductVersion", "6.0.1");
            AddAnnotation("Relational:MaxIdentifierLength", 63);
        }
    }
}
