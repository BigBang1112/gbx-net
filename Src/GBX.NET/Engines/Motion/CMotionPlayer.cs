namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08034000</remarks>
[Node(0x08034000)]
public class CMotionPlayer : CMotion
{
    private CMotionCmdBase? _base;
    private CMotionTrack?[]? tracks;

    public CMotionCmdBase? Base { get => _base; set => _base = value; }
    public CMotionTrack?[]? Tracks { get => tracks; set => tracks = value; }

    protected CMotionPlayer()
    {

    }

    #region 0x004 chunk

    /// <summary>
    /// CMotionPlayer 0x004 chunk
    /// </summary>
    [Chunk(0x08034004)]
    public class Chunk08034004 : Chunk<CMotionPlayer>
    {
        public int U01;
        public bool U02;
        public Id U03;

        public override void Read(CMotionPlayer n, GameBoxReader r, ILogger? logger)
        {
            n._base = Parse<CMotionCmdBase>(r, 0x08029000, progress: null, logger);
            U01 = r.ReadInt32(); // SavePlayState?
            U02 = r.ReadBoolean(); // IsPlaying?
            U03 = r.ReadId();
            n.tracks = r.ReadArray<CMotionTrack?>(r => r.ReadNodeRef<CMotionTrack>());
        }
    }

    #endregion
}