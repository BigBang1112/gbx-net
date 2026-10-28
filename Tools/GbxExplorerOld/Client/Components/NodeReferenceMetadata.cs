using GBX.NET;
using GBX.NET.Engines.MwFoundations;
using System.Collections;
using System.Reflection;

namespace GbxExplorerOld.Client.Components;

internal static class NodeReferenceMetadata
{
    public static NodeRef? GetReference(object? owner, PropertyInfo? property)
    {
        if (owner is null || property is null || !typeof(CMwNod).IsAssignableFrom(property.PropertyType))
        {
            return null;
        }

        for (var type = property.DeclaringType; type is not null; type = type.BaseType)
        {
            var viewProperty = type.GetProperty("References", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (viewProperty is null)
            {
                continue;
            }

            var view = viewProperty.GetValue(owner);
            return view?.GetType().GetProperty(property.Name)?.GetValue(view) as NodeRef;
        }

        return null;
    }

    public static IEnumerable GetItems(object value)
    {
        var type = value.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(NodeRefList<>))
        {
            return (IEnumerable)type.GetMethod(nameof(NodeRefList<CMwNod>.EnumerateReferences))!.Invoke(value, null)!;
        }

        return (IEnumerable)value;
    }

    public static Type GetNodeType(NodeRef reference) => reference.GetType().GetGenericArguments()[0];
}
