using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.Events
{
    public class GetEvent : RestModuleBase
    {
        [Route("/events/{eventId}", "GET")]
        public async Task<RestResult> GetEventAsync(string eventId)
        {
            if(!Guid.TryParse(eventId, out var guid))
            {
                return RestResult.BadRequest;
            }

            var evnt = await DataAccessLayer.GetEventAsync(guid).ConfigureAwait(false);

            if (evnt == null)
                return RestResult.NotFound;

            var author = evnt.Authors!.FirstOrDefault(x => !x.Revised);

            var authorUser = author != null ?  await DiscordService.GetUserAsync(author.UserId).ConfigureAwait(false) : null;

            return RestResult.OK.WithData(new Event
            {
                Author = await GetAuthorAsync(author!),
                // bit of a sus async bypass here
                Contributors = await Task.WhenAll(evnt.Authors?.Where(x => x.Revised).Select(x => GetAuthorAsync(x))!),
                Description = evnt.Description,
                EventId = evnt.EventId.ToString("N"),
                HeldAt = evnt.HeldAt,
                Summaries = evnt.Summaries!.Select(x => ConvertSummary(x)),
                Title = evnt.Title,
                Thumbnail = evnt.Thumbnail
            });
        }

        private async Task<Author?> GetAuthorAsync(Data.Models.Author author)
        {
            var authorUser = author != null ? await DiscordService.GetUserAsync(author.UserId).ConfigureAwait(false) : null;

            return new Author()
            {
                Avatar = authorUser?.GetAvatarUrl() ?? DiscordService.GetDefaultAvatar(author!.UserId),
                Discriminator = authorUser?.Discriminator ?? "0000",
                UserId = author!.UserId,
                Username = authorUser?.Username ?? "unknown"
            };
        }

        private EventSummary? ConvertSummary(Data.Models.Summary summary)
        {
            return new EventSummary
            {
                Content = summary.Content,
                FeatureType = summary.Status,
                Id = summary.SummaryId.ToString("N"),
                IsNew = summary.IsNew,
                Thumbnail = summary.Url,
                Title = summary.Title,
                Type = summary.Type
            };
        }
    }
}
