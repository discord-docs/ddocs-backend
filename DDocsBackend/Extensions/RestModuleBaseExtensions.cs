using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    public static class RestModuleBaseExtensions
    {
        public static async Task<Author?> GetAuthorAsync(this RestModuleBase modBase, Data.Models.Author author)
        {
            var authorUser = author != null ? await modBase.DiscordService.GetUserAsync(author.UserId).ConfigureAwait(false) : null;

            return new Author()
            {
                Avatar = authorUser?.GetAvatarUrl() ?? modBase.DiscordService.GetDefaultAvatar(author!.UserId),
                Discriminator = authorUser?.Discriminator ?? "0000",
                UserId = author!.UserId,
                Username = authorUser?.Username ?? "unknown"
            };
        }

        public static EventSummary? ConvertSummary(this RestModuleBase modBase, Data.Models.Summary summary)
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
