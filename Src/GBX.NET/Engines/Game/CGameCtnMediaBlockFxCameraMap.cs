namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera map effect.
/// </summary>
/// <remarks>ID: 0x03139000</remarks>
[Node(0x03139000)]
[NodeExtension("GameCtnMediaBlockFxCameraMap")]
public class CGameCtnMediaBlockFxCameraMap : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
{
    private TimeSingle start;
    private TimeSingle end = TimeSingle.FromSeconds(3);

    [NodeMember]
    [AppliedWithChunk<Chunk03139000>]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03139001>]
    public TimeSingle End { get => end; set => end = value; }

    internal CGameCtnMediaBlockFxCameraMap()
    {

    }

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03139000)]
    public class Chunk03139000 : Chunk<CGameCtnMediaBlockFxCameraMap>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03139001)]
    public class Chunk03139001 : Chunk<CGameCtnMediaBlockFxCameraMap>
    {
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
        public FileRef? U17;

        public override void ReadWrite(CGameCtnMediaBlockFxCameraMap n, GameBoxReaderWriter rw)
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
            rw.FileRef(ref U17);
        }
    }

    #endregion

    #endregion
}
