using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker track.
/// </summary>
/// <remarks>ID: 0x03078000</remarks>
[Node(0x03078000)]
[NodeExtension("GameCtnMediaTrack")]
public class CGameCtnMediaTrack : CMwNod
{
    #region Fields

    private string name;
    private IList<CGameCtnMediaBlock> blocks;
    private bool isKeepPlaying;
    private bool isCycling;
    private bool isReadOnly;

    #endregion

    #region Properties

    /// <summary>
    /// Name of the track.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk(typeof(Chunk03078001))]
    public string Name { get => name; set => name = value; }

    /// <summary>
    /// List of blocks.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03078001))]
    public IList<CGameCtnMediaBlock> Blocks { get => blocks; set => blocks = value; }

    /// <summary>
    /// If the track should keep playing the last block after it ends.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03078002))]
    [AppliedWithChunk(typeof(Chunk03078004))]
    public bool IsKeepPlaying { get => isKeepPlaying; set => isKeepPlaying = value; }

    [NodeMember]
    public bool IsCycling { get => isCycling; set => isCycling = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03078003))]
    [AppliedWithChunk(typeof(Chunk03078004))]
    public bool IsReadOnly { get => isReadOnly; set => isReadOnly = value; }

    #endregion

    #region Constructors

    protected CGameCtnMediaTrack()
    {
        name = null!;
        blocks = null!;
    }

    public static CGameCtnMediaTrackBuilder Create() => new();

    #endregion

    #region Methods

    public override string ToString() => $"{base.ToString()} {{ \"{Name}\" }}";

    /// <summary>
    /// Transfers the MediaTracker track properties from either <see cref="Chunk03078002"/> (ESWC) or <see cref="Chunk03078004"/> (TMF)
    /// to <see cref="Chunk03078005"/> (ManiaPlanet and Trackmania®). If those chunks aren't presented, no action is performed.
    /// <see cref="Chunk03078003"/> is additionally removed for undiscovered safety of the chunk.
    /// </summary>
    /// <returns>True if any of the chunks were transfered.</returns>
    public bool TransferMediaTrackTo005()
    {
        var chunk002 = GetChunk<Chunk03078002>();
        var chunk004 = GetChunk<Chunk03078004>();

        if (chunk002 == null && chunk004 == null) return false;

        CreateChunk<Chunk03078005>();

        RemoveChunk<Chunk03078002>();
        RemoveChunk<Chunk03078003>(); // Removed atm for safety. TODO: inspect this chunk
        RemoveChunk<Chunk03078004>();

        return true;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk (main)

    /// <summary>
    /// CGameCtnMediaTrack 0x001 chunk (main)
    /// </summary>
    [Chunk(0x03078001, "main")]
    public class Chunk03078001 : Chunk<CGameCtnMediaTrack>
    {
        private int listVersion = 10;

        public int U02 = -1;

        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name!);
            rw.Int32(ref listVersion);
            rw.ListNode<CGameCtnMediaBlock>(ref n.blocks!);
            rw.Int32(ref U02);
        }

        public override async Task ReadWriteAsync(CGameCtnMediaTrack n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.String(ref n.name!);
            rw.Int32(ref listVersion);
            n.blocks = (await rw.ListNodeAsync<CGameCtnMediaBlock>(n.blocks!, cancellationToken))!;
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x002 chunk (TMS/TMU IsKeepPlaying)

    /// <summary>
    /// CGameCtnMediaTrack 0x002 chunk (TMS/TMU IsKeepPlaying). Represents <see cref="IsKeepPlaying"/> for ESWC tracks. This chunk should be removed or transfered
    /// to <see cref="Chunk03078005"/> in the new versions of ManiaPlanet with <see cref="TransferMediaTrackTo005"/>.
    /// </summary>
    [Chunk(0x03078002, "TMS/TMU IsKeepPlaying")]
    public class Chunk03078002 : Chunk<CGameCtnMediaTrack>
    {
        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isKeepPlaying);
        }
    }

    #endregion

    #region 0x003 chunk (TMS/TMU IsReadOnly)

    /// <summary>
    /// CGameCtnMediaTrack 0x003 chunk (TMS/TMU IsReadOnly)
    /// </summary>
    [Chunk(0x03078003, "TMS/TMU IsReadOnly")]
    public class Chunk03078003 : Chunk<CGameCtnMediaTrack>
    {
        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isReadOnly);
        }
    }

    #endregion

    #region 0x004 chunk (TMUF parameters)

    /// <summary>
    /// CGameCtnMediaTrack 0x004 chunk (TMUF parameters). Represents This chunk should be removed or transfered to <see cref="Chunk03078005"/> in the new versions of ManiaPlanet with <see cref="TransferMediaTrackTo005"/>, as ManiaPlanet corrupted backwards compatibility of this one.
    /// </summary>
    [Chunk(0x03078004, "TMUF parameters")]
    public class Chunk03078004 : Chunk<CGameCtnMediaTrack>
    {
        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isKeepPlaying);
            rw.Boolean(ref n.isReadOnly);
        }
    }

    #endregion

    #region 0x005 chunk (MP parameters)

    /// <summary>
    /// CGameCtnMediaTrack 0x005 chunk (MP parameters).
    /// </summary>
    [Chunk(0x03078005, "MP parameters")]
    public class Chunk03078005 : Chunk<CGameCtnMediaTrack>, IVersionable
    {
        private int version = 1;

        public float U04 = -1;
        public float U05 = -1;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Boolean(ref n.isKeepPlaying);
            rw.Boolean(ref n.isReadOnly);
            rw.Boolean(ref n.isCycling);

            if (version >= 1)
            {
                rw.Single(ref U04);
                rw.Single(ref U05);
            }
        }
    }

    #endregion

    #endregion
}
