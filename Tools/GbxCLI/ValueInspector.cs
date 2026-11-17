using GBX.NET;
using GBX.NET.Attributes;
using GBX.NET.Engines.MwFoundations;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GbxCLI;

/// <summary>
/// Reads the members of any value through reflection, for <c>inspect</c> and the browser.
/// </summary>
internal static class ValueInspector
{
    private const string RootedJustification = "GBX.NET, GBX.NET.PAK and TmEssentials are rooted with TrimmerRootAssembly, so the metadata of their members is kept.";

    /// <summary>
    /// Whether the value is written on one line, without members.
    /// </summary>
    public static bool IsScalar(object value) => value is string or byte[] || value.GetType().IsValueType;

    /// <summary>
    /// Gets the items of a collection, or the public properties and fields of another object, sorted by name.
    /// </summary>
    /// <param name="useStoredReferences">
    /// List node references as they are stored, so that external nodes are not loaded from other files.
    /// </param>
    public static IEnumerable<InspectorMember> GetMembers(object value, bool useStoredReferences = true)
    {
        if (IsScalar(value))
        {
            return [];
        }

        if (value is IEnumerable collection)
        {
            return GetItems(useStoredReferences && value is INodeRefList list ? list.EnumerateReferences() : collection);
        }

        return GetObjectMembers(value, useStoredReferences);
    }

    private static IEnumerable<InspectorMember> GetItems(IEnumerable collection)
    {
        var index = 0;

        foreach (var item in collection)
        {
            yield return new InspectorMember($"[{index++}]", item);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = RootedJustification)]
    private static IEnumerable<InspectorMember> GetObjectMembers(object value, bool useStoredReferences)
    {
        var members = new Dictionary<string, MemberInfo>(StringComparer.Ordinal);

        for (var current = value.GetType(); current is not null; current = current.BaseType)
        {
            foreach (var property in current.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                if (property.GetMethod?.IsPublic == true && property.GetIndexParameters().Length == 0)
                {
                    members.TryAdd(property.Name, property);
                }
            }

            foreach (var field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                members.TryAdd(field.Name, field);
            }
        }

        // Generated reference views expose stored handles without loading external files.
        var referenceView = useStoredReferences && members.TryGetValue("References", out var viewMember) && viewMember is PropertyInfo viewProperty
            ? viewProperty.GetValue(value)
            : null;

        foreach (var member in members.Values.OrderBy(member => member.Name, StringComparer.Ordinal))
        {
            if (referenceView is not null && member.Name == "References")
            {
                continue;
            }

            var referenceProperty = referenceView?.GetType().GetProperty(member.Name);
            var memberType = member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;

            // Computed aliases can resolve references indirectly, too.
            if (referenceView is not null && referenceProperty is null && member is PropertyInfo computed
                && (typeof(CMwNod).IsAssignableFrom(memberType) || computed.SetMethod is null)
                && member.Name is not ("Chunks" or "GameVersion"))
            {
                yield return new InspectorMember(member.Name, "(computed; inspect with gbx eval)");
                continue;
            }

            object? memberValue = null;
            string? error = null;

            try
            {
                memberValue = referenceProperty is not null ? referenceProperty.GetValue(referenceView)
                    : member is PropertyInfo propertyInfo ? propertyInfo.GetValue(value)
                    : ((FieldInfo)member).GetValue(value);
            }
            catch (Exception exception)
            {
                error = exception is TargetInvocationException { InnerException: { } inner } ? inner.Message : exception.Message;
            }

            yield return new InspectorMember(member.Name, memberValue, error,
                IsFormatted: member.IsDefined(typeof(SupportsFormattingAttribute), inherit: true));
        }
    }
}
