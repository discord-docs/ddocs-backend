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
            var result = await DiscordService.GetUserDetailsAsync(Authentication!.UserId).ConfigureAwait(false);

            if (result == null)
                return RestResult.NotFound;

            return RestResult.OK.WithData(new User
            {
                Avatar = result.Avatar,
                Discriminator = result.Discriminator,
                Id = result.UserId,
                Username = result.Username,
                IsAuthor = await DataAccessLayer.IsAuthorAsync(result.UserId).ConfigureAwait(false)
            });
        }
    }
}
