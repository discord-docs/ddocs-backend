using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Converters
{
    internal class GuidConverter : JsonConverter
    {
        public static GuidConverter Instance
            => new();

        public override bool CanConvert(Type objectType) => true;
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return Guid.Parse((string)reader.Value!);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue((value as Guid?)?.ToString("N"));
        }
    }
}
