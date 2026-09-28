namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0910E000</remarks>
[Class(0x0910E000)]
public partial class CPlugVehicleCarPhyShape : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0910E000;




    private CPlugSurface? moveShape;
    [AppliedWithChunk<Chunk0910E000>]
    public CPlugSurface? MoveShape { get => moveShapeFile?.GetNode(ref moveShape) ?? moveShape; set => moveShape = value; }
    private Components.GbxRefTableFile? moveShapeFile;
    public Components.GbxRefTableFile? MoveShapeFile { get => moveShapeFile; set => moveShapeFile = value; }
    public CPlugSurface? GetMoveShape(GbxReadSettings settings = default, bool exceptions = false) => moveShapeFile?.GetNode(ref moveShape, settings, exceptions) ?? moveShape;

    private BoxAligned carVsCarShapeBox;
    [AppliedWithChunk<Chunk0910E000>]
    public BoxAligned CarVsCarShapeBox { get => carVsCarShapeBox; set => carVsCarShapeBox = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugVehicleCarPhyShape"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugVehicleCarPhyShape() { }


    /// <summary>
    /// CPlugVehicleCarPhyShape 0x000 chunk
    /// </summary>
    [Chunk(0x0910E000)]
    public partial class Chunk0910E000 : Chunk<CPlugVehicleCarPhyShape>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0910E000;

        public int Version { get; set; }

        public float U01;
        public CPlugSolid? U02;
        public CPlugVehicleWheelPhyModel[]? U04;
        public BoxAligned U05;
        public bool U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;
        public float U11;
        public float U12;
        public float U13;
        public float U14;
        public float U15;
        public Vec4[]? U16;
        public bool U18;
        public bool U19;
        public float U20;
        public float U21;
        public bool U22;
        public float U23;
        public bool U24;
        public float U25;
        public bool U26;
        public float U27;
        public float U28;
        public float U29;
        public float U30;
        public float U31;
        public float U32;
        public string? U33;
        public CPlugSurface? U34;
        public Vec3[]? U35;
        public CPlugSurface? U36;
        public CPlugSurface? U37;
        public CPlugSurface? U38;

        public override void ReadWrite(CPlugVehicleCarPhyShape n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Single(ref U01);
            rw.NodeRef<CPlugSolid>(ref U02);
            if (Version >= 1)
            {
                if (U02==null)
                {
                    rw.NodeRef<CPlugSurface>(ref n.moveShape, ref n.moveShapeFile);
                }
            }
            rw.ArrayReadableWritable<CPlugVehicleWheelPhyModel>(ref U04!);
            rw.BoxAligned(ref U05);
            rw.Boolean(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref U10);
            rw.Single(ref U11);
            rw.Single(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Array<Vec4>(ref U16!);
            rw.BoxAligned(ref n.carVsCarShapeBox);
            rw.Boolean(ref U18);
            rw.Boolean(ref U19);
            rw.Single(ref U20);
            rw.Single(ref U21);
            rw.Boolean(ref U22);
            rw.Single(ref U23);
            rw.Boolean(ref U24);
            rw.Single(ref U25);
            rw.Boolean(ref U26);
            rw.Single(ref U27);
            rw.Single(ref U28);
            rw.Single(ref U29);
            if (Version >= 2)
            {
                rw.Single(ref U30);
                rw.Single(ref U31);
                rw.Single(ref U32);
                if (Version >= 3)
                {
                    rw.String(ref U33);
                    if (U33==null||U33=="")
                    {
                        rw.NodeRef<CPlugSurface>(ref U34);
                    }
                    if (Version >= 4)
                    {
                        rw.Array<Vec3>(ref U35!);
                        if (Version >= 5)
                        {
                            rw.NodeRef<CPlugSurface>(ref U36);
                            if (Version >= 6)
                            {
                                rw.NodeRef<CPlugSurface>(ref U37);
                                rw.NodeRef<CPlugSurface>(ref U38);
                            }
                        }
                    }
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0910E000 => new Chunk0910E000(),
        _ => base.NewChunk(chunkId),
    };
}
