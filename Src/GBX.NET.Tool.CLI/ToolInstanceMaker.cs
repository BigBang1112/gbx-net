using GBX.NET.Tool.CLI.Exceptions;
using GBX.NET.Tool.CLI.Inputs;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

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

    [RequiresDynamicCode(DynamicCodeMessages.MakeGenericTypeMessage)]
    public async IAsyncEnumerable<T> MakeToolInstancesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        // Each loop iteration defines tool instance
        do
        {
            var paramsForCtor = Array.Empty<object>();
            var pickedCtor = default(ConstructorInfo);

            // Tries to pick the FIRST valid constructor
            foreach (var constructor in toolFunctionality.Constructors)
            {
                // Tests the constructor with input values
                // If successful, returns the array of parameters to provide to it
                // The array can be empty if parameterless constructor was picked
                // If not, null is returned
                paramsForCtor = await TryPickConstructorAsync(constructor, cancellationToken);

                if (paramsForCtor is not null)
                {
                    pickedCtor = constructor;
                    break;
                }
            }

            // If no valid constructor was found
            if (pickedCtor is null)
            {
                throw new ConsoleProblemException("Invalid files passed to the tool.");
            }

            // Instantiate the tool
            yield return (T)pickedCtor.Invoke(paramsForCtor);
        }
        while (unprocessedObjects.Count > 0);
        // Continue creating more tool instances if there are unused resolved objects left
    }

    [RequiresDynamicCode(DynamicCodeMessages.MakeGenericTypeMessage)]
    private async Task<object[]?> TryPickConstructorAsync(ConstructorInfo constructor, CancellationToken cancellationToken)
    {
        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            // inputless tools?
            return [];
        }

        var paramsForCtor = new object[parameters.Length];
        var index = 0;

        foreach (var parameter in parameters)
        {
            var type = parameter.ParameterType;

            // Non-generic types

            if (type == typeof(ILogger))
            {
                paramsForCtor[index++] = logger;
                continue;
            }

            if (type == typeof(Gbx))
            {
                // Retrieve the next unused resolved object of any Gbx type
                // Null is returned if there's no match
                var paramObj = await GetParameterObjectAsync(obj => obj is Gbx, cancellationToken);

                if (paramObj is null)
                {
                    // Constructor is not valid for this configuration
                    return null;
                }

                paramsForCtor[index++] = paramObj;
                continue;
            }

            if (!type.IsGenericType)
            {
                continue;
            }

            // Generic types

            var typeDef = type.GetGenericTypeDefinition();

            if (typeDef == typeof(Gbx<>))
            {
                var nodeType = type.GetGenericArguments()[0];

                // Retrieve the next unused resolved object of Gbx with this generic arg type (no covariance)
                // Null is returned if there's no match
                var paramObj = await GetParameterObjectAsync(obj => obj is Gbx gbx && gbx.Node?.GetType() == nodeType, cancellationToken);

                if (paramObj is null)
                {
                    // Constructor is not valid for this configuration
                    return null;
                }

                paramsForCtor[index++] = paramObj;
                continue;
            }

            // Currently only generic IEnumerable<> is recognized from collections
            if (typeDef == typeof(IEnumerable<>))
            {
                var elementType = type.GetGenericArguments()[0];

                // Create a List<elementType> to populate and pass to constructor
                // This cannot be handled properly in NativeAOT complication
                var finalCollection = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;

                // Maybe unprocessedObjects logic missing here?
                // Could break if there's single Gbx param and then an IEnumerable<Gbx> afterwards

                // Resolve inputs into individual objects, iterated one by one (no collection should appear here)
                await foreach (var resolvedObject in EnumerateResolvedObjectsAsync(cancellationToken))
                {
                    // objects that were already sent to parameters should not be provided again in next instances
                    if (usedObjects.Contains(resolvedObject))
                    {
                        continue;
                    }

                    // Only objects that match the exact element type will be counted (for now)
                    if (resolvedObject.GetType() == elementType)
                    {
                        // Object is added to the list that will be passed to the constructor
                        finalCollection.Add(resolvedObject);
                        usedObjects.Add(resolvedObject);
                    }
                    else
                    {
                        // Objects that don't match the type are saved for later checks
                        unprocessedObjects.Add(resolvedObject);
                    }
                }

                paramsForCtor[index++] = finalCollection;

                continue;
            }
        }

        return paramsForCtor;
    }

    private async Task<object?> GetParameterObjectAsync(Predicate<object> predicate, CancellationToken cancellationToken)
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

        // Resolve inputs into individual objects, iterated one by one (no collection should appear here)
        await foreach (var resolvedObject in EnumerateResolvedObjectsAsync(cancellationToken))
        {
            // objects that were already sent to parameters should not be provided again in next instances
            if (usedObjects.Contains(resolvedObject))
            {
                continue;
            }

            // Only objects that match the predicate will be used for the parameter
            // It doesn't need to know the ctor parameter type, but the object HAS to be implicitly castable to the ctor type
            if (predicate(resolvedObject))
            {
                // If there's already a parameter object, save the new one for later checks
                if (paramForCtor is not null)
                {
                    unprocessedObjects.Add(resolvedObject);
                    continue;
                }

                paramForCtor = resolvedObject;
                usedObjects.Add(resolvedObject);
            }
        }

        return paramForCtor;
    }

    private async IAsyncEnumerable<object> EnumerateResolvedObjectsAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var input in toolConfig.Inputs)
        {
            // Resolve objects from input and cache them
            if (!resolvedInputs.TryGetValue(input, out var resolvedObject))
            {
                resolvedObject = await input.ResolveAsync(cancellationToken);
                resolvedInputs.Add(input, resolvedObject);
            }

            // Skip unresolved inputs
            if (resolvedObject is null)
            {
                continue;
            }

            // If resolved object is a collection, iterate through it immediately
            // Collection in a collection is not supported
            if (resolvedObject is IEnumerable<object> enumerable)
            {
                foreach (var obj in enumerable)
                {
                    if (obj is IEnumerable<object>)
                    {
                        continue;
                    }

                    yield return obj;
                }
            }
            else
            {
                yield return resolvedObject;
            }
        }
    }
}
