using GbxExplorerOld.Client.Attributes;
using GbxExplorerOld.Client.Components.ValueRenderers;
using System.Reflection;

namespace GbxExplorerOld.Client;

public static class ComponentPerType
{
    public static Type GetType<T>(Type? type, ref Dictionary<Type, Type>? cache, Type? enumType)
    {
        if (type is null)
        {
            return typeof(T);
        }

        if (type.IsGenericType && type.Name != "KeyValuePair`2")
        {
            var genericType = type.GetGenericTypeDefinition();

            type = genericType == typeof(Nullable<>)
                ? type.GetGenericArguments()[0]
                : genericType;
        }

        if (enumType is not null && type.IsEnum)
        {
            return enumType;
        }

        cache ??= CacheComponents<T>();

        var tempType = type;

        while (tempType.BaseType is not null)
        {
            if (cache.TryGetValue(tempType, out Type? element))
            {
                return element;
            }

            tempType = tempType.BaseType;
        }

        var interfaces = type.GetInterfaces();

        for (var i = 0; i < interfaces.Length; i++)
        {
            var inter = interfaces[i];

            if (inter.IsGenericType)
            {
                inter = inter.GetGenericTypeDefinition();
            }

            if (cache.TryGetValue(inter, out Type? element))
            {
                return element;
            }
        }

        return typeof(T);
    }

    private static Dictionary<Type, Type> CacheComponents<T>()
    {
        var dict = new Dictionary<Type, Type>();

        foreach (var type in Assembly.GetExecutingAssembly().ExportedTypes)
        {
            if (!type.IsSubclassOf(typeof(T)))
            {
                continue;
            }

            var att = type.GetCustomAttribute<AppliesForTypeAttribute>();

            if (att is null)
            {
                continue;
            }

            foreach (var attType in att.Types)
            {
                dict.Add(attType, type);
            }
        }

        return dict;
    }
}
