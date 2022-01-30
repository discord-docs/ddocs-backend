using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DDocsBackend.Converters
{
    public class DDocsContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.Ignored)
                return property;

            if (member is PropertyInfo propInfo)
            {
                var converter = GetConverter(property, propInfo, propInfo.PropertyType, 0);

                if (converter != null)
                    property.Converter = converter;
            }
            else
                throw new InvalidOperationException($"{member.DeclaringType?.FullName}.{member.Name} is not a property.");

            return property;
        }

        private static JsonConverter? GetConverter(JsonProperty property, PropertyInfo info, Type type, int depth)
        {
            if (type.IsArray)
                return MakeGenericConverter(property, info, typeof(ArrayConverter<>), type.GetElementType()!, depth);

            if (type.IsConstructedGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();

                if (genericType == typeof(Nullable<>))
                    return MakeGenericConverter(property, info, typeof(NullableConverter<>), type.GenericTypeArguments[0], depth);
            }

            if (type == typeof(ulong))
                return SnowflakeConverter.Instance;

            return null;
        }

        private static JsonConverter? MakeGenericConverter(JsonProperty property, PropertyInfo info, Type converterType, Type innerType, int depth)
        {
            var genericType = converterType.MakeGenericType(innerType).GetTypeInfo();
            var innerConverter = GetConverter(property, info, innerType, depth + 1)!;
            return genericType.DeclaredConstructors.First().Invoke(new object[] { innerConverter }) as JsonConverter;
        }
    }
}
