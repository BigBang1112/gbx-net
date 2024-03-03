using System.Collections.Immutable;

namespace GBX.NET;

/// <summary>
/// [CSystemPackDesc] Description of a file reference. Also known as "fileref".
/// </summary>
public sealed record PackDesc(string FilePath = "", ImmutableArray<byte> Checksum = default, string? LocatorUrl = "")
{
    /// <summary>
    /// A default instance of <see cref=""/> with empty values.
    /// </summary>
    public static readonly PackDesc Empty = new();
    
    /// <summary>
    /// Gets the locator URI of the file reference.
    /// </summary>
    /// <returns>A Uri object if <see cref="LocatorUrl"/> is specified, otherwise null.</returns>
    public Uri? GetLocatorUri()
    {
        return string.IsNullOrEmpty(LocatorUrl) ? null : new Uri(LocatorUrl);
    }

    public override string ToString()
    {
        return $"PackDesc: \"{FilePath}\" ({LocatorUrl})";
    }
}
