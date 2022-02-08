using DDocsBackend.Data.Models;
using DDocsBackend.RestModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Helpers
{
    public class ImageHelper
    {
        public static async Task<ProcessedImage> ProcessUserImageAsync(RestModuleBase context, Stream imageContent, CropBody? body = null)
        {
            using (var image = Image.Load(imageContent, out var format))
            {
                // create the asset 
                var contentType = GetContentType(format);

                if(body != null)
                {
                    image.Mutate(i => i.Crop(new Rectangle(body.X, body.Y, body.CropWidth, body.CropHeight)));
                }

                var mainAsset = await context.DataAccessLayer.CreateAssetAsync(AssetType.Content, contentType, AssetType.Thumbnail, contentType);
                await context.CDNService.WriteAsync(mainAsset.Id!, AssetType.Content, contentType, image);

                // create the thumbnail
                image.Mutate(x => x.Resize(0, 80));

                await context.CDNService.WriteAsync(mainAsset.Thumbnail!.Id!, AssetType.Thumbnail, contentType, image);

                return new ProcessedImage
                {
                    Format = format.Name.ToLower(),
                    AssetId = mainAsset.Id,
                    ThumbnailId = mainAsset.Thumbnail!.Id
                };
            }
        }

        public static ContentType GetContentType(IImageFormat format)
        {
            return format.Name switch
            {
                "JPEG" => ContentType.Jpeg,
                "PNG" => ContentType.Png,
                _ => ContentType.Jpeg
            };
        }
    }

    public struct ProcessedImage
    {
        public string? AssetId { get; set; }
        public string? ThumbnailId { get; set; }
        public string? Format { get; set; }
    }
}
