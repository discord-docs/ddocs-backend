using DDocsBackend.Data;
using DDocsBackend.Data.Models;
using DDocsBackend.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Services
{
    public struct ReadResult : IDisposable
    {  
        public Image Image { get; }
        public ContentType Type { get; }

        public ReadResult(Image img, ContentType type)
        {
            Image = img;
            Type = type;
        }

        public void Dispose()
        {
            Image.Dispose();
            Configuration.Default.MemoryAllocator.ReleaseRetainedResources();
        }
    }

    public class CDNService
    {
        private readonly Logger _log;

#if DEBUG
        public const string AssetDirectory = "./assets";
        public const string AvatarsDirectory = "./assets/avatars";
        public const string SiteContentDirectory = "./assets/content";
        public const string ThumbnailDirectory = "./assets/content/thumbnails";
#else
        public const string AssetDirectory = "/assets";
        public const string AvatarsDirectory = "/assets/avatars";
        public const string SiteContentDirectory = "/assets/content";
        public const string ThumbnailDirectory = "/assets/content/thumbnails";
#endif

        private readonly DataAccessLayer _dataAccessLayer;

        public CDNService(DataAccessLayer dataAccessLayer)
        {
            _log = Logger.GetLogger<CDNService>();

            Configuration.Default.MemoryAllocator = ArrayPoolMemoryAllocator.CreateWithMinimalPooling();

            _dataAccessLayer = dataAccessLayer;

            if (!Directory.Exists(AssetDirectory))
                Directory.CreateDirectory(AssetDirectory);
            if (!Directory.Exists(AvatarsDirectory))
                Directory.CreateDirectory(AvatarsDirectory);
            if (!Directory.Exists(SiteContentDirectory))
                Directory.CreateDirectory(SiteContentDirectory);
            if (!Directory.Exists(ThumbnailDirectory))
                Directory.CreateDirectory(ThumbnailDirectory);
        }

        public string? GetAssetHash(string id, AssetType type)
        {
            if(!AssetExists(id, type))
                return null;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(GetAssetDirectory(type) + $"/{id}"))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public bool AssetExists(string id, AssetType type)
        {
            var dir = GetAssetDirectory(type);

            return File.Exists(dir + $"/{id}");
        }

        public async Task<ReadResult?> ReadAsync(string id, AssetType type)
        {
            var dir = GetAssetDirectory(type);

            if (!File.Exists(dir + $"/{id}"))
            {
                _log.Debug($"{Logger.BuildColoredString("Asset not found", ConsoleColor.Red)}: [{type}] {dir + $"/{id}"}", Severity.CDN);
                return null;
            }


            (Image Image, IImageFormat Format) image;

            using(var fs = File.OpenRead(dir + $"/{id}"))
            {
                image = await Image.LoadWithFormatAsync(fs).ConfigureAwait(false);
            }

            var contentType = ImageHelper.GetContentType(image.Format);

            _log.Debug($"Asset {dir + $"/{id}"} loaded", Severity.CDN);

            return new ReadResult(image.Image, contentType);
        }

        public async Task WriteAsync(string id, AssetType type, ContentType contentType, Image image)
        {
            var dir = GetAssetDirectory(type);

            switch (contentType)
            {
                case ContentType.Jpeg:
                    await image.SaveAsJpegAsync(dir + $"/{id}").ConfigureAwait(false);
                    _log.Debug($"Asset saved: {dir + $"/{id}"}", Severity.CDN);
                    break;

                case ContentType.Png:
                    await image.SaveAsPngAsync(dir + $"/{id}").ConfigureAwait(false);
                    _log.Debug($"Asset saved: {dir + $"/{id}"}", Severity.CDN);
                    break;
            }
        }

        private string GetAssetDirectory(AssetType type)
        {
            return type switch
            {
                AssetType.Avatar => AvatarsDirectory,
                AssetType.Content => SiteContentDirectory,
                AssetType.Thumbnail => ThumbnailDirectory,
                _ => AssetDirectory
            };
        }
    }
}
