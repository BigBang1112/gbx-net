using System.Diagnostics;

namespace GBX.NET;

/// <summary>
/// A file reference used for locating media.
/// </summary>
[DebuggerTypeProxy(typeof(DebugView))]
public record FileRef
{
    /// <summary>
    /// Version of the file reference.
    /// </summary>
    public byte Version { get; init; }

    /// <summary>
    /// File checksum.
    /// </summary>
    public byte[]? Checksum { get; init; }

    /// <summary>
    /// File relative to user folder (or Skins folder if <c><see cref="Version"/> &lt;= 1</c>).
    /// </summary>
    public string FilePath { get; init; }

    /// <summary>
    /// Url of the locator.
    /// </summary>
    public string? LocatorUrl { get; init; }

    /// <summary>
    /// Empty file reference version 3.
    /// </summary>
    public FileRef()
    {
        Version = 3;
        Checksum = new byte[32];
        FilePath = "";
        LocatorUrl = null;
    }

    /// <summary>
    /// File reference.
    /// </summary>
    /// <param name="version">Version of the file reference.</param>
    /// <param name="checksum">File checksum, should be null if <c><paramref name="version"/> &lt; 3</c>.</param>
    /// <param name="filePath">If <c><paramref name="version"/> &gt; 1</c>, relative to user folder, else relative to Skins folder.</param>
    /// <param name="locatorUrl">Url of the locator.</param>
    public FileRef(byte version, byte[]? checksum, string filePath, string? locatorUrl)
    {
        Version = version;
        Checksum = checksum;
        FilePath = filePath;
        LocatorUrl = locatorUrl;
    }

    public Uri? GetLocatorUri()
    {
        if (LocatorUrl is null)
        {
            return null;
        }

        return new Uri(LocatorUrl);
    }

    /// <summary>
    /// Converts the file reference to a string using the <see cref="FilePath"/>.
    /// </summary>
    /// <returns>Returns <see cref="FilePath"/>.</returns>
    public override string ToString()
    {
        return FilePath ?? string.Empty;
    }

    public static byte[] DefaultChecksum
    {
        get => new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public static readonly FileRef Default = new();

    private class DebugView
    {
        private readonly FileRef fileRef;

        public byte Version => fileRef.Version;
        public string Checksum => Convert.ToBase64String(fileRef.Checksum ?? Array.Empty<byte>());
        public string? FilePath => fileRef.FilePath;
        public string? LocatorUrl => fileRef.LocatorUrl;

        public DebugView(FileRef fileRef) => this.fileRef = fileRef;
    }
}
