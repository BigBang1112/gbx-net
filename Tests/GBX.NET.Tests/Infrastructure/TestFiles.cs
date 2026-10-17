namespace GBX.NET.Tests.Infrastructure;

internal static class TestFiles
{
    public static string Gbx(params string[] paths) =>
        Path.Combine([AppContext.BaseDirectory, "Files", "Gbx", .. paths]);

    public static IEnumerable<string> GbxFixtures()
    {
        var root = Gbx();
        var paths = Directory.EnumerateFiles(root, "*.Gbx", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/'))
            .Order(StringComparer.Ordinal)
            .ToArray();

        if (paths.Length == 0)
        {
            throw new InvalidOperationException($"No Gbx fixtures found in {root}.");
        }

        return paths;
    }
}
