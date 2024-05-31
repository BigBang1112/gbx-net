using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using GbxExplorerOld.Client.Models;
using System.Reflection;

namespace GbxExplorerOld.Client.Services;

public class TypeCacheService : ITypeCacheService
{
    private static readonly Dictionary<Type, TypeInfoModel> cache = new();

    public (TypeInfoModel, bool isNullable) GetTypeInfoModel(Type type, PropertyInfo? property)
    {
        if (cache.TryGetValue(type, out var model))
        {
            if (model.IsNullable.HasValue)
            {
                return (model, model.IsNullable.Value);
            }
        }
        else
        {
            var (nullable, actualName, standardName, elementType) = GetTypeNameAndNullability(type);

            var isGeneric = (!elementType.IsValueType || !nullable.GetValueOrDefault()) && elementType.IsGenericType;
            var propTypeName = isGeneric ? elementType.GetGenericTypeDefinition().Name : actualName;
            var genericArguments = isGeneric ? elementType.GetGenericArguments() : null;
            var arrayRank = type.IsArray ? type.GetArrayRank() : default(int?);
            var isNodeType = elementType.IsSubclassOf(typeof(CMwNod));
            var engine = default(string);
            
            if (isNodeType && elementType.Namespace is not null)
            {
                var lastIndexOfDot = elementType.Namespace.LastIndexOf('.');
                engine = elementType.Namespace.Substring(lastIndexOfDot + 1);
            }
            
            model = new TypeInfoModel(type,
                                      elementType,
                                      genericArguments,
                                      actualName,
                                      standardName,
                                      propTypeName,
                                      nullable,
                                      arrayRank,
                                      isNodeType,
                                      engine);

            cache.Add(type, model);
        }

        if (property is not null)
        {
            if (IsMarkedAsNullable(property))
            {
                return (model, isNullable: true);
            }
        }

        return (model, isNullable: false);
    }

    static bool IsMarkedAsNullable(PropertyInfo p)
    {
        return new NullabilityInfoContext().Create(p).ReadState is NullabilityState.Nullable;
    }

    private static (bool? nullable, string actualName, string? standardName, Type elementType) GetTypeNameAndNullability(Type type)
    {
        var nullable = default(bool?);

        if (type.IsValueType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                nullable = true;
                type = type.GetGenericArguments()[0];
            }
            else
            {
                nullable = false;
            }
        }
        else
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();

                if (elementType is not null)
                {
                    type = elementType;
                }
            }
        }

        return (nullable, type.Name, GetStandardNameFromOfficialName(type.Name), type);
    }

    private static string? GetStandardNameFromOfficialName(string typeName) => typeName switch
    {
        nameof(Boolean) => "bool",
        nameof(String) => "string",
        nameof(Char) => "char",
        nameof(SByte) => "sbyte",
        nameof(Byte) => "byte",
        nameof(Int16) => "short",
        nameof(UInt16) => "ushort",
        nameof(Int32) => "int",
        nameof(UInt32) => "uint",
        nameof(Int64) => "long",
        nameof(UInt64) => "ulong",
        nameof(Single) => "float",
        nameof(Double) => "double",
        nameof(Decimal) => "decimal",
        nameof(Object) => "object",

        _ => null
    };
}
