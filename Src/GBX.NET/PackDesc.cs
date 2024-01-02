namespace GBX.NET;

public record PackDesc(string FilePath = "", byte[]? Checksum = null, string? LocatorUrl = "")
{
    public static readonly PackDesc Default = new();

    public Uri? GetLocatorUri()
    {
        if (string.IsNullOrEmpty(LocatorUrl))
        {
            return null;
        }

        return new Uri(LocatorUrl);
    }
}
