using GBX.NET.Builders.Engines.Game;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Ghost.
/// </summary>
/// <remarks>ID: 0x030E5000</remarks>
[Node(0x030E5000)]
[NodeExtension("GameCtnMediaBlockGhost")]
public partial class CGameCtnMediaBlockGhost : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private TimeSingle? start;
    private TimeSingle? end;
    private IList<Key>? keys;
    private CGameCtnGhost ghostModel;
    private float startOffset;
    private bool noDamage;
    private bool forceLight;
    private bool forceHue;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys?.Cast<CGameCtnMediaBlock.Key>() ?? Enumerable.Empty<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    TimeSingle IHasTwoKeys.Start
    {
        get => start.GetValueOrDefault();
        set => start = value;
    }

    TimeSingle IHasTwoKeys.End
    {
        get => end.GetValueOrDefault(start.GetValueOrDefault() + TimeSingle.FromSeconds(3));
        set => end = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>(sinceVersion: 0, upToVersion: 2)]
    public TimeSingle? Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>(sinceVersion: 0, upToVersion: 2)]
    public TimeSingle? End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5002>(sinceVersion: 3)]
    public IList<Key>? Keys { get => keys; set => keys = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>]
    public CGameCtnGhost GhostModel { get => ghostModel; set => ghostModel = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5001>]
    [AppliedWithChunk<Chunk030E5002>]
    public float StartOffset { get => startOffset; set => startOffset = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5002>]
    public bool NoDamage { get => noDamage; set => noDamage = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5002>]
    public bool ForceLight { get => forceLight; set => forceLight = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk030E5002>]
    public bool ForceHue { get => forceHue; set => forceHue = value; }

    #endregion

    #region Constructors

    internal CGameCtnMediaBlockGhost()
    {
        ghostModel = null!;
    }

    public static CGameCtnMediaBlockGhostBuilder Create(CGameCtnGhost ghostModel) => new(ghostModel);

    #endregion

    #region Chunks

    #region 0x001 chunk

    [Chunk(0x030E5001)]
    public class Chunk030E5001 : Chunk<CGameCtnMediaBlockGhost>
    {
        private static void ReadWriteBeforeGhost(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end, n.start.GetValueOrDefault() + TimeSingle.FromSeconds(3));
        }

        public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            ReadWriteBeforeGhost(n, rw);
            rw.NodeRef<CGameCtnGhost>(ref n.ghostModel!);
            ReadWriteAfterGhost(n, rw);
        }

        public override async Task ReadWriteAsync(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            ReadWriteBeforeGhost(n, rw);
            n.ghostModel = (await rw.NodeRefAsync<CGameCtnGhost>(n.ghostModel!, cancellationToken))!;
            ReadWriteAfterGhost(n, rw);
        }

        private static void ReadWriteAfterGhost(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.startOffset);
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x030E5002)]
    public class Chunk030E5002 : Chunk<CGameCtnMediaBlockGhost>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        private void ReadWriteBeforeGhost(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (Version >= 3)
            {
                rw.ListKey(ref n.keys);
            }
            else
            {
                rw.TimeSingle(ref n.start);
                rw.TimeSingle(ref n.end, n.start.GetValueOrDefault() + TimeSingle.FromSeconds(3));
            }
        }

        public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            ReadWriteBeforeGhost(n, rw);
            rw.NodeRef<CGameCtnGhost>(ref n.ghostModel!);
            ReadWriteAfterGhost(n, rw);
        }

        public override async Task ReadWriteAsync(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            ReadWriteBeforeGhost(n, rw);
            n.ghostModel = (await rw.NodeRefAsync<CGameCtnGhost>(n.ghostModel!, cancellationToken))!;
            ReadWriteAfterGhost(n, rw);
        }

        private static void ReadWriteAfterGhost(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.startOffset);
            rw.Boolean(ref n.noDamage);
            rw.Boolean(ref n.forceLight);
            rw.Boolean(ref n.forceHue);
        }
    }

    #endregion

    #endregion
}
