using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace GBX.NET;

public static class GbxPath
{
    /// <summary>
    /// Array of characters that are not allowed in Windows file names.
    /// </summary>
    public static ImmutableArray<char> InvalidFileNameChars { get; } = ImmutableArray.Create([
        '\"', '<', '>', '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31, ':', '*', '?', '\\', '/'
    ]);

#if NET8_0_OR_GREATER
    /// <summary>
    /// <see cref="SearchValues"/> for characters that are not allowed in Windows file names. This has faster Contains operations than <see cref="InvalidFileNameChars"/>.
    /// </summary>
    public static SearchValues<char> InvalidFileNameCharSearchValues { get; } = SearchValues.Create([
        '\"', '<', '>', '|', '\0',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
        (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
        (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
        (char)31, ':', '*', '?', '\\', '/'
    ]);
#endif

    /// <summary>
    /// Gets the file name without the extension, also stripping the Gbx's double extension properly.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <returns>The file name without extension.</returns>
    [return: NotNullIfNotNull(nameof(path))]
    public static string? GetFileNameWithoutExtension(string? path)
    {
        return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path));
    }

    /// <summary>
    /// Changes the file path extension, considering the Gbx's double extension.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <param name="extension">The new extension.</param>
    /// <returns>The file path with changed extension.</returns>
    [return: NotNullIfNotNull(nameof(path))]
    public static string? ChangeExtension(string? path, string? extension)
    {
        return Path.ChangeExtension(Path.ChangeExtension(path, null), extension);
    }

    /// <summary>
    /// Gets a valid file name by replacing invalid Windows characters with underscores.
    /// </summary>
    /// <param name="fileName">Name of the file, WITHOUT the directory path.</param>
    /// <returns>File name that has no issues on Windows.</returns>
    public static string GetValidFileName(string fileName)
    {
#if NET8_0_OR_GREATER
        var invalidChars = InvalidFileNameCharSearchValues;
#else
        var invalidChars = InvalidFileNameChars;
#endif
        var buffer = ArrayPool<char>.Shared.Rent(fileName.Length);
        var bufferIndex = 0;

        foreach (var c in fileName)
        {
            buffer[bufferIndex++] = invalidChars.Contains(c) ? '_' : c;
        }

        var result = new string(buffer, 0, bufferIndex);
        ArrayPool<char>.Shared.Return(buffer);

        return result;
    }
}
