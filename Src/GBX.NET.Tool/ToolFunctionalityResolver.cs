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

        var paramObjects = ResolveConstructors(type, logger);

        logger.LogInformation("Tool properties resolved successfully.");

        return new ToolFunctionality<T>
        {
            InputParameters = paramObjects
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

    private static object?[] ResolveConstructors(Type type, ILogger logger)
    {
        foreach (var ctor in type.GetConstructors())
        {
            var parameters = ctor.GetParameters();

            if (parameters.Length == 0)
            {
                // input-less tools?
                continue;
            }

            var ctorUsable = true;
            var paramObjects = new object?[parameters.Length];

            // Check for constructor parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (parameter.ParameterType == typeof(ILogger))
                {
                    paramObjects[i] = logger;
                    continue;
                }

                ctorUsable = false;
                break;
            }

            if (ctorUsable)
            {
                return paramObjects;
            }
        }

        return [];
    }
}
