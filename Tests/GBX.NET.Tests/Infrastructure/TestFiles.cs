namespace GBX.NET.Tests.Infrastructure;

internal static class TestFiles
{
    public static string Gbx(params string[] paths) =>
        Path.Combine([AppContext.BaseDirectory, "Files", "Gbx", .. paths]);
}
