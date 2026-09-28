namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E027000</remarks>
[Class(0x2E027000)]
public partial class CGameCommonItemEntityModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E027000;




    private CMwNod? phyModel;
    [AppliedWithChunk<Chunk2E027000>]
    public CMwNod? PhyModel { get => phyModel; set => phyModel = value; }

    private CMwNod? visModel;
    [AppliedWithChunk<Chunk2E027000>]
    public CMwNod? VisModel { get => visModel; set => visModel = value; }

    private CPlugStaticObjectModel? staticObject;
    [AppliedWithChunk<Chunk2E027000>]
    public CPlugStaticObjectModel? StaticObject { get => staticObject; set => staticObject = value; }

    private CMwNod? triggerShape;
    [AppliedWithChunk<Chunk2E027000>]
    public CMwNod? TriggerShape { get => triggerShape; set => triggerShape = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCommonItemEntityModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCommonItemEntityModel() { }


    /// <summary>
    /// CGameCommonItemEntityModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E027000)]
    public partial class Chunk2E027000 : Chunk<CGameCommonItemEntityModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E027000;

        public int Version { get; set; }

        public string? U01;
        public string? U02;
        public Iso4 U03;
        public CPlugParticleEmitterModel? U04;
        public CGameActionModel[]? U05;
        public CMwNod? U06;
        public string? U07;
        public string? U08;
        public string? U09;
        public string? U10;
        public string? U11;
        public Iso4 U12;
        /// <summary>
        /// ExprValidator
        /// </summary>
        public int U13;
        public byte U14;

        public override void ReadWrite(CGameCommonItemEntityModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 0)
            {
                rw.NodeRef<CMwNod>(ref n.phyModel);
                rw.NodeRef<CMwNod>(ref n.visModel);
            }
            if (Version == 3)
            {
                rw.String(ref U01);
                rw.String(ref U02);
            }
            if (Version >= 4)
            {
                rw.NodeRef<CPlugStaticObjectModel>(ref n.staticObject);
            }
            if (Version >= 2)
            {
                rw.NodeRef<CMwNod>(ref n.triggerShape);
                rw.Iso4(ref U03);
                rw.NodeRef<CPlugParticleEmitterModel>(ref U04);
                rw.ArrayNodeRef<CGameActionModel>(ref U05!);
                if (Version <= 5)
                {
                    rw.NodeRef<CMwNod>(ref U06);
                }
                rw.String(ref U07);
                rw.String(ref U08);
                rw.String(ref U09);
                rw.String(ref U10);
                rw.String(ref U11);
                rw.Iso4(ref U12);
                rw.Int32(ref U13); // ExprValidator
                if (Version >= 5)
                {
                    rw.Byte(ref U14);
                }
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E027000 => new Chunk2E027000(),
        _ => base.NewChunk(chunkId),
    };
}
