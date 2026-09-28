namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0314D000</remarks>
[Class(0x0314D000)]
public partial class CGameCtnZoneTransition : CGameCtnZone, IClass
{
    [Hexadecimal] public static new uint Id => 0x0314D000;




    private CGameCtnBlockInfoTransition? blockInfoTransition;
    [AppliedWithChunk<Chunk0314D000>]
    public CGameCtnBlockInfoTransition? BlockInfoTransition { get => blockInfoTransitionFile?.GetNode(ref blockInfoTransition) ?? blockInfoTransition; set => blockInfoTransition = value; }
    private Components.GbxRefTableFile? blockInfoTransitionFile;
    public Components.GbxRefTableFile? BlockInfoTransitionFile { get => blockInfoTransitionFile; set => blockInfoTransitionFile = value; }
    public CGameCtnBlockInfoTransition? GetBlockInfoTransition(GbxReadSettings settings = default, bool exceptions = false) => blockInfoTransitionFile?.GetNode(ref blockInfoTransition, settings, exceptions) ?? blockInfoTransition;

    private string? replacementZoneId;
    [AppliedWithChunk<Chunk0314D005>]
    public string? ReplacementZoneId { get => replacementZoneId; set => replacementZoneId = value; }

    private EBorderType border_North;
    [AppliedWithChunk<Chunk0314D008>]
    public EBorderType Border_North { get => border_North; set => border_North = value; }

    private CGameCtnZoneGenealogy? genealogy_North;
    [AppliedWithChunk<Chunk0314D008>]
    public CGameCtnZoneGenealogy? Genealogy_North { get => genealogy_North; set => genealogy_North = value; }

    private EBorderType border_East;
    [AppliedWithChunk<Chunk0314D008>]
    public EBorderType Border_East { get => border_East; set => border_East = value; }

    private CGameCtnZoneGenealogy? genealogy_East;
    [AppliedWithChunk<Chunk0314D008>]
    public CGameCtnZoneGenealogy? Genealogy_East { get => genealogy_East; set => genealogy_East = value; }

    private EBorderType border_South;
    [AppliedWithChunk<Chunk0314D008>]
    public EBorderType Border_South { get => border_South; set => border_South = value; }

    private CGameCtnZoneGenealogy? genealogy_South;
    [AppliedWithChunk<Chunk0314D008>]
    public CGameCtnZoneGenealogy? Genealogy_South { get => genealogy_South; set => genealogy_South = value; }

    private EBorderType border_West;
    [AppliedWithChunk<Chunk0314D008>]
    public EBorderType Border_West { get => border_West; set => border_West = value; }

    private CGameCtnZoneGenealogy? genealogy_West;
    [AppliedWithChunk<Chunk0314D008>]
    public CGameCtnZoneGenealogy? Genealogy_West { get => genealogy_West; set => genealogy_West = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnZoneTransition"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnZoneTransition() { }


    /// <summary>
    /// CGameCtnZoneTransition 0x000 chunk
    /// </summary>
    [Chunk(0x0314D000)]
    public partial class Chunk0314D000 : Chunk<CGameCtnZoneTransition>
    {
        /// <inheritdoc />
        public override uint Id => 0x0314D000;


        public override void ReadWrite(CGameCtnZoneTransition n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnBlockInfoTransition>(ref n.blockInfoTransition, ref n.blockInfoTransitionFile);
        }
    }

    /// <summary>
    /// CGameCtnZoneTransition 0x005 chunk
    /// </summary>
    [Chunk(0x0314D005)]
    public partial class Chunk0314D005 : Chunk<CGameCtnZoneTransition>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0314D005;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnZoneTransition n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref n.replacementZoneId);
        }
    }

    /// <summary>
    /// CGameCtnZoneTransition 0x006 chunk
    /// </summary>
    [Chunk(0x0314D006)]
    public partial class Chunk0314D006 : Chunk<CGameCtnZoneTransition>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0314D006;

        public int Version { get; set; }

        public int U01;

        public override void ReadWrite(CGameCtnZoneTransition n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnZoneTransition 0x007 chunk
    /// </summary>
    [Chunk(0x0314D007)]
    public partial class Chunk0314D007 : Chunk<CGameCtnZoneTransition>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0314D007;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public string? U03;

        public override void ReadWrite(CGameCtnZoneTransition n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
            rw.Id(ref U02);
            rw.Id(ref U03);
        }
    }

    /// <summary>
    /// CGameCtnZoneTransition 0x008 chunk
    /// </summary>
    [Chunk(0x0314D008)]
    public partial class Chunk0314D008 : Chunk<CGameCtnZoneTransition>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0314D008;

        public int Version { get; set; }


        public override void ReadWrite(CGameCtnZoneTransition n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.EnumInt32<EBorderType>(ref n.border_North);
            rw.Node<CGameCtnZoneGenealogy>(ref n.genealogy_North);
            rw.EnumInt32<EBorderType>(ref n.border_East);
            rw.Node<CGameCtnZoneGenealogy>(ref n.genealogy_East);
            rw.EnumInt32<EBorderType>(ref n.border_South);
            rw.Node<CGameCtnZoneGenealogy>(ref n.genealogy_South);
            rw.EnumInt32<EBorderType>(ref n.border_West);
            rw.Node<CGameCtnZoneGenealogy>(ref n.genealogy_West);
        }
    }



    public enum EBorderType
    {
        Parent,
        ParentToChild,
        ChildToParent,
        Straight,
        Child,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0314D000 => new Chunk0314D000(),
        0x0314D005 => new Chunk0314D005(),
        0x0314D006 => new Chunk0314D006(),
        0x0314D007 => new Chunk0314D007(),
        0x0314D008 => new Chunk0314D008(),
        _ => base.NewChunk(chunkId),
    };
}
