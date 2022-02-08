using DDocsBackend.Helpers;
using DDocsBackend.RestModels;
using HttpMultipartParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Routes.CDN
{
    public class CreateAsset : RestModuleBase
    {
        [Route("/assets", "POST")]
        [RequireAuthentication(true)]
        public async Task<RestResult> CreateAssetAsync()
        {
            var body = await MultipartFormDataParser.ParseAsync(Request.InputStream).ConfigureAwait(false);

            if(body == null)
                return RestResult.BadRequest;

            var fileContent = body.Files.FirstOrDefault(x => x.Name != null && x.Name == "content");

            if (fileContent == null)
                return RestResult.BadRequest;

            CropBody? cropBody = null;

            if (body.HasParameter("crop"))
            {
                var json = body.GetParameterValue("crop");
                cropBody = JsonConvert.DeserializeObject<CropBody>(json);
            }

            var processed = await ImageHelper.ProcessUserImageAsync(this, fileContent.Data, cropBody).ConfigureAwait(false);

            return RestResult.OK.WithData(new
            {
                id = processed.AssetId,
                format = processed.Format
            });
        }
    }
}
