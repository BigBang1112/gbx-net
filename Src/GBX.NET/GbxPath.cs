using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

public static class GbxPath
{
    public static ImmutableArray<char> InvalidFileNameChars { get; } = ImmutableArray.Create([
        '\"', '<', '>', '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31, ':', '*', '?', '\\', '/'
    ]);

#if NET8_0_OR_GREATER
    public static SearchValues<char> InvalidFileNameCharSearchValues { get; } = SearchValues.Create([
        '\"', '<', '>', '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31, ':', '*', '?', '\\', '/'
    ]);
#endif

    [return: NotNullIfNotNull(nameof(path))]
    public static string? GetFileNameWithoutExtension(string? path)
    {
        return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
    }
}
