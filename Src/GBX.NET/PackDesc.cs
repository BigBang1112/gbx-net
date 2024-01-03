namespace GBX.NET;

/// <summary>
/// [CSystemPackDesc] Description of a file reference. Also known as "fileref".
/// </summary>
public record PackDesc(string FilePath = "", byte[]? Checksum = null, string? LocatorUrl = "")
{
    /// <summary>
    /// A default instance of <see cref=""/> with empty values.
    /// </summary>
    public static readonly PackDesc Default = new();
    
    /// <summary>
    /// Gets the locator URI of the file reference.
    /// </summary>
    /// <returns>A Uri object if LocatorUrl is specified, otherwise null.</returns>
    public Uri? GetLocatorUri()
    {
        if (string.IsNullOrEmpty(LocatorUrl))
        {
            return null;
        }

        return new Uri(LocatorUrl);
    }
}
