using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Data.Models
{
    public class Asset
    {
        [Key]
        public string? Id { get; set; }
        public AssetType Type { get; set; }
        public ContentType ContentType { get; set; }
        public virtual Asset? Thumbnail { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum AssetType
    {
        Avatar,
        Content,
        Thumbnail,
    }

    public enum ContentType : byte
    {
        Png,
        Jpeg,
    }
}
