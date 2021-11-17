namespace GBX.NET.Debugging;

#if DEBUG
public class GameBoxBodyDebugger
{
    public byte[]? CompressedData { get; set; }
    public byte[]? UncompressedData { get; set; }
}
#endif
