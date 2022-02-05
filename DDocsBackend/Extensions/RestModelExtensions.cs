using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    public static class RestModelExtensions
    {
        public static async Task<Draft> ToRestModelAsync(this Data.Models.EventDraft draft, RestModuleBase context)
        {
            return new Draft
            {
                Title = draft.Title,
                Description = draft.Description,
                HeldAt = draft.HeldAt,
                DraftId = draft.DraftId,
                Author = await draft.Author!.ToRestModelAsync(context),
                Contributors = (await Task.WhenAll(draft.Contributors.Select(x => x.ToRestModelAsync(context)))).ToList(),
                Thumbnail = draft.Thumbnail,
                Summaries = draft.Summaries.Select(x => x.ToRestModel()).ToList()
            };
        }

        public static async Task<Author> ToRestModelAsync(this Data.Models.Author author, RestModuleBase context)
        {
            var user = await context.DiscordService.GetUserDetailsAsync(author.UserId).ConfigureAwait(false);

            return new Author
            {
                UserId = author.UserId,
                Avatar = user.Avatar,
                Discriminator = user.Discriminator,
                Username = user.Username
            };
        }

        public static EventSummary ToRestModel(this Data.Models.Summary summary)
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
