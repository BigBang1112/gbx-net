using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GBX.NET;

public static class Lzo
{
    private const string TrimWarningAlways = "The LZO type is dynamically loaded.";
    internal const string TrimWarningIfDynamic = "The LZO type could be dynamically loaded if Lzo.SetLzo() was not called beforehand. If Lzo.SetLzo() was called, you can globally suppress this warning.";

    private const string expectedClassName = "MiniLZO";
    private const string expectedCompressMethodName = "Compress";
    private const string expectedDecompressMethodName = "Decompress";
    private const string expectedAttributeName = "LZOforGBX.NET";
    private static bool checkedForLzo;
    
#if NET6_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
#endif
    private static Type? predefinedLzoType;
    
    private static MethodInfo? methodLzoCompress;
    private static MethodInfo? methodLzoDecompress;

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningIfDynamic)]
#endif
    public static void Decompress(byte[] input, byte[] output)
    {
        if (input.Length > 0x10000000)
        {
            throw new LengthLimitException(input.Length);
        }
        
        if (output.Length > 0x10000000)
        {
            throw new LengthLimitException(output.Length);
        }

        CheckForLzo();
        methodLzoDecompress!.Invoke(null, new object[] { input, output });
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningIfDynamic)]
#endif
    public static byte[] Compress(byte[] data)
    {
        if (data.Length > 0x10000000)
        {
            throw new LengthLimitException(data.Length);
        }

        CheckForLzo();
        return (byte[])methodLzoCompress!.Invoke(null, new object[] { data })!;
    }

    /// <exception cref="MissingLzoException"></exception>

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningIfDynamic)]
#endif
    private static void CheckForLzo()
    {
        if (checkedForLzo) return;

        var lzoFound = CheckForLzoFromPredefinedType();

        if (!lzoFound)
        {
            FindLzoInCurrentDomain(ref lzoFound);
        }

        if (!lzoFound)
        {
            throw new MissingLzoException();
        }

        checkedForLzo = true;
    }

    public static void SetLzo<
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
#endif
        T>()
    {
        SetLzo(typeof(T));
    }

    public static void SetLzo(
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
#endif
        Type type)
    {
        predefinedLzoType = type;
    }
    
    private static bool CheckForLzoFromPredefinedType()
    {
        if (predefinedLzoType is null)
        {
            return false;
        }

        return CheckForLzoFromType(predefinedLzoType);
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningAlways)]
#endif
    private static void FindLzoInCurrentDomain(ref bool lzoFound)
    {
        foreach (var dllFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
        {
            lzoFound = AnalyzeDllFile(dllFile);

            if (lzoFound)
            {
                break;
            }
        }
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningAlways)]
#endif
    private static bool AnalyzeDllFile(string dllFile)
    {
        var assembly = Assembly.LoadFrom(dllFile); // TODO: also support System.Reflection.Metadata

        var attributes = assembly.GetCustomAttributesData();

        foreach (var attribute in attributes)
        {
            var verified = VerifyAttribute(assembly, attribute);

            if (verified)
            {
                return true;
            }
        }

        return false;
    }
    
#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningAlways)]
#endif
    private static bool VerifyAttribute(Assembly assembly, CustomAttributeData attribute)
    {
        if (attribute.ConstructorArguments.Count != 2)
            return false;

        var expectedAttribute = new[] { expectedAttributeName, "true" };

        var hasExpectedAttribute = attribute.ConstructorArguments
            .Select(x => x.Value as string)
            .Where(x => x is not null)
            .SequenceEqual(expectedAttribute);

        if (hasExpectedAttribute)
        {
            var lzoFound = CheckForLzoFromAssembly(assembly);

            if (lzoFound)
            {
                return true;
            }
        }

        return false;
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode(TrimWarningAlways)]
#endif
    private static bool CheckForLzoFromAssembly(Assembly assembly)
    {
        var type = GetTypeFromAssembly(assembly);

        if (type is null)
        {
            return false;
        }

        return CheckForLzoFromType(type);
    }
    
    private static bool CheckForLzoFromType(
#if NET6_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
#endif
    Type type)
    {
        methodLzoCompress = type.GetMethod(expectedCompressMethodName, new Type[] { typeof(byte[]) });
        methodLzoDecompress = type.GetMethod(expectedDecompressMethodName, new Type[] { typeof(byte[]), typeof(byte[]) });

        if (methodLzoCompress == null || methodLzoDecompress == null)
        {
            return false;
        }

        return true;
    }

#if NET6_0_OR_GREATER
    [RequiresUnreferencedCode("The type is dynamically loaded.")]
#endif
    private static Type? GetTypeFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes().FirstOrDefault(x => x.Name == expectedClassName);
    }
}
