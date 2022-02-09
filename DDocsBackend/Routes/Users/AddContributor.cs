using HttpMultipartParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDocsBackend.Data.Models;
using DDocsBackend.Helpers;
using DDocsBackend.RestModels;
using SixLabors.ImageSharp;
using System.Net;

namespace DDocsBackend.Routes.Users
{
    internal class AddContributor : RestModuleBase
    {
        [Route("/users/contributors", "POST")]
        [RequireAdmin]
        public async Task<RestResult> ExecuteAsync()
        {
            var body = await MultipartFormDataParser.ParseAsync(Request.InputStream).ConfigureAwait(false);

            if (!body.HasParameter("id")  || !body.HasParameter("username") || !body.HasParameter("discriminator"))
            {
                return RestResult.BadRequest;
            }

            var rawUserId = body.GetParameterValue("id");

            if (rawUserId == null)
            {
                return RestResult.BadRequest;
            }

            if (!ulong.TryParse(rawUserId, out var userId))
            {
                return RestResult.BadRequest;
            }


            var user = await DiscordService.GetUserAsync(userId).ConfigureAwait(false);

            if(user == null)
            {
                return RestResult.NotFound;
            }

            Stream fileContent;

            using(var client = new HttpClient())
            {
                var result = await client.GetAsync(user.GetAvatarUrl(Discord.ImageFormat.Png) ?? user.GetDefaultAvatarUrl()).ConfigureAwait(false);

                if(!result.IsSuccessStatusCode)
                    return RestResult.NotFound;

                fileContent = await result.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }

            if (fileContent == null)
            {
                return RestResult.BadRequest;
            }
            
            var asset = await DataAccessLayer.CreateAssetAsync(AssetType.Avatar, ContentType.Png).ConfigureAwait(false);

            using (var image = Image.Load(fileContent))
            {
                await CDNService.WriteAsync(asset.Id!, AssetType.Avatar, ContentType.Png, image).ConfigureAwait(false);
            }

           
            var username = body.GetParameterValue("username");
            var discriminator = body.GetParameterValue("discriminator");
            
            var contributor = await DataAccessLayer.CreateContributorAsync(userId, username, discriminator, asset).ConfigureAwait(false);
            
            return RestResult.OK.WithData(new RestModels.Author()
            {
                UserId = contributor.UserId,
                Username = contributor.Username,
                Discriminator = contributor.Discriminator,
                Avatar = asset.Id
            });
        }
    }
}
