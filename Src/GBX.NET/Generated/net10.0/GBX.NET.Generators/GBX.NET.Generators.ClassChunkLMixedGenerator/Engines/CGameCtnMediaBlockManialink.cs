namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0312A000</remarks>
[Class(0x0312A000)]
public partial class CGameCtnMediaBlockManialink : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x0312A000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk0312A001>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk0312A001>]
    public TimeSingle End { get => end; set => end = value; }

    private string? manialinkUrl;
    [AppliedWithChunk<Chunk0312A001>]
    public string? ManialinkUrl { get => manialinkUrl; set => manialinkUrl = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockManialink"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockManialink() { }


    /// <summary>
    /// CGameCtnMediaBlockManialink 0x001 chunk
    /// </summary>
    [Chunk(0x0312A001)]
    public partial class Chunk0312A001 : Chunk<CGameCtnMediaBlockManialink>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0312A001;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnMediaBlockManialink n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.String(ref n.manialinkUrl);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0312A001 => new Chunk0312A001(),
        _ => base.NewChunk(chunkId),
    };
}
