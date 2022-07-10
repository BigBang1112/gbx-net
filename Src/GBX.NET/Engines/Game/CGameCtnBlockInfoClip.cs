namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03053000</remarks>
[Node(0x03053000)]
[NodeExtension("TMEDClip")]
[NodeExtension("EDClip")]
public class CGameCtnBlockInfoClip : CGameCtnBlockInfo
{
    public enum EClipType
    {
        ClassicClip,
        FreeClipSide,
        FreeClipTop,
        FreeClipBottom
    }

    private string? aSymmetricalClipId;
    private bool isFullFreeClip;
    private bool isExclusiveFreeClip;
    private EClipType clipType;
    private bool canBeDeletedByFullFreeClip;
    private EMultiDir topBottomMultiDir;
    private bool hasPassingPoint;
    private Vec2 passingPointPos;
    private float passingPointRoll;
    private float passingPointPitch;
    private string? clipGroupId;
    private string? symmetricalClipGroupId;

    [NodeMember(ExactlyNamed = true)]
    public string? ASymmetricalClipId { get => aSymmetricalClipId; set => aSymmetricalClipId = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool IsFullFreeClip { get => isFullFreeClip; set => isFullFreeClip = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool IsExclusiveFreeClip { get => isExclusiveFreeClip; set => isExclusiveFreeClip = value; }
    
    [NodeMember(ExactlyNamed = true)]
    public EClipType ClipType { get => clipType; set => clipType = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool CanBeDeletedByFullFreeClip { get => canBeDeletedByFullFreeClip; set => canBeDeletedByFullFreeClip = value; }

    [NodeMember(ExactlyNamed = true)]
    public EMultiDir TopBottomMultiDir { get => topBottomMultiDir; set => topBottomMultiDir = value; }

    [NodeMember(ExactlyNamed = true)]
    public bool HasPassingPoint { get => hasPassingPoint; set => hasPassingPoint = value; }

    [NodeMember]
    public Vec2 PassingPointPos { get => passingPointPos; set => passingPointPos = value; }

    [NodeMember(ExactlyNamed = true)]
    public float PassingPointRoll { get => passingPointRoll; set => passingPointRoll = value; }

    [NodeMember(ExactlyNamed = true)]
    public float PassingPointPitch { get => passingPointPitch; set => passingPointPitch = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? ClipGroupId
    {
        get
        {
            DiscoverChunk<Chunk03053008>();
            return clipGroupId;
        }
        set
        {
            DiscoverChunk<Chunk03053008>();
            clipGroupId = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    public string? SymmetricalClipGroupId
    {
        get
        {
            DiscoverChunk<Chunk03053008>();
            return symmetricalClipGroupId;
        }
        set
        {
            DiscoverChunk<Chunk03053008>();
            symmetricalClipGroupId = value;
        }
    }

    protected CGameCtnBlockInfoClip()
    {

    }

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x002 chunk
    /// </summary>
    [Chunk(0x03053002)]
    public class Chunk03053002 : Chunk<CGameCtnBlockInfoClip>
    {
        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.aSymmetricalClipId);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x004 chunk
    /// </summary>
    [Chunk(0x03053004)]
    public class Chunk03053004 : Chunk<CGameCtnBlockInfoClip>
    {
        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isFullFreeClip);
            rw.Boolean(ref n.isExclusiveFreeClip);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x005 chunk
    /// </summary>
    [Chunk(0x03053005)]
    public class Chunk03053005 : Chunk<CGameCtnBlockInfoClip>
    {
        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.EnumInt32<EClipType>(ref n.clipType);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x006 chunk
    /// </summary>
    [Chunk(0x03053006)]
    public class Chunk03053006 : Chunk<CGameCtnBlockInfoClip>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Boolean(ref n.canBeDeletedByFullFreeClip);
            
            if (version >= 1)
            {
                rw.EnumInt32<EMultiDir>(ref n.topBottomMultiDir);
            }
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x007 chunk
    /// </summary>
    [Chunk(0x03053007)]
    public class Chunk03053007 : Chunk<CGameCtnBlockInfoClip>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Boolean(ref n.hasPassingPoint);

            if (n.hasPassingPoint)
            {
                rw.Vec2(ref n.passingPointPos);
                rw.Single(ref n.passingPointRoll);
                rw.Single(ref n.passingPointPitch);
            }
        }
    }

    #endregion

    #region 0x007 chunk

    /// <summary>
    /// CGameCtnBlockInfoClip 0x008 skippable chunk
    /// </summary>
    [Chunk(0x03053008)]
    public class Chunk03053008 : SkippableChunk<CGameCtnBlockInfoClip>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCtnBlockInfoClip n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Id(ref n.clipGroupId);
            rw.Id(ref n.symmetricalClipGroupId);
        }
    }

    #endregion
}
