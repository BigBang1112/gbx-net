using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

public static class GbxPath
{
    [return: NotNullIfNotNull(nameof(path))]
    public static string? GetFileNameWithoutExtension(string? path)
    {
        return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
    }
}
