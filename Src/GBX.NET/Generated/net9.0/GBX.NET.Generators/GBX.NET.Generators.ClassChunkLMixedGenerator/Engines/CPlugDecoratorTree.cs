namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090A2000</remarks>
[Class(0x090A2000)]
public partial class CPlugDecoratorTree : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090A2000;




    private string? treeId;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public string? TreeId { get => treeId; set => treeId = value; }

    private CPlugMaterial? material;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public CPlugMaterial? Material { get => material; set => material = value; }

    private CPlugTreeLight? treeLight;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public CPlugTreeLight? TreeLight { get => treeLight; set => treeLight = value; }

    private int visibleCond;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public int VisibleCond { get => visibleCond; set => visibleCond = value; }

    private bool visibleApplyOnChilds;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public bool VisibleApplyOnChilds { get => visibleApplyOnChilds; set => visibleApplyOnChilds = value; }

    private int shadowCasterCond;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public int ShadowCasterCond { get => shadowCasterCond; set => shadowCasterCond = value; }

    private bool shadowCasterApplyOnChilds;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public bool ShadowCasterApplyOnChilds { get => shadowCasterApplyOnChilds; set => shadowCasterApplyOnChilds = value; }

    private bool transformVisualToSurface;
    [AppliedWithChunk<Chunk090A2006>]
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public bool TransformVisualToSurface { get => transformVisualToSurface; set => transformVisualToSurface = value; }

    private int existCond;
    [AppliedWithChunk<Chunk090A2007>]
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public int ExistCond { get => existCond; set => existCond = value; }

    private bool noLocation;
    [AppliedWithChunk<Chunk090A2008>]
    [AppliedWithChunk<Chunk090A2009>]
    public bool NoLocation { get => noLocation; set => noLocation = value; }

    private int collidableCond;
    [AppliedWithChunk<Chunk090A2009>]
    public int CollidableCond { get => collidableCond; set => collidableCond = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugDecoratorTree"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugDecoratorTree() { }


    /// <summary>
    /// CPlugDecoratorTree 0x006 chunk
    /// </summary>
    [Chunk(0x090A2006)]
    public partial class Chunk090A2006 : Chunk<CPlugDecoratorTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x090A2006;


        public override void ReadWrite(CPlugDecoratorTree n, GbxReaderWriter rw)
        {
            rw.Id(ref n.treeId);
            rw.NodeRef<CPlugMaterial>(ref n.material);
            rw.NodeRef<CPlugTreeLight>(ref n.treeLight);
            rw.Int32(ref n.visibleCond);
            rw.Boolean(ref n.visibleApplyOnChilds);
            rw.Int32(ref n.shadowCasterCond);
            rw.Boolean(ref n.shadowCasterApplyOnChilds);
            rw.Boolean(ref n.transformVisualToSurface);
        }
    }

    /// <summary>
    /// CPlugDecoratorTree 0x007 chunk
    /// </summary>
    [Chunk(0x090A2007)]
    public partial class Chunk090A2007 : Chunk<CPlugDecoratorTree>
    {
        /// <inheritdoc />
        public override uint Id => 0x090A2007;


        public override void ReadWrite(CPlugDecoratorTree n, GbxReaderWriter rw)
        {
            rw.Id(ref n.treeId);
            rw.NodeRef<CPlugMaterial>(ref n.material);
            rw.NodeRef<CPlugTreeLight>(ref n.treeLight);
            rw.Int32(ref n.existCond);
            rw.Int32(ref n.visibleCond);
            rw.Boolean(ref n.visibleApplyOnChilds);
            rw.Int32(ref n.shadowCasterCond);
            rw.Boolean(ref n.shadowCasterApplyOnChilds);
            rw.Boolean(ref n.transformVisualToSurface);
        }
    }

    /// <summary>
    /// CPlugDecoratorTree 0x008 chunk
    /// </summary>
    [Chunk(0x090A2008)]
    public partial class Chunk090A2008 : Chunk090A2007
    {
        /// <inheritdoc />
        public override uint Id => 0x090A2008;


        public override void ReadWrite(CPlugDecoratorTree n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Boolean(ref n.noLocation);
        }
    }

    /// <summary>
    /// CPlugDecoratorTree 0x009 chunk
    /// </summary>
    [Chunk(0x090A2009)]
    public partial class Chunk090A2009 : Chunk090A2008
    {
        /// <inheritdoc />
        public override uint Id => 0x090A2009;


        public override void ReadWrite(CPlugDecoratorTree n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);
            rw.Int32(ref n.collidableCond);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090A2006 => new Chunk090A2006(),
        0x090A2007 => new Chunk090A2007(),
        0x090A2008 => new Chunk090A2008(),
        0x090A2009 => new Chunk090A2009(),
        _ => base.NewChunk(chunkId),
    };
}
