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
        logger.LogInformation("Resolving tool functionality...");

        var ctors = ResolveConstructors(logger);

        ResolveInterfaces(out var produceMethods, out var mutateMethods, out var configType, logger);

        logger.LogInformation("Tool functionality resolved successfully.");

        return new ToolFunctionality<T>
        {
            Constructors = ctors,
            ProduceMethods = produceMethods,
            MutateMethods = mutateMethods,
            ConfigType = configType
        };
    }

    private static ConstructorInfo[] ResolveConstructors(ILogger logger)
    {
        var ctors = typeof(T).GetConstructors();

        if (ctors.Length == 0)
        {
            throw new Exception("No public constructors found.");
        }

        if (logger.IsEnabled(LogLevel.Information))
        {
            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                if (parameters.Length == 0)
                {
                    logger.LogInformation("Default constructor found.");
                    continue;
                }

                logger.LogInformation("Constructor with parameters found: {Parameters}", string.Join(", ", parameters.Select(x => FormatType(x.ParameterType))));
            }

            logger.LogInformation("The most suitable one will be picked to create tool instances.");
        }

        return ctors;
    }

    private static void ResolveInterfaces(
        out MethodInfo[] produceMethods,
        out MethodInfo[] mutateMethods,
        out Type? configType,
        ILogger logger)
    {
        var methods = typeof(T).GetMethods();

        if (logger.IsEnabled(LogLevel.Debug))
        {
            foreach (var method in methods)
            {
                logger.LogDebug("Public tool method: {MethodName}", method.Name);
            }
        }

        var prodMethods = new List<MethodInfo>();
        var mutMethods = new List<MethodInfo>();
        configType = null;

        foreach (var interfaceType in typeof(T).GetInterfaces())
        {
            if (interfaceType == typeof(ITool))
            {
                logger.LogDebug("Tool interface found.");
                continue;
            }

            var genericType = interfaceType.GetGenericTypeDefinition();

            if (genericType == typeof(IConfigurable<>))
            {
                configType = interfaceType.GetGenericArguments()[0];
                logger.LogInformation("Tool is configurable: {ConfigType}", FormatType(configType));
                continue;
            }

            if (genericType == typeof(IProductive<>))
            {
                var producedType = interfaceType.GetGenericArguments()[0];
                var method = methods.Single(m => m.Name == nameof(IProductive<object>.Produce) && m.ReturnType == producedType);
                prodMethods.Add(method);
                logger.LogInformation("Tool produces new data: {ProducedType}", FormatType(producedType));
                continue;
            }

            if (genericType == typeof(IMutative<>))
            {
                var producedType = interfaceType.GetGenericArguments()[0];
                var method = methods.Single(m => m.Name == nameof(IMutative<object>.Mutate) && m.ReturnType == producedType);
                mutMethods.Add(method);
                logger.LogInformation("Tool mutates data: {ProducedType} (in CLI, only mutated in memory)", FormatType(producedType));
                continue;
            }
        }

        produceMethods = prodMethods.ToArray();
        mutateMethods = mutMethods.ToArray();
    }

    private static string FormatType(Type type)
    {
        if (type.IsGenericType)
        {
            var name = type.Name.Substring(0, type.Name.IndexOf('`'));
            var args = type.GetGenericArguments().Select(FormatType);
            return $"{name}<{string.Join(", ", args)}>";
        }

        return type.Name;
    }
}
