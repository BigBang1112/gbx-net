using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// A MediaTracker clip.
/// </summary>
/// <remarks>ID: 0x03079000</remarks>
[Node(0x03079000)]
[NodeExtension("Clip")]
public class CGameCtnMediaClip : CMwNod
{
    #region Fields

    private string name;
    private IList<CGameCtnMediaTrack> tracks;
    private bool stopWhenRespawn;
    private bool stopWhenLeave;
    private int? localPlayerClipEntIndex; // -1

    #endregion

    #region Properties

    /// <summary>
    /// Name of the clip. The value of this property is an empty string if the clip is an intro, ambiance or podium.
    /// </summary>
    [NodeMember]
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03079002>]
    [AppliedWithChunk<Chunk03079003>]
    [AppliedWithChunk<Chunk03079005>]
    [AppliedWithChunk<Chunk0307900D>]
    public string Name { get => name; set => name = value; }

    /// <summary>
    /// List of MediaTracker tracks.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk03079002>]
    [AppliedWithChunk<Chunk03079003>]
    [AppliedWithChunk<Chunk03079005>]
    [AppliedWithChunk<Chunk0307900D>]
    public IList<CGameCtnMediaTrack> Tracks { get => tracks; set => tracks = value; }

    /// <summary>
    /// Stop the clip when player respawns.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0307900D>]
    public bool StopWhenRespawn { get => stopWhenRespawn; set => stopWhenRespawn = value; }

    /// <summary>
    /// Stop the clip when player leaves the trigger.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk<Chunk0307900A>]
    [AppliedWithChunk<Chunk0307900D>]
    public bool StopWhenLeave { get => stopWhenLeave; set => stopWhenLeave = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk03079007>]
    [AppliedWithChunk<Chunk0307900D>]
    public int? LocalPlayerClipEntIndex { get => localPlayerClipEntIndex; set => localPlayerClipEntIndex = value; }

    #endregion

    #region Constructors

    internal CGameCtnMediaClip()
    {
        name = "";
        tracks = Array.Empty<CGameCtnMediaTrack>();
    }

    public static CGameCtnMediaClipBuilder Create() => new();

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{base.ToString()} {{ \"{(string.IsNullOrEmpty(Name) ? "Unnamed clip" : Name)}\" }}";
    }

    public IEnumerable<CGameCtnGhost> GetGhosts()
    {
        foreach (var track in Tracks)
        {
            foreach (var block in track.Blocks)
            {
                if (block is CGameCtnMediaBlockGhost ghostBlock)
                {
                    yield return ghostBlock.GhostModel;
                }
            }
        }
    }

    #endregion

    #region Chunks

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x002 chunk
    /// </summary>
    [Chunk(0x03079002)]
    public class Chunk03079002 : Chunk<CGameCtnMediaClip>
    {
        private int listVersion = 10;

        public bool U01;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ListNode<CGameCtnMediaTrack>(ref n.tracks!); // with sorting
            rw.String(ref n.name!);
            rw.Boolean(ref U01); // Looks to be the same as U01 from 0x004
        }

        public override async Task ReadWriteAsync(CGameCtnMediaClip n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Int32(ref listVersion);
            n.tracks = (await rw.ListNodeAsync<CGameCtnMediaTrack>(n.tracks!, cancellationToken))!;
            rw.String(ref n.name!);
            rw.Boolean(ref U01); // Looks to be the same as U01 from 0x004
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x003 chunk
    /// </summary>
    [Chunk(0x03079003)]
    public class Chunk03079003 : Chunk<CGameCtnMediaClip>
    {
        private int listVersion = 10;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ListNode<CGameCtnMediaTrack>(ref n.tracks!); // with sorting
            rw.String(ref n.name!);
        }

        public override async Task ReadWriteAsync(CGameCtnMediaClip n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Int32(ref listVersion);
            n.tracks = (await rw.ListNodeAsync<CGameCtnMediaTrack>(n.tracks!, cancellationToken))!;
            rw.String(ref n.name!);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x004 chunk
    /// </summary>
    [Chunk(0x03079004)]
    public class Chunk03079004 : Chunk<CGameCtnMediaClip>
    {
        public CMwNod? U01;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // Looks to be the same as U01 from 0x002
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x005 chunk
    /// </summary>
    [Chunk(0x03079005)]
    public class Chunk03079005 : Chunk<CGameCtnMediaClip>
    {
        private int listVersion = 10;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ListNode<CGameCtnMediaTrack>(ref n.tracks!); // withOUT sorting
            rw.String(ref n.name!);
        }

        public override async Task ReadWriteAsync(CGameCtnMediaClip n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Int32(ref listVersion);
            n.tracks = (await rw.ListNodeAsync<CGameCtnMediaTrack>(n.tracks!, cancellationToken))!;
            rw.String(ref n.name!);
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x007 chunk
    /// </summary>
    [Chunk(0x03079007)]
    public class Chunk03079007 : Chunk<CGameCtnMediaClip>
    {
        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.localPlayerClipEntIndex, -1);
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x008 chunk
    /// </summary>
    [Chunk(0x03079008)]
    public class Chunk03079008 : Chunk<CGameCtnMediaClip>
    {
        public float U01;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01); // 0.2
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CGameCtnMediaClip 0x009 chunk
    /// </summary>
    [Chunk(0x03079009)]
    public class Chunk03079009 : Chunk<CGameCtnMediaClip>
    {
        public string U01 = ""; // Same as 0x00D U05

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.String(ref U01!); // Same as 0x00D U05
        }
    }

    #endregion

    #region 0x00A chunk

    /// <summary>
    /// CGameCtnMediaClip 0x00A chunk
    /// </summary>
    [Chunk(0x0307900A)]
    public class Chunk0307900A : Chunk<CGameCtnMediaClip>
    {
        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.stopWhenLeave);
        }
    }

    #endregion

    #region 0x00B chunk

    /// <summary>
    /// CGameCtnMediaClip 0x00B chunk
    /// </summary>
    [Chunk(0x0307900B)]
    public class Chunk0307900B : Chunk<CGameCtnMediaClip>
    {
        public bool U01;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01); // probably NOT StopWhenRespawn
        }
    }

    #endregion

    #region 0x00C chunk

    /// <summary>
    /// CGameCtnMediaClip 0x00C chunk
    /// </summary>
    [Chunk(0x0307900C)]
    public class Chunk0307900C : Chunk<CGameCtnMediaClip>
    {
        public int U01;

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x00D chunk

    /// <summary>
    /// CGameCtnMediaClip 0x00D chunk
    /// </summary>
    [Chunk(0x0307900D)]
    public class Chunk0307900D : Chunk<CGameCtnMediaClip>, IVersionable
    {
        private int version;
        private int listVersion = 10;

        public bool U03;
        public string U05 = ""; // Same as 0x009 U05
        public float U06 = 0.2f;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnMediaClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref listVersion);
            rw.ListNode<CGameCtnMediaTrack>(ref n.tracks!);
            rw.String(ref n.name!);
            rw.Boolean(ref n.stopWhenLeave);
            rw.Boolean(ref U03);
            rw.Boolean(ref n.stopWhenRespawn);
            rw.String(ref U05!); // Same as 0x009 U05
            rw.Single(ref U06);
            rw.Int32(ref n.localPlayerClipEntIndex, -1);
        }

        public override async Task ReadWriteAsync(CGameCtnMediaClip n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Int32(ref version);
            rw.Int32(ref listVersion);
            n.tracks = (await rw.ListNodeAsync<CGameCtnMediaTrack>(n.tracks!, cancellationToken))!;
            rw.String(ref n.name!);
            rw.Boolean(ref n.stopWhenLeave);
            rw.Boolean(ref U03);
            rw.Boolean(ref n.stopWhenRespawn);
            rw.String(ref U05!); // Same as 0x009 U05
            rw.Single(ref U06);
            rw.Int32(ref n.localPlayerClipEntIndex, -1);
        }
    }

    #endregion

    #region 0x00E skippable chunk

    /// <summary>
    /// CGameCtnMediaClip 0x00E skippable chunk
    /// </summary>
    [Chunk(0x0307900E), IgnoreChunk]
    public class Chunk0307900E : SkippableChunk<CGameCtnMediaClip>
    {

    }

    #endregion

    #endregion
}
