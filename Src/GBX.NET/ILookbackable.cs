namespace GBX.NET;

/// <summary>
/// Supports <see cref="Id"/> (lookback string) management on its own.
/// </summary>
public interface ILookbackable
{
    GameBox? GBX { get; }

    int? IdVersion { get; set; }
    List<string> IdStrings { get; set; }
    bool IdWritten { get; set; }
}
