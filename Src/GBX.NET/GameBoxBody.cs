using GBX.NET.Debugging;
using System.Runtime.Serialization;

namespace GBX.NET;

public abstract class GameBoxBody : GameBoxPart
{
    public GameBoxBodyDebugger? Debugger { get; protected set; }

    [IgnoreDataMember]
    public SortedDictionary<int, Node> AuxilaryNodes { get; }

    internal bool IsParsed { get; set; }
    internal int UncompressedSize { get; set; }

    /// <summary>
    /// Pure body data usually in compressed form. This property is used if GameBox's ParseHeader methods are used, otherwise null.
    /// </summary>
    internal byte[]? RawData { get; set; }

    public GameBoxBody(GameBox gbx) : base(gbx)
    {
        AuxilaryNodes = new SortedDictionary<int, Node>();
    }
}