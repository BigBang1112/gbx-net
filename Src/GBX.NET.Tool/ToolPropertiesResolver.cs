using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool;

/// <summary>
/// Resolves tool functionality to use in the tool console.
/// </summary>
public sealed class ToolPropertiesResolver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicConstructors)] T> where T : ITool
{
    public static ToolProperties<T> Resolve(ILogger logger)
    {
        logger.LogInformation("Resolving tool properties...");

        var type = typeof(T);

        foreach (var ctor in type.GetConstructors())
        {
            if (ctor.GetParameters().Length == 0)
            {
                continue;
            }

            // Check for constructor parameters
            var parameters = ctor.GetParameters();
        }

        foreach (var interfaceType in type.GetInterfaces())
        {
            if (interfaceType == typeof(ITool))
            {
                continue;
            }

            var genericType = interfaceType.GetGenericTypeDefinition();

            if (genericType == typeof(IConfigurable<>))
            {
                var configType = interfaceType.GetGenericArguments()[0];
                continue;
            }

            if (genericType == typeof(IProductive<>))
            {
                var producedType = interfaceType.GetGenericArguments()[0];
                continue;
            }

            if (genericType == typeof(IMutative<>))
            {
                var producedType = interfaceType.GetGenericArguments()[0];
                continue;
            }
        }

        logger.LogInformation("Tool properties resolved successfully.");

        return new ToolProperties<T>();
    }
}
