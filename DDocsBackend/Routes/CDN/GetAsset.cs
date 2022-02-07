using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace DDocsBackend.Routes.CDN
{
    public class GetAsset : RestModuleBase
    {
        [Route(@"\/assets\/(.{32})$", "GET", true)]
        public async Task<RestResult> ExecuteAsync(string assetId)
        {
            if (!Guid.TryParse(assetId, out var id))
            {
                return RestResult.BadRequest;
            }

            var assetDetails = await DataAccessLayer.GetAssetAsync(id).ConfigureAwait(false);

            if (assetDetails == null)
            {
                return RestResult.NotFound;
            }

            using (var result = await CDNService.ReadAsync(id.ToString("N"), assetDetails.Type).ConfigureAwait(false))
            {
                if (!result.HasValue)
                {
                    return RestResult.NotFound;
                }

                Response.StatusCode = 200;
                Response.AddHeader("Content-Type", result.Value.Type.ToHTTPContentType());

                await result.Value.Image.SaveAsync(Response.OutputStream, result.Value.Type.GetEncoder()).ConfigureAwait(false);

                Response.Close();
            }

            return RestResult.NoAction;
        }
    }
}
