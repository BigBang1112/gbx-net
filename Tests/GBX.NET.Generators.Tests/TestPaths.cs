namespace GBX.NET.Generators.Tests;

internal static class TestPaths
{
    public static string GetEngineDirectory()
    {
        // CallerFilePath can contain a virtual Source Link path in CI builds.
        // Start from the test output directory to find the physical checkout.
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            var engines = Path.Combine(directory.FullName, "Src", "GBX.NET", "Engines");
            if (File.Exists(Path.Combine(directory.FullName, "GBX.NET.slnx")) && Directory.Exists(engines))
            {
                return engines;
            }
        }

        throw new DirectoryNotFoundException($"Could not locate Src/GBX.NET/Engines from test output directory '{AppContext.BaseDirectory}'.");
    }
}
