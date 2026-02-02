namespace GBX.NET;

public sealed record AuthorInfo
{
    public int AuthorVersion { get; init; }
    public string AuthorLogin { get; init; } = string.Empty;
    public string AuthorNickname { get; init; } = string.Empty;
    public string AuthorZone { get; init; } = string.Empty;
    public string AuthorExtraInfo { get; init; } = string.Empty;
}
