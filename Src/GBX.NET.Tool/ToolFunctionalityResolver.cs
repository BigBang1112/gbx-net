using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GBX.NET.Tool;

/// <summary>
/// Resolves tool functionality to use in the tool console.
/// </summary>
public sealed class ToolFunctionalityResolver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods)] T> where T : ITool
{
    public static ToolFunctionality<T> Resolve(ILogger logger)
    {
        logger.LogInformation("Resolving tool properties...");

        var type = typeof(T);

        ResolveInterfaces(type, out var produceMethods, out var mutateMethods, out var configType);

        logger.LogInformation("Tool properties resolved successfully.");

        return new ToolFunctionality<T>
        {
            Constructors = type.GetConstructors(),
            ProduceMethods = produceMethods,
            MutateMethods = mutateMethods,
            ConfigType = configType
        };
    }

    private static void ResolveInterfaces([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicMethods)] Type type,
        out MethodInfo[] produceMethods,
        out MethodInfo[] mutateMethods,
        out Type? configType)
    {
        var methods = type.GetMethods();
        var prodMethods = new List<MethodInfo>();
        var mutMethods = new List<MethodInfo>();
        configType = null;

        foreach (var interfaceType in type.GetInterfaces())
        {
            if (interfaceType == typeof(ITool))
            {
                continue;
            }

            var genericType = interfaceType.GetGenericTypeDefinition();

            if (genericType == typeof(IConfigurable<>))
            {
                configType = interfaceType.GetGenericArguments()[0];
                continue;
            }

            if (genericType == typeof(IProductive<>))
            {
                var producedType = interfaceType.GetGenericArguments()[0];
                var method = methods.Single(m => m.Name == nameof(IProductive<object>.Produce) && m.ReturnType == producedType);
                prodMethods.Add(method);
                continue;
            }

            if (genericType == typeof(IMutative<>))
            {
                var producedType = interfaceType.GetGenericArguments()[0];
                var method = methods.Single(m => m.Name == nameof(IMutative<object>.Mutate) && m.ReturnType == producedType);
                prodMethods.Add(method);
                continue;
            }
        }

        produceMethods = prodMethods.ToArray();
        mutateMethods = mutMethods.ToArray();
    }
}
