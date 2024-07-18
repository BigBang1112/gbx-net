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
        var pickedCtor = default(ConstructorInfo);

        while (true)
        {
            var paramsForCtor = new List<object>();

            foreach (var constructor in toolFunctionality.Constructors)
            {
                var invalidCtor = false;
                var parameters = constructor.GetParameters();

                if (parameters.Length == 0)
                {
                    // inputless tools?
                    continue;
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

                        var parameterUsed = false;

                        // if there are leftover objects from previous instance
                        if (unprocessedObjects.Count > 0)
                        {
                            var obj = unprocessedObjects[0];

                            if (obj is Gbx gbx && gbx.Node?.GetType() == nodeType)
                            {
                                paramsForCtor.Add(obj);
                                usedObjects.Add(obj);
                                parameterUsed = true;

                                unprocessedObjects.RemoveAt(0);
                                continue;
                            }
                        }

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

                                    if (obj is Gbx gbxx && gbxx.Node?.GetType() == nodeType)
                                    {
                                        if (parameterUsed)
                                        {
                                            unprocessedObjects.Add(gbxx);
                                            continue;
                                        }

                                        paramsForCtor.Add(gbxx);
                                        usedObjects.Add(gbxx);
                                        parameterUsed = true;
                                    }
                                }

                                continue;
                            }

                            if (usedObjects.Contains(resolvedObject))
                            {
                                continue;
                            }

                            if (resolvedObject is Gbx gbx && gbx.Node?.GetType() == nodeType)
                            {
                                if (parameterUsed)
                                {
                                    unprocessedObjects.Add(gbx);
                                    continue;
                                }

                                paramsForCtor.Add(gbx);
                                usedObjects.Add(gbx);
                                parameterUsed = true;
                            }
                        }

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
                    paramsForCtor.Clear();
                }
                else
                {
                    pickedCtor = constructor;
                    break;
                }
            }

            // Instantiate the tool
            if (pickedCtor is null)
            {
                throw new ConsoleProblemException("Invalid files passed to the tool.");
            }

            yield return (T)pickedCtor.Invoke(paramsForCtor.ToArray());

            // Run all produce methods in parallel and run mutate methods in sequence
            if (unprocessedObjects.Count == 0)
            {
                break;
            }
        }
    }
}
