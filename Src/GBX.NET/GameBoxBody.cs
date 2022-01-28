using GBX.NET.Debugging;

namespace GBX.NET;

public abstract class GameBoxBody : GameBoxPart
{
    public GameBoxBodyDebugger? Debugger { get; protected set; }

    internal bool IsParsed { get; set; }
    internal int UncompressedSize { get; set; }

    /// <summary>
    /// Pure body data usually in compressed form. This property is used if GameBox's ParseHeader methods are used, otherwise null.
    /// </summary>
    internal byte[]? RawData { get; set; }

    protected GameBoxBody(GameBox gbx) : base(gbx)
    {

    }
}