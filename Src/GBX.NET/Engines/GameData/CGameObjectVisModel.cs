namespace GBX.NET.Engines.GameData;

[Node(0x2E007000)]
public class CGameObjectVisModel : CMwNod
{
    private string? mesh;
    private string? soundRefSpawn;
    private string? soundRefUnspawn;
    private string? soundRefGrab;
    private string? smashParticleRef;
    private string? visEntFx;
    private CMwNod? meshShadedFid;
    private Vec3? domeShaderColor;
    private CPlugSolid2Model? meshShaded;

    [NodeMember(ExactlyNamed = true)]
    public string? Mesh { get => mesh; set => mesh = value; }

    [NodeMember(ExactlyNamed = false, ExactName = "SoundRef_Spawn")]
    public string? SoundRefSpawn { get => soundRefSpawn; set => soundRefSpawn = value; }

    [NodeMember(ExactlyNamed = false, ExactName = "SoundRef_Unspawn")]
    public string? SoundRefUnspawn { get => soundRefUnspawn; set => soundRefUnspawn = value; }

    [NodeMember(ExactlyNamed = false, ExactName = "SoundRef_Grab")]
    public string? SoundRefGrab { get => soundRefGrab; set => soundRefGrab = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? SmashParticleRef { get => smashParticleRef; set => smashParticleRef = value; }

    [NodeMember(ExactlyNamed = true)]
    public string? VisEntFx { get => visEntFx; set => visEntFx = value; }

    [NodeMember(ExactlyNamed = true)]
    public CMwNod? MeshShadedFid { get => meshShadedFid; set => meshShadedFid = value; }

    [NodeMember(ExactlyNamed = true)]
    public Vec3? DomeShaderColor { get => domeShaderColor; set => domeShaderColor = value; }

    [NodeMember(ExactlyNamed = true)]
    public CPlugSolid2Model? MeshShaded { get => meshShaded; set => meshShaded = value; }

    protected CGameObjectVisModel()
    {
        
    }


    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameObjectVisModel 0x001 chunk
    /// </summary>
    [Chunk(0x2E007001)]
    public class Chunk2E007001 : Chunk<CGameObjectVisModel>, IVersionable
    {
        private int version;

        public CMwNod? U01;
        public CMwNod? U02;
        public int U03;
        public string? U04;
        public string? U05;
        public string? U06;
        public string? U07;
        public int? U08;
        public int? U09;
        public CMwNod? U10;
        public CMwNod? U11;
        public float? U12;
        public CMwNod? U13;
        public CMwNod? U14;
        public CMwNod? U15;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameObjectVisModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version < 9)
            {
                rw.NodeRef(ref U14);
                rw.NodeRef(ref U15);
            }

            rw.String(ref n.mesh);

            rw.NodeRef<CPlugSolid2Model>(ref n.meshShaded);

            if (version >= 7) // Condition not seen in code
            {
                rw.NodeRef(ref U02);
            }

            rw.Int32(ref U03); // SPlugLightBallStateSimple array

            if (version < 17)
            {
                rw.String(ref n.soundRefSpawn);
                rw.String(ref n.soundRefUnspawn);
                rw.String(ref n.soundRefGrab);
            }

            if (version >= 10)
            {
                rw.String(ref U07);

                if (version >= 12)
                {
                    rw.String(ref n.smashParticleRef);

                    if (version >= 13)
                    {
                        rw.Int32(ref U08); // CPlugFileImg array
                        rw.Int32(ref U09); // SSpriteParam array

                        if (version >= 16)
                        {
                            rw.NodeRef(ref U10);
                            rw.NodeRef(ref U11);

                            if (version >= 19)
                            {
                                rw.Single(ref U12);

                                if (version >= 20)
                                {
                                    rw.String(ref n.visEntFx);
                                    // if empty then maybe nodref? proly not

                                    if (string.IsNullOrEmpty(n.visEntFx))
                                    {
                                        rw.NodeRef(ref U13);
                                    }

                                    if (version >= 21)
                                    {
                                        rw.NodeRef(ref n.meshShadedFid);

                                        if (version >= 22)
                                        {
                                            rw.Vec3(ref n.domeShaderColor);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #endregion
}
