namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0338B000</remarks>
[Class(0x0338B000)]
public partial class CGameCtnMediaBlockOpponentVisibility : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x0338B000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk0338B000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk0338B000>]
    public TimeSingle End { get => end; set => end = value; }

    private EVisibility visibility;
    [AppliedWithChunk<Chunk0338B001>]
    public EVisibility Visibility { get => visibility; set => visibility = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockOpponentVisibility"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockOpponentVisibility() { }


    /// <summary>
    /// CGameCtnMediaBlockOpponentVisibility 0x000 chunk
    /// </summary>
    [Chunk(0x0338B000)]
    public partial class Chunk0338B000 : Chunk<CGameCtnMediaBlockOpponentVisibility>
    {
        /// <inheritdoc />
        public override uint Id => 0x0338B000;


        public override void ReadWrite(CGameCtnMediaBlockOpponentVisibility n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockOpponentVisibility 0x001 chunk
    /// </summary>
    [Chunk(0x0338B001)]
    public partial class Chunk0338B001 : Chunk<CGameCtnMediaBlockOpponentVisibility>
    {
        /// <inheritdoc />
        public override uint Id => 0x0338B001;


        public override void ReadWrite(CGameCtnMediaBlockOpponentVisibility n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EVisibility>(ref n.visibility);
        }
    }



    public enum EVisibility
    {
        Hidden,
        Ghost,
        Opaque,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0338B000 => new Chunk0338B000(),
        0x0338B001 => new Chunk0338B001(),
        _ => base.NewChunk(chunkId),
    };
}
