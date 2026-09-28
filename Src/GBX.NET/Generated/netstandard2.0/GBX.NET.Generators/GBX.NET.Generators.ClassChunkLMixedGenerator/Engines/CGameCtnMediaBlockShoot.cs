namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03145000</remarks>
[Class(0x03145000)]
public partial class CGameCtnMediaBlockShoot : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03145000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03145000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03145000>]
    public TimeSingle End { get => end; set => end = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockShoot"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockShoot() { }


    /// <summary>
    /// CGameCtnMediaBlockShoot 0x000 chunk
    /// </summary>
    [Chunk(0x03145000)]
    public partial class Chunk03145000 : Chunk<CGameCtnMediaBlockShoot>
    {
        /// <inheritdoc />
        public override uint Id => 0x03145000;


        public override void ReadWrite(CGameCtnMediaBlockShoot n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03145000 => new Chunk03145000(),
        _ => base.NewChunk(chunkId),
    };
}
