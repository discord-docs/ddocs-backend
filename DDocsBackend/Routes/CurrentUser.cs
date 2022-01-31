using DDocsBackend.RestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes
{
    internal class CurrentUser : RestModuleBase
    {
        [Route("/users/@me", "GET")]
        [RequireAuthentication]
        public async Task<RestResult> ExecuteAsync()
        {
            var oauth = await DataAccessLayer.GetDiscordOAuthAsync(Authentication!.UserId).ConfigureAwait(false);

            if (oauth == null)
                return RestResult.Unauthorized;

            var result = await DiscordService.GetUserAsync(oauth).ConfigureAwait(false);

            if (result == null)
                return RestResult.NotFound;

            return RestResult.OK.WithData(new User
            {
                Avatar = result.GetAvatarUrl() ?? result.GetDefaultAvatarUrl(),
                Discriminator = result.Discriminator,
                Id = result.Id,
                Username = result.Username,
                IsAuthor = await DataAccessLayer.IsAuthorAsync(result.Id).ConfigureAwait(false)
            });
        }
    }
}
