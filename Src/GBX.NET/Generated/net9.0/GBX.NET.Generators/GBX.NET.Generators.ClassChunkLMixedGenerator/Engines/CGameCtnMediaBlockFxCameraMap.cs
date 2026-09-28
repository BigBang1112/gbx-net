namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03139000</remarks>
[Class(0x03139000)]
public partial class CGameCtnMediaBlockFxCameraMap : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x03139000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk03139000>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk03139000>]
    public TimeSingle End { get => end; set => end = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnMediaBlockFxCameraMap"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnMediaBlockFxCameraMap() { }


    /// <summary>
    /// CGameCtnMediaBlockFxCameraMap 0x000 chunk
    /// </summary>
    [Chunk(0x03139000)]
    public partial class Chunk03139000 : Chunk<CGameCtnMediaBlockFxCameraMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x03139000;


        public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    /// <summary>
    /// CGameCtnMediaBlockFxCameraMap 0x001 chunk
    /// </summary>
    [Chunk(0x03139001)]
    public partial class Chunk03139001 : Chunk<CGameCtnMediaBlockFxCameraMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x03139001;

        public float U01;
        public int U02;
        public float U03;
        public float U04;
        public float U05;
        public int U06;
        public float U07;
        public float U08;
        public float U09;
        public int U10;
        public float U11;
        public float U12;
        public byte U13;
        public int U14;
        public int U15;
        public int U16;
        public PackDesc? U17;

        public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Int32(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Int32(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Int32(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Byte(ref U13);
            rw.Int32(ref U14);
            rw.Int32(ref U15);
            rw.Int32(ref U16);
            rw.PackDesc(ref U17);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03139000 => new Chunk03139000(),
        0x03139001 => new Chunk03139001(),
        _ => base.NewChunk(chunkId),
    };
}
