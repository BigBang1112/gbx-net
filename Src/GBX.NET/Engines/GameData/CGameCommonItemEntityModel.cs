namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E027000</remarks>
[Node(0x2E027000)]
public class CGameCommonItemEntityModel : CMwNod
{
    private CMwNod? phyModel;
    private CMwNod? visModel;
    private CMwNod? staticObject;
    private CPlugSurface? triggerShape;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E027000>]
    public CMwNod? PhyModel { get => phyModel; set => phyModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E027000>]
    public CMwNod? VisModel { get => visModel; set => visModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E027000>(sinceVersion: 4)]
    public CMwNod? StaticObject { get => staticObject; set => staticObject = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk2E027000>(sinceVersion: 4)]
    public CPlugSurface? TriggerShape { get => triggerShape; set => triggerShape = value; }

    internal CGameCommonItemEntityModel()
    {
        
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCommonItemEntityModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E027000)]
    public class Chunk2E027000 : Chunk<CGameCommonItemEntityModel>, IVersionable
    {
        private int version;

        public string? U01;
        public string? U02;
        public Iso4? U03;
        public CPlugParticleEmitterModel? U04;
        public CGameActionModel?[]? U05;
        public Node? U06;
        public string? U07;
        public string? U08;
        public string? U09;
        public string? U10;
        public string? U11;
        public Iso4? U12;
        public int? U13;
        public byte? U14;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCommonItemEntityModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version == 0)
            {
                rw.NodeRef<CMwNod>(ref n.phyModel);
                rw.NodeRef<CMwNod>(ref n.visModel);
            }
            else if (version == 3)
            {
                rw.String(ref U01);
                rw.String(ref U02);
            }
            else
            {
                rw.NodeRef<CMwNod>(ref n.staticObject);
            }

            if (version >= 2)
            {
                rw.NodeRef<CPlugSurface>(ref n.triggerShape);
                rw.Iso4(ref U03);
                rw.NodeRef<CPlugParticleEmitterModel>(ref U04);
                rw.ArrayNode<CGameActionModel>(ref U05);

                if (version < 6)
                {
                    rw.NodeRef(ref U06);
                }

                rw.String(ref U07);
                rw.String(ref U08);
                rw.String(ref U09);
                rw.String(ref U10);
                rw.String(ref U11);
                rw.Iso4(ref U12);
                rw.Int32(ref U13); // ExprValidator

                if (version >= 5)
                {
                    rw.Byte(ref U14);
                }
            }
        }
    }

    #endregion
}
