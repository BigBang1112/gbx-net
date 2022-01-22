using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker track (0x03078000)
/// </summary>
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
    public string Name
    {
        get => name;
        set => name = value;
    }

    /// <summary>
    /// List of blocks.
    /// </summary>
    [NodeMember]
    public IList<CGameCtnMediaBlock> Blocks
    {
        get => blocks;
        set => blocks = value;
    }

    /// <summary>
    /// If the track should keep playing the last block after it ends.
    /// </summary>
    [NodeMember]
    public bool IsKeepPlaying
    {
        get => isKeepPlaying;
        set => isKeepPlaying = value;
    }

    [NodeMember]
    public bool IsCycling
    {
        get => isCycling;
        set => isCycling = value;
    }

    [NodeMember]
    public bool IsReadOnly
    {
        get => isReadOnly;
        set => isReadOnly = value;
    }

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

    #region 0x001 chunk

    [Chunk(0x03078001, "main")]
    public class Chunk03078001 : Chunk<CGameCtnMediaTrack>
    {
        private int tracksVersion = 10;

        public int U02 = -1;

        public int TracksVersion
        {
            get => tracksVersion;
            set => tracksVersion = value;
        }

        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name!);
            rw.Int32(ref tracksVersion);
            rw.ListNode<CGameCtnMediaBlock>(ref n.blocks!);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaTrack 0x002 chunk. Represents <see cref="IsKeepPlaying"/> for ESWC tracks. This chunk should be removed or transfered
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

    #region 0x003 chunk

    [Chunk(0x03078003, "TMS/TMU IsReadOnly")]
    public class Chunk03078003 : Chunk<CGameCtnMediaTrack>
    {
        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isReadOnly);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnMediaTrack 0x004 chunk. Represents <see cref="IsKeepPlaying"/> for TMF tracks. This chunk should be removed or transfered
    /// to <see cref="Chunk03078005"/> in the new versions of ManiaPlanet with <see cref="TransferMediaTrackTo005"/>.
    /// </summary>
    [Chunk(0x03078004, "TMUF parameters")]
    public class Chunk03078004 : Chunk<CGameCtnMediaTrack>
    {
        public int U01;

        public override void ReadWrite(CGameCtnMediaTrack n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isKeepPlaying);
            rw.Int32(ref U01); // probably isReadOnly
        }
    }

    #endregion

    #region 0x005 chunk

    [Chunk(0x03078005, "MP parameters")]
    public class Chunk03078005 : Chunk<CGameCtnMediaTrack>, IVersionable
    {
        private int version = 1;

        public float U04 = -1;
        public float U05 = -1;

        public int Version
        {
            get => version;
            set => version = value;
        }

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
