using System.Collections;
using System.Reflection;
using GBX.NET.Engines.MwFoundations;

namespace GbxCLI;

internal static class FileInspector
{
    internal static bool CanBrowse(object value)
    {
        var type = value.GetType();
        return !(type.IsPrimitive || type.IsEnum || value is string or decimal or DateTime or DateTimeOffset or DateOnly or TimeOnly or TimeSpan or Guid);
    }

    internal static IEnumerable<InspectorMember> GetEntries(object value, bool useStoredReferences = false)
    {
        if (!CanBrowse(value))
        {
            yield break;
        }

        var type = value.GetType();

        if (value is IEnumerable collection)
        {
            if (useStoredReferences && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(GBX.NET.NodeRefList<>))
            {
                collection = (IEnumerable)type.GetMethod("EnumerateReferences")!.Invoke(value, null)!;
            }

            var elementType = type.IsArray ? type.GetElementType() : type.GetInterfaces()
                .FirstOrDefault(candidate => candidate.IsGenericType && candidate.GetGenericTypeDefinition() == typeof(IEnumerable<>))?
                .GetGenericArguments()[0];
            var index = 0;

            foreach (var item in collection)
            {
                yield return new InspectorMember($"[{index++}]", item?.GetType() ?? elementType ?? typeof(object), item);
            }

            yield break;
        }

        var members = new Dictionary<string, MemberInfo>(StringComparer.Ordinal);

        for (var current = type; current is not null; current = current.BaseType)
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

        var referenceView = useStoredReferences && members.TryGetValue("References", out var viewMember)
            && viewMember is PropertyInfo viewProperty ? viewProperty.GetValue(value) : null;

        foreach (var member in members.Values.OrderBy(member => member.Name, StringComparer.Ordinal))
        {
            if (referenceView is not null && member.Name == "References")
            {
                continue;
            }

            // Generated reference views expose stored handles without loading external files.
            var referenceProperty = referenceView?.GetType().GetProperty(member.Name);
            var memberType = member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;

            // Computed aliases can resolve references indirectly, too.
            if (referenceView is not null && referenceProperty is null && member is PropertyInfo computed
                && (typeof(CMwNod).IsAssignableFrom(memberType) || computed.SetMethod is null)
                && member.Name is not ("Chunks" or "GameVersion"))
            {
                yield return new InspectorMember(member.Name, memberType, "(computed; inspect with gbx eval)");
                continue;
            }

            object? memberValue = null;
            string? error = null;

            try
            {
                memberValue = referenceProperty is not null ? referenceProperty.GetValue(referenceView)
                    : member is PropertyInfo propertyInfo ? propertyInfo.GetValue(value) : ((FieldInfo)member).GetValue(value);
            }
            catch (Exception exception)
            {
                error = exception is TargetInvocationException invocation
                    ? invocation.InnerException?.Message ?? invocation.Message : exception.Message;
            }

            yield return new InspectorMember(member.Name, memberType, memberValue, error);
        }
    }
}
