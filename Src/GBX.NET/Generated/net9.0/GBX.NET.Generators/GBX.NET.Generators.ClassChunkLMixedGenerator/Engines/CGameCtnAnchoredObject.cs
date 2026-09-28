namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03101000</remarks>
[Class(0x03101000)]
public partial class CGameCtnAnchoredObject : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03101000;




    private Ident itemModel = Ident.Empty;
    [AppliedWithChunk<Chunk03101002>]
    public Ident ItemModel { get => itemModel; set => itemModel = value; }

    private Vec3 yawPitchRoll;
    [AppliedWithChunk<Chunk03101002>]
    public Vec3 YawPitchRoll { get => yawPitchRoll; set => yawPitchRoll = value; }

    private Byte3 blockUnitCoord;
    [AppliedWithChunk<Chunk03101002>]
    public Byte3 BlockUnitCoord { get => blockUnitCoord; set => blockUnitCoord = value; }

    private string? anchorTreeId;
    [AppliedWithChunk<Chunk03101002>]
    public string? AnchorTreeId { get => anchorTreeId; set => anchorTreeId = value; }

    private Vec3 absolutePositionInMap;
    [AppliedWithChunk<Chunk03101002>]
    public Vec3 AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }

    private CGameWaypointSpecialProperty? waypointSpecialProperty;
    [AppliedWithChunk<Chunk03101002>]
    public CGameWaypointSpecialProperty? WaypointSpecialProperty { get => waypointSpecialProperty; set => waypointSpecialProperty = value; }

    private short flags;
    [AppliedWithChunk<Chunk03101002>]
    public short Flags { get => flags; set => flags = value; }

    private Vec3 pivotPosition;
    [AppliedWithChunk<Chunk03101002>]
    public Vec3 PivotPosition { get => pivotPosition; set => pivotPosition = value; }

    private float scale;
    [AppliedWithChunk<Chunk03101002>]
    public float Scale { get => scale; set => scale = value; }

    private PackDesc? packDesc;
    [AppliedWithChunk<Chunk03101002>]
    public PackDesc? PackDesc { get => packDesc; set => packDesc = value; }


    /// <summary>
    /// CGameCtnAnchoredObject 0x002 chunk
    /// </summary>
    [Chunk(0x03101002)]
    public partial class Chunk03101002 : Chunk<CGameCtnAnchoredObject>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03101002;

        public int Version { get; set; }

        public int U01;
        public Vec3? U02;
        public Vec3? U03;

        public override void ReadWrite(CGameCtnAnchoredObject n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Ident(ref n.itemModel);
            rw.Vec3(ref n.yawPitchRoll);
            rw.Byte3(ref n.blockUnitCoord);
            rw.Id(ref n.anchorTreeId);
            rw.Vec3(ref n.absolutePositionInMap);
            rw.NodeRef<CGameWaypointSpecialProperty>(ref n.waypointSpecialProperty);
            if (Version <= 4)
            {
                rw.Int32(ref U01);
            }
            if (Version >= 4)
            {
                rw.Int16(ref n.flags);
                if (Version >= 5)
                {
                    rw.Vec3(ref n.pivotPosition);
                    if (Version >= 6)
                    {
                        rw.Single(ref n.scale);
                        if (Version >= 7)
                        {
                            if ((n.Flags&4)!=0)
                            {
                                rw.PackDesc(ref n.packDesc);
                            }
                            if (Version >= 8)
                            {
                                rw.Vec3(ref U02);
                                rw.Vec3(ref U03);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CGameCtnAnchoredObject 0x004 skippable chunk
    /// </summary>
    [Chunk(0x03101004)]
    public partial class Chunk03101004 : SkippableChunk<CGameCtnAnchoredObject>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x03101004;

        public int Version { get; set; }

        public int U01 = -1;

        public override void ReadWrite(CGameCtnAnchoredObject n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnAnchoredObject 0x005 skippable chunk
    /// </summary>
    [Chunk(0x03101005)]
    public partial class Chunk03101005 : SkippableChunk<CGameCtnAnchoredObject>
    {
        /// <inheritdoc />
        public override uint Id => 0x03101005;

        /// <inheritdoc />
        public override bool Ignore => true;

    }



    public enum EPhaseOffset
    {
        None,
        One8th,
        Two8th,
        Three8th,
        Four8th,
        Five8th,
        Six8th,
        Seven8th,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03101002 => new Chunk03101002(),
        0x03101004 => new Chunk03101004(),
        0x03101005 => new Chunk03101005(),
        _ => base.NewChunk(chunkId),
    };
}
