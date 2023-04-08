namespace GBX.NET.Engines.Game;

/// <summary>
/// Ghost data.
/// </summary>
/// <remarks>ID: 0x0303F000</remarks>
[Node(0x0303F000)]
[NodeExtension("Ghost")]
public partial class CGameGhost : CMwNod
{
    private bool isReplaying;
    private Data? sampleData;

    [NodeMember]
    [AppliedWithChunk<Chunk0303F006>]
    public bool IsReplaying { get => isReplaying; set => isReplaying = value; }

    [NodeMember(ExactName = "TM_Data")]
    [AppliedWithChunk<Chunk0303F003>]
    [AppliedWithChunk<Chunk0303F005>]
    [AppliedWithChunk<Chunk0303F006>]

    public Data? SampleData
    {
        get
        {
            if (sampleData is null)
            {
                return null;
            }

            sampleData.Parse();

            return sampleData;
        }
    }

    internal CGameGhost()
    {

    }

    #region Chunks

    #region 0x003 chunk

    /// <summary>
    /// CGameGhost 0x003 chunk
    /// </summary>
    [Chunk(0x0303F003)]
    public class Chunk0303F003 : Chunk<CGameGhost>
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        
        public int[]? Times;

        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            Data = rw.Bytes(Data)!;

            n.sampleData = new Data(Data, isOldData: true);

            n.sampleData.Offsets = rw.Array(n.sampleData.Offsets);
            rw.Array(ref Times);
            n.sampleData.IsFixedTimeStep = rw.Boolean(n.sampleData.IsFixedTimeStep);
            n.sampleData.SamplePeriod = rw.TimeInt32(n.sampleData.SamplePeriod);
            n.sampleData.Version = rw.Int32(n.sampleData.Version);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameGhost 0x004 chunk
    /// </summary>
    [Chunk(0x0303F004)]
    public class Chunk0303F004 : Chunk<CGameGhost>
    {
        public int U01;

        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // 0x0A103000
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameGhost 0x005 chunk
    /// </summary>
    [Chunk(0x0303F005)]
    public class Chunk0303F005 : Chunk<CGameGhost>
    {
        public int UncompressedSize { get; set; }
        public int CompressedSize { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();

        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            UncompressedSize = rw.Int32(UncompressedSize);
            CompressedSize = rw.Int32(CompressedSize);
            Data = rw.Bytes(Data, CompressedSize)!;

            n.sampleData = new Data(Data, isOldData: false);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameGhost 0x006 chunk
    /// </summary>
    [Chunk(0x0303F006)]
    public class Chunk0303F006 : Chunk0303F005
    {
        public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isReplaying);
            base.ReadWrite(n, rw);
        }
    }

    #endregion

    #region 0x007 skippable chunk

    /// <summary>
    /// CGameGhost 0x007 skippable chunk
    /// </summary>
    [Chunk(0x0303F007), IgnoreChunk]
    public class Chunk0303F007 : SkippableChunk<CGameGhost>
    {

    }

    #endregion

    #endregion
}
