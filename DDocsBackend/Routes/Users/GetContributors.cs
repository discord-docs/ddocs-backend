using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDocsBackend.RestModels;

namespace DDocsBackend.Routes.Users
{
    public class GetContributors : RestModuleBase
    {
        [Route("/users/contributors", "GET")]
        public async Task<RestResult> ExecuteAsync()
        {
            var contributors = await DataAccessLayer.GetContributorsAsync().ConfigureAwait(false);

            return RestResult.OK.WithData(contributors.Select(x => new Author()
            {
                UserId = x.UserId,
                Username = x.Username,
                Avatar = x.ProfilePicture!.Id,
                Discriminator = x.Discriminator
                
            }));
        }
    }
}
