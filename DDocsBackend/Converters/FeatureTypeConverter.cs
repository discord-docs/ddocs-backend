using DDocsBackend.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Converters
{
    internal class FeatureTypeConverter : JsonConverter
    {
        public static FeatureTypeConverter Instance
           => new();

        public override bool CanConvert(Type objectType) => true;

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            return (FeatureType)Enum.Parse(typeof(SummaryType), value!, true);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(value!.ToString()!.ToLower());
        }
    }
}
