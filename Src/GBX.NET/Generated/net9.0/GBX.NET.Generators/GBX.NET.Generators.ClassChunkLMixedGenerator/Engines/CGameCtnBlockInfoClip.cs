namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03053000</remarks>
[Class(0x03053000)]
public partial class CGameCtnBlockInfoClip : CGameCtnBlockInfo, IClass
{
    [Hexadecimal] public static new uint Id => 0x03053000;




    private string? aSymmetricalClipId;
    [AppliedWithChunk<Chunk03053002>]
    public string? ASymmetricalClipId { get => aSymmetricalClipId; set => aSymmetricalClipId = value; }

    private bool isFullFreeClip;
    [AppliedWithChunk<Chunk03053004>]
    public bool IsFullFreeClip { get => isFullFreeClip; set => isFullFreeClip = value; }

    private bool isExclusiveFreeClip;
    [AppliedWithChunk<Chunk03053004>]
    public bool IsExclusiveFreeClip { get => isExclusiveFreeClip; set => isExclusiveFreeClip = value; }

    private EClipType clipType;
    [AppliedWithChunk<Chunk03053005>]
    public EClipType ClipType { get => clipType; set => clipType = value; }

    private bool canBeDeletedByFullFreeClip;
    [AppliedWithChunk<Chunk03053006>]
    public bool CanBeDeletedByFullFreeClip { get => canBeDeletedByFullFreeClip; set => canBeDeletedByFullFreeClip = value; }

    private EMultiDir topBottomMultiDir;
    [AppliedWithChunk<Chunk03053006>]
    public EMultiDir TopBottomMultiDir { get => topBottomMultiDir; set => topBottomMultiDir = value; }

    private bool hasPassingPoint;
    [AppliedWithChunk<Chunk03053007>]
    public bool HasPassingPoint { get => hasPassingPoint; set => hasPassingPoint = value; }

    private Vec2 passingPointPos;
    [AppliedWithChunk<Chunk03053007>]
    public Vec2 PassingPointPos { get => passingPointPos; set => passingPointPos = value; }

    private float passingPointRoll;
    [AppliedWithChunk<Chunk03053007>]
    public float PassingPointRoll { get => passingPointRoll; set => passingPointRoll = value; }

    private float passingPointPitch;
    [AppliedWithChunk<Chunk03053007>]
    public float PassingPointPitch { get => passingPointPitch; set => passingPointPitch = value; }

    private string? clipGroupId;
    [AppliedWithChunk<Chunk03053008>]
    public string? ClipGroupId { get => clipGroupId; set => clipGroupId = value; }

    private string? symmetricalClipGroupId;
    [AppliedWithChunk<Chunk03053008>]
    public string? SymmetricalClipGroupId { get => symmetricalClipGroupId; set => symmetricalClipGroupId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnBlockInfoClip"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnBlockInfoClip() { }


    /// <summary>
    /// CGameCtnBlockInfoClip 0x002 chunk
    /// </summary>
    [Chunk(0x03053002)]
    public partial class Chunk03053002 : Chunk<CGameCtnBlockInfoClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03053002;


        public override void ReadWrite(CGameCtnBlockInfoClip n, GbxReaderWriter rw)
        {
            rw.Id(ref n.aSymmetricalClipId);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoClip 0x004 chunk
    /// </summary>
    [Chunk(0x03053004)]
    public partial class Chunk03053004 : Chunk<CGameCtnBlockInfoClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03053004;


        public override void ReadWrite(CGameCtnBlockInfoClip n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isFullFreeClip);
            rw.Boolean(ref n.isExclusiveFreeClip);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoClip 0x005 chunk
    /// </summary>
    [Chunk(0x03053005)]
    public partial class Chunk03053005 : Chunk<CGameCtnBlockInfoClip>
    {
        /// <inheritdoc />
        public override uint Id => 0x03053005;


        public override void ReadWrite(CGameCtnBlockInfoClip n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EClipType>(ref n.clipType);
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoClip 0x006 chunk
    /// </summary>
    [Chunk(0x03053006)]
    public partial class Chunk03053006 : Chunk<CGameCtnBlockInfoClip>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03053006;

        public int Version { get; set; }

        public byte? U01;
        public byte? U02;
        public byte? U03;

        public override void ReadWrite(CGameCtnBlockInfoClip n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.canBeDeletedByFullFreeClip);
            if (Version >= 1)
            {
                rw.EnumInt32<EMultiDir>(ref n.topBottomMultiDir);
                if (Version >= 2)
                {
                    rw.Byte(ref U01);
                    if (Version >= 3)
                    {
                        rw.Byte(ref U02);
                        if (Version >= 4)
                        {
                            rw.Byte(ref U03);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoClip 0x007 chunk
    /// </summary>
    [Chunk(0x03053007)]
    public partial class Chunk03053007 : Chunk<CGameCtnBlockInfoClip>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03053007;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnBlockInfoClip n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref n.hasPassingPoint);
            if (n.HasPassingPoint)
            {
                rw.Vec2(ref n.passingPointPos);
                rw.Single(ref n.passingPointRoll);
                rw.Single(ref n.passingPointPitch);
            }
        }
    }

    /// <summary>
    /// CGameCtnBlockInfoClip 0x008 skippable chunk
    /// </summary>
    [Chunk(0x03053008)]
    public partial class Chunk03053008 : SkippableChunk<CGameCtnBlockInfoClip>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03053008;

        public int Version { get; set; }

        public string? U01;
        public string? U02;

        public override void ReadWrite(CGameCtnBlockInfoClip n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.clipGroupId);
            rw.Id(ref n.symmetricalClipGroupId);
            if (Version >= 1)
            {
                rw.Id(ref U01);
                rw.Id(ref U02);
            }
        }
    }



    public enum EClipType
    {
        ClassicClip,
        FreeClipSide,
        FreeClipTop,
        FreeClipBottom,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03053002 => new Chunk03053002(),
        0x03053004 => new Chunk03053004(),
        0x03053005 => new Chunk03053005(),
        0x03053006 => new Chunk03053006(),
        0x03053007 => new Chunk03053007(),
        0x03053008 => new Chunk03053008(),
        _ => base.NewChunk(chunkId),
    };
}
