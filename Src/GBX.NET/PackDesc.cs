namespace GBX.NET;

public record PackDesc(byte Version, byte[]? Checksum, string FilePath, string? LocatorUrl)
{
    public static readonly PackDesc Default = new();

    public PackDesc() : this(Version: 3, Checksum: new byte[32], FilePath: string.Empty, LocatorUrl: string.Empty) { }

    public Uri? GetLocatorUri()
    {
        if (string.IsNullOrEmpty(LocatorUrl))
        {
            return null;
        }

        return new Uri(LocatorUrl);
    }
}
