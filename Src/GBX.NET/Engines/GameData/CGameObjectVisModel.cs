namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E007000</remarks>
[Node(0x2E007000)]
public class CGameObjectVisModel : CMwNod
{
    #region Fields

    private string? mesh;
    private string? soundRefSpawn;
    private string? soundRefUnspawn;
    private string? soundRefGrab;
    private string? soundRefSmashed;
    private string? soundRefPermanent;
    private Iso4? soundLocPermanent;
    private string? smashParticleRef;
    private string? visEntFx;
    private CMwNod? meshShadedFid;
    private Vec3? domeShaderColor;
    private CPlugSolid2Model? meshShaded;

    #endregion

    #region Properties

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E007001))]
    public string? Mesh { get => mesh; set => mesh = value; }

    [NodeMember(ExactName = "SoundRef_Spawn")]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 0, upToVersion: 16)]
    [AppliedWithChunk(typeof(Chunk2E007002))]
    public string? SoundRefSpawn
    {
        get
        {
            DiscoverChunk<Chunk2E007002>();
            return soundRefSpawn;
        }
        set
        {
            DiscoverChunk<Chunk2E007002>();
            soundRefSpawn = value;
        }
    }

    [NodeMember(ExactName = "SoundRef_Unspawn")]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 0, upToVersion: 16)]
    [AppliedWithChunk(typeof(Chunk2E007002))]
    public string? SoundRefUnspawn
    {
        get
        {
            DiscoverChunk<Chunk2E007002>();
            return soundRefUnspawn;
        }
        set
        {
            DiscoverChunk<Chunk2E007002>();
            soundRefUnspawn = value;
        }
    }

    [NodeMember(ExactName = "SoundRef_Grab")]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 0, upToVersion: 16)]
    [AppliedWithChunk(typeof(Chunk2E007002))]
    public string? SoundRefGrab
    {
        get
        {
            DiscoverChunk<Chunk2E007002>();
            return soundRefGrab;
        }
        set
        {
            DiscoverChunk<Chunk2E007002>();
            soundRefGrab = value;
        }
    }

    [NodeMember(ExactName = "SoundRef_Smashed")]
    [AppliedWithChunk(typeof(Chunk2E007002))]
    public string? SoundRefSmashed
    {
        get
        {
            DiscoverChunk<Chunk2E007002>();
            return soundRefSmashed;
        }
        set
        {
            DiscoverChunk<Chunk2E007002>();
            soundRefSmashed = value;
        }
    }

    [NodeMember(ExactName = "SoundRef_Permanent")]
    [AppliedWithChunk(typeof(Chunk2E007002))]
    public string? SoundRefPermanent
    {
        get
        {
            DiscoverChunk<Chunk2E007002>();
            return soundRefPermanent;
        }
        set
        {
            DiscoverChunk<Chunk2E007002>();
            soundRefPermanent = value;
        }
    }

    [NodeMember(ExactName = "SoundLoc_Permanent")]
    [AppliedWithChunk(typeof(Chunk2E007002))]
    public Iso4? SoundLocPermanent
    {
        get
        {
            DiscoverChunk<Chunk2E007002>();
            return soundLocPermanent;
        }
        set
        {
            DiscoverChunk<Chunk2E007002>();
            soundLocPermanent = value;
        }
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 12)]
    public string? SmashParticleRef { get => smashParticleRef; set => smashParticleRef = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 20)]
    public string? VisEntFx { get => visEntFx; set => visEntFx = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 21)]
    public CMwNod? MeshShadedFid { get => meshShadedFid; set => meshShadedFid = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E007001), sinceVersion: 22)]
    public Vec3? DomeShaderColor { get => domeShaderColor; set => domeShaderColor = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E007001))]
    public CPlugSolid2Model? MeshShaded { get => meshShaded; set => meshShaded = value; }

    #endregion

    #region Constructors

    internal CGameObjectVisModel()
    {
        
    }

    #endregion

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
        public CMwNod? U16;

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

            if (version < 18)
            {
                rw.NodeRef(ref U16); // CPlugAnimFile
            }

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

    #region 0x002 skippable chunk

    /// <summary>
    /// CGameObjectVisModel 0x002 skippable chunk
    /// </summary>
    [Chunk(0x2E007002)]
    public class Chunk2E007002 : SkippableChunk<CGameObjectVisModel>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameObjectVisModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.String(ref n.soundRefSpawn);
            rw.String(ref n.soundRefUnspawn);
            rw.String(ref n.soundRefGrab);
            rw.String(ref n.soundRefSmashed);
            rw.String(ref n.soundRefPermanent);
            rw.Iso4(ref n.soundLocPermanent);
        }
    }

    #endregion

    #endregion
}
