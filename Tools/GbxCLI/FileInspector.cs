using System.Collections;
using System.Reflection;

namespace GbxCLI;

internal static class FileInspector
{
    internal static bool CanBrowse(object value)
    {
        var type = value.GetType();
        return !(type.IsPrimitive || type.IsEnum || value is string or decimal or DateTime or DateTimeOffset or DateOnly or TimeOnly or TimeSpan or Guid);
    }

    internal static IEnumerable<InspectorMember> GetEntries(object value)
    {
        if (!CanBrowse(value))
        {
            yield break;
        }

        var type = value.GetType();

        if (value is IEnumerable collection)
        {
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

        foreach (var member in members.Values.OrderBy(member => member.Name, StringComparer.Ordinal))
        {
            var memberType = member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;
            object? memberValue = null;
            string? error = null;

            try
            {
                memberValue = member is PropertyInfo propertyInfo ? propertyInfo.GetValue(value) : ((FieldInfo)member).GetValue(value);
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
