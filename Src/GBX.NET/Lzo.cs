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
            var assemblyMetadata = Assembly.LoadFrom(dllFile); // TODO: also support System.Reflection.Metadata

            try
            {
                var attributes = assemblyMetadata.GetCustomAttributesData();

                foreach (var attribute in attributes)
                {
                    if (attribute.ConstructorArguments.Count != 2)
                        continue;

                    if (attribute.ConstructorArguments[0].Value is string sK && sK == "LZOforGBX.NET"
                        && attribute.ConstructorArguments[1].Value is string sV && sV == "true")
                    {
                        lzoFound = CheckForLZO(Assembly.Load(assemblyMetadata.GetName()));
                        if (lzoFound) break;
                    }
                }
            }
            catch (FileLoadException)
            {

            }

            if (lzoFound) break;
        }

        if (!lzoFound)
            throw new MissingLzoException();

        checkedForLzo = true;
    }

    private static bool CheckForLZO(Assembly assembly)
    {
        var type = assembly.GetTypes().FirstOrDefault(x => x.Name == "MiniLZO");

        if (type is null)
        {
            return false;
        }

        methodLzoCompress = type.GetMethod("Compress", new Type[] { typeof(byte[]) });
        methodLzoDecompress = type.GetMethod("Decompress", new Type[] { typeof(byte[]), typeof(byte[]) });

        if (methodLzoCompress == null || methodLzoDecompress == null)
            return false;

        return true;
    }
}
