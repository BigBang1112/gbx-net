using GBX.NET.Tool.CLI.Exceptions;
using GBX.NET.Tool.CLI.Inputs;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GBX.NET.Tool.CLI;

internal sealed class ToolInstanceMaker<T> where T : ITool
{
    private readonly ToolFunctionality<T> toolFunctionality;
    private readonly ToolConfiguration toolConfig;
    private readonly ILogger logger;

    private readonly Dictionary<Input, object?> resolvedInputs = [];
    private readonly HashSet<object> usedObjects = [];
    private readonly List<object> unprocessedObjects = [];

    public ToolInstanceMaker(ToolFunctionality<T> toolFunctionality, ToolConfiguration toolConfig, ILogger logger)
    {
        this.toolFunctionality = toolFunctionality;
        this.toolConfig = toolConfig;
        this.logger = logger;
    }

    public async IAsyncEnumerable<T> MakeToolInstances()
    {
        do
        {
            var paramsForCtor = new List<object>();
            var pickedCtor = default(ConstructorInfo);

            foreach (var constructor in toolFunctionality.Constructors)
            {
                paramsForCtor = await TryPickConstructorAsync(constructor);

                if (paramsForCtor.Count > 0)
                {
                    pickedCtor = constructor;
                    break;
                }
            }

            if (pickedCtor is null)
            {
                throw new ConsoleProblemException("Invalid files passed to the tool.");
            }

            // Instantiate the tool
            yield return (T)pickedCtor.Invoke(paramsForCtor.ToArray());
        }
        while (unprocessedObjects.Count > 0);
    }

    private async Task<List<object>> TryPickConstructorAsync(ConstructorInfo constructor)
    {
        var paramsForCtor = new List<object>();

        var invalidCtor = false;
        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            // inputless tools?
            return paramsForCtor;
        }

        foreach (var parameter in parameters)
        {
            var type = parameter.ParameterType;

            if (type == typeof(ILogger))
            {
                paramsForCtor.Add(logger);
                continue;
            }

            if (type == typeof(Gbx))
            {
                var paramObj = await GetParameterObjectAsync(obj => obj is Gbx);
                paramsForCtor.Add(paramObj);
                continue;
            }

            if (!type.IsGenericType)
            {
                continue;
            }

            var typeDef = type.GetGenericTypeDefinition();

            if (typeDef == typeof(Gbx<>))
            {
                var nodeType = type.GetGenericArguments()[0];
                var paramObj = await GetParameterObjectAsync(obj => obj is Gbx gbx && gbx.Node?.GetType() == nodeType);
                paramsForCtor.Add(paramObj);
                continue;
            }

            if (typeDef == typeof(IEnumerable<>))
            {
                var elementType = type.GetGenericArguments()[0];
                continue;
            }
        }

        if (invalidCtor)
        {
            return [];
        }

        return paramsForCtor;
    }

    private async Task<object> GetParameterObjectAsync(Predicate<object> predicate)
    {
        // if there are leftover objects from previous instance
        if (unprocessedObjects.Count > 0)
        {
            var obj = unprocessedObjects[0];

            // check if the leftover fits this parameter
            if (predicate(obj))
            {
                usedObjects.Add(obj);
                unprocessedObjects.RemoveAt(0);
                return obj;
            }
        }

        var paramForCtor = default(object);

        foreach (var input in toolConfig.Inputs)
        {
            if (!resolvedInputs.TryGetValue(input, out var resolvedObject))
            {
                resolvedObject = await input.ResolveAsync(default);
                resolvedInputs.Add(input, resolvedObject);
            }

            if (resolvedObject is null)
            {
                continue;
            }

            if (resolvedObject is IEnumerable<object> enumerable)
            {
                foreach (var obj in enumerable)
                {
                    if (obj is null || usedObjects.Contains(obj) || obj is IEnumerable<object>)
                    {
                        continue;
                    }

                    if (predicate(obj))
                    {
                        if (paramForCtor is not null)
                        {
                            unprocessedObjects.Add(obj);
                            continue;
                        }

                        paramForCtor = obj;
                        usedObjects.Add(obj);
                    }
                }

                continue;
            }

            if (usedObjects.Contains(resolvedObject))
            {
                continue;
            }

            if (predicate(resolvedObject))
            {
                if (paramForCtor is not null)
                {
                    unprocessedObjects.Add(resolvedObject);
                    continue;
                }

                paramForCtor = resolvedObject;
                usedObjects.Add(resolvedObject);
            }
        }

        if (paramForCtor is null)
        {
            throw new Exception("Invalid file passed to the tool.");
        }

        return paramForCtor;
    }
}
