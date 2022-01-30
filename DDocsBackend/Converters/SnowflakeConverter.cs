using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Converters
{
    internal class SnowflakeConverter : JsonConverter
    {
        public static SnowflakeConverter Instance
            => new();

        public override bool CanConvert(Type objectType) => true;
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return ulong.Parse((string)reader.Value!, NumberStyles.None, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(((ulong)value!).ToString(CultureInfo.InvariantCulture));
        }
    }
}
