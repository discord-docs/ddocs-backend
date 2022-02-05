using DDocsBackend.Data.Models;
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
        private static readonly MethodInfo? _shouldSerialize = typeof(DDocsContractResolver).GetTypeInfo().GetDeclaredMethod("ShouldSerialize");

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

                if (depth == 0 && genericType == typeof(Optional<>))
                {
                    var typeInput = info.DeclaringType;
                    var innerTypeOutput = type.GenericTypeArguments[0];

                    var getter = typeof(Func<,>).MakeGenericType(typeInput!, type);
                    var getterDelegate = info.GetMethod?.CreateDelegate(getter);
                    var shouldSerialize = _shouldSerialize?.MakeGenericMethod(typeInput!, innerTypeOutput);
                    var shouldSerializeDelegate = (Func<object, Delegate, bool>)shouldSerialize!.CreateDelegate(typeof(Func<object, Delegate, bool>));
                    property.ShouldSerialize = x => shouldSerializeDelegate(x, getterDelegate!);

                    return MakeGenericConverter(property, info, typeof(OptionalConverter<>), innerTypeOutput, depth);
                }
                else if (genericType == typeof(Nullable<>))
                    return MakeGenericConverter(property, info, typeof(NullableConverter<>), type.GenericTypeArguments[0], depth);
            }

            if (type == typeof(ulong))
                return SnowflakeConverter.Instance;

            if (type == typeof(SummaryType))
                return SummaryTypeConverter.Instance;

            if (type == typeof(FeatureType))
                return FeatureTypeConverter.Instance;

            if (type == typeof(Guid))
                return GuidConverter.Instance;

            return null;
        }

        private static bool ShouldSerialize<TOwner, TValue>(object? owner, Delegate? getter)
        {
            return ((Func<TOwner?, Optional<TValue?>>)getter!)((TOwner?)owner).IsSpecified;
        }

        private static JsonConverter? MakeGenericConverter(JsonProperty property, PropertyInfo info, Type converterType, Type innerType, int depth)
        {
            var genericType = converterType.MakeGenericType(innerType).GetTypeInfo();
            var innerConverter = GetConverter(property, info, innerType, depth + 1)!;
            return genericType.DeclaredConstructors.First().Invoke(new object[] { innerConverter }) as JsonConverter;
        }
    }
}
