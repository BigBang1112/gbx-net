using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET.Tool;

/// <summary>
/// Resolves tool functionality to use in the tool console.
/// </summary>
public sealed class ToolFunctionalityResolver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicConstructors)] T> where T : ITool
{
    public static ToolFunctionality<T> Resolve(ILogger logger)
    {
        logger.LogInformation("Resolving tool properties...");

        var type = typeof(T);

        ResolveInterfaces(type);

        logger.LogInformation("Tool properties resolved successfully.");

        return new ToolFunctionality<T>
        {
            Constructors = type.GetConstructors()
        };
    }

    private static void ResolveInterfaces(Type type)
    {
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
    }
}
