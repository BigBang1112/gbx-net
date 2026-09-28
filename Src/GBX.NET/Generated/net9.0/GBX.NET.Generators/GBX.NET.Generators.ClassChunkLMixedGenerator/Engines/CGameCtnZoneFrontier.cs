namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0305E000</remarks>
[Class(0x0305E000)]
public partial class CGameCtnZoneFrontier : CGameCtnZone, IClass
{
    [Hexadecimal] public static new uint Id => 0x0305E000;




    private CGameCtnBlockInfoFrontier? blockInfoFrontier;
    [AppliedWithChunk<Chunk0305E001>]
    public CGameCtnBlockInfoFrontier? BlockInfoFrontier { get => blockInfoFrontierFile?.GetNode(ref blockInfoFrontier) ?? blockInfoFrontier; set => blockInfoFrontier = value; }
    private Components.GbxRefTableFile? blockInfoFrontierFile;
    public Components.GbxRefTableFile? BlockInfoFrontierFile { get => blockInfoFrontierFile; set => blockInfoFrontierFile = value; }
    public CGameCtnBlockInfoFrontier? GetBlockInfoFrontier(GbxReadSettings settings = default, bool exceptions = false) => blockInfoFrontierFile?.GetNode(ref blockInfoFrontier, settings, exceptions) ?? blockInfoFrontier;

    private string? parentZoneId;
    [AppliedWithChunk<Chunk0305E001>]
    public string? ParentZoneId { get => parentZoneId; set => parentZoneId = value; }

    private string? childZoneId;
    [AppliedWithChunk<Chunk0305E001>]
    public string? ChildZoneId { get => childZoneId; set => childZoneId = value; }

    private int blockYOffsetFromParent;
    [AppliedWithChunk<Chunk0305E002>]
    public int BlockYOffsetFromParent { get => blockYOffsetFromParent; set => blockYOffsetFromParent = value; }

    private bool frontierParentBorder_AcceptPylons;
    [AppliedWithChunk<Chunk0305E003>]
    [AppliedWithChunk<Chunk0305E004>]
    public bool FrontierParentBorder_AcceptPylons { get => frontierParentBorder_AcceptPylons; set => frontierParentBorder_AcceptPylons = value; }

    private bool frontierChildBorder_AcceptPylons;
    [AppliedWithChunk<Chunk0305E003>]
    [AppliedWithChunk<Chunk0305E004>]
    public bool FrontierChildBorder_AcceptPylons { get => frontierChildBorder_AcceptPylons; set => frontierChildBorder_AcceptPylons = value; }

    private bool frontierTransitionMiddle_AcceptPylons;
    [AppliedWithChunk<Chunk0305E004>]
    public bool FrontierTransitionMiddle_AcceptPylons { get => frontierTransitionMiddle_AcceptPylons; set => frontierTransitionMiddle_AcceptPylons = value; }

    private bool frontierStraightMiddle_AcceptPylons;
    [AppliedWithChunk<Chunk0305E004>]
    public bool FrontierStraightMiddle_AcceptPylons { get => frontierStraightMiddle_AcceptPylons; set => frontierStraightMiddle_AcceptPylons = value; }

    private CGameCtnZoneFusionInfo[]? compatibleZones;
    [AppliedWithChunk<Chunk0305E005>]
    public CGameCtnZoneFusionInfo[]? CompatibleZones { get => compatibleZones; set => compatibleZones = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnZoneFrontier"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnZoneFrontier() { }


    /// <summary>
    /// CGameCtnZoneFrontier 0x001 chunk
    /// </summary>
    [Chunk(0x0305E001)]
    public partial class Chunk0305E001 : Chunk<CGameCtnZoneFrontier>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305E001;


        public override void ReadWrite(CGameCtnZoneFrontier n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoFrontier>(ref n.blockInfoFrontier, ref n.blockInfoFrontierFile);
            rw.Id(ref n.parentZoneId);
            rw.Id(ref n.childZoneId);
        }
    }

    /// <summary>
    /// CGameCtnZoneFrontier 0x002 chunk
    /// </summary>
    [Chunk(0x0305E002)]
    public partial class Chunk0305E002 : Chunk<CGameCtnZoneFrontier>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305E002;

        public int U01;

        public override void ReadWrite(CGameCtnZoneFrontier n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref n.blockYOffsetFromParent);
        }
    }

    /// <summary>
    /// CGameCtnZoneFrontier 0x003 chunk
    /// </summary>
    [Chunk(0x0305E003)]
    public partial class Chunk0305E003 : Chunk<CGameCtnZoneFrontier>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305E003;


        public override void ReadWrite(CGameCtnZoneFrontier n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.frontierParentBorder_AcceptPylons);
            rw.Boolean(ref n.frontierChildBorder_AcceptPylons);
        }
    }

    /// <summary>
    /// CGameCtnZoneFrontier 0x004 chunk
    /// </summary>
    [Chunk(0x0305E004)]
    public partial class Chunk0305E004 : Chunk0305E003
    {
        /// <inheritdoc />
        public override uint Id => 0x0305E004;


        public override void ReadWrite(CGameCtnZoneFrontier n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref n.frontierTransitionMiddle_AcceptPylons);
            rw.Boolean(ref n.frontierStraightMiddle_AcceptPylons);
        }
    }

    /// <summary>
    /// CGameCtnZoneFrontier 0x005 skippable chunk
    /// </summary>
    [Chunk(0x0305E005)]
    public partial class Chunk0305E005 : SkippableChunk<CGameCtnZoneFrontier>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305E005;


        public override void ReadWrite(CGameCtnZoneFrontier n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CGameCtnZoneFusionInfo>(ref n.compatibleZones!);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0305E001 => new Chunk0305E001(),
        0x0305E002 => new Chunk0305E002(),
        0x0305E003 => new Chunk0305E003(),
        0x0305E004 => new Chunk0305E004(),
        0x0305E005 => new Chunk0305E005(),
        _ => base.NewChunk(chunkId),
    };
}
