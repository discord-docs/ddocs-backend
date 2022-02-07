using DDocsBackend.Data.Models;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend
{
    public static class ContentTypeExtensions
    {
        public static string ToHTTPContentType(this ContentType type)
        {
            return type switch
            {
                ContentType.Jpeg => "image/jpeg",
                ContentType.Png => "image/png",
                _ => ""
            };
        }

        public static IImageEncoder GetEncoder(this ContentType type)
        {
            return type switch
            {
                ContentType.Jpeg => new JpegEncoder(),
                ContentType.Png => new PngEncoder(),
                _ => new JpegEncoder(),
            };
        }
    }
}
