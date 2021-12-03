using System.Reflection;

namespace GBX.NET;

internal static class Lzo
{
    private static bool checkedForLzo;
    private static MethodInfo? methodLzoCompress;
    private static MethodInfo? methodLzoDecompress;

    public static void Decompress(byte[] input, byte[] output)
    {
        CheckForLZO();
        methodLzoDecompress!.Invoke(null, new object[] { input, output });
    }

    public static byte[] Compress(byte[] data)
    {
        CheckForLZO();
        return (byte[])methodLzoCompress!.Invoke(null, new object[] { data })!;
    }

    /// <exception cref="MissingLzoException"></exception>
    private static void CheckForLZO()
    {
        if (checkedForLzo) return;

        var lzoFound = false;

        foreach (var dllFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
        {
            lzoFound = AnalyzeDllFile(dllFile);

            if (lzoFound)
                break;
        }

        if (!lzoFound)
            throw new MissingLzoException();

        checkedForLzo = true;
    }

    private static bool AnalyzeDllFile(string dllFile)
    {
        var assembly = Assembly.LoadFrom(dllFile); // TODO: also support System.Reflection.Metadata

        var attributes = assembly.GetCustomAttributesData();

        foreach (var attribute in attributes)
        {
            var verified = VerifyAttribute(assembly, attribute);

            if (verified)
                return true;
        }

        return false;
    }

    private static bool VerifyAttribute(Assembly assembly, CustomAttributeData attribute)
    {
        if (attribute.ConstructorArguments.Count != 2)
            return false;

        var expectedAttribute = new[] { "LZOforGBX.NET", "true" };

        var hasExpectedAttribute = attribute.ConstructorArguments
            .Select(x => x.Value as string)
            .Where(x => x is not null)
            .SequenceEqual(expectedAttribute);

        if (hasExpectedAttribute)
        {
            var lzoFound = CheckForLZO(assembly);

            if (lzoFound)
                return true;
        }

        return false;
    }

    private static bool CheckForLZO(Assembly assembly)
    {
        var type = assembly.GetTypes()
            .FirstOrDefault(x => x.Name == "MiniLZO");

        if (type is null)
            return false;

        methodLzoCompress = type.GetMethod("Compress", new Type[] { typeof(byte[]) });
        methodLzoDecompress = type.GetMethod("Decompress", new Type[] { typeof(byte[]), typeof(byte[]) });

        if (methodLzoCompress == null || methodLzoDecompress == null)
            return false;

        return true;
    }
}
