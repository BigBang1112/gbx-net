namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0911C000</remarks>
[Class(0x0911C000)]
public partial class CPlugTrainWagonModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0911C000;




    private CPlugSurface? shape;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugSurface? Shape { get => shapeFile?.GetNode(ref shape) ?? shape; set => shape = value; }
    private Components.GbxRefTableFile? shapeFile;
    public Components.GbxRefTableFile? ShapeFile { get => shapeFile; set => shapeFile = value; }
    public CPlugSurface? GetShape(GbxReadSettings settings = default, bool exceptions = false) => shapeFile?.GetNode(ref shape, settings, exceptions) ?? shape;

    private CPlugSolid2Model? mesh;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugSolid2Model? Mesh { get => meshFile?.GetNode(ref mesh) ?? mesh; set => mesh = value; }
    private Components.GbxRefTableFile? meshFile;
    public Components.GbxRefTableFile? MeshFile { get => meshFile; set => meshFile = value; }
    public CPlugSolid2Model? GetMesh(GbxReadSettings settings = default, bool exceptions = false) => meshFile?.GetNode(ref mesh, settings, exceptions) ?? mesh;

    private bool isLoco;
    [AppliedWithChunk<Chunk0911C000>]
    public bool IsLoco { get => isLoco; set => isLoco = value; }

    private CPlugSound? soundEngine;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugSound? SoundEngine { get => soundEngine; set => soundEngine = value; }

    private CPlugSound? soundBrake;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugSound? SoundBrake { get => soundBrake; set => soundBrake = value; }

    private CPlugSound? soundRailContact;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugSound? SoundRailContact { get => soundRailContactFile?.GetNode(ref soundRailContact) ?? soundRailContact; set => soundRailContact = value; }
    private Components.GbxRefTableFile? soundRailContactFile;
    public Components.GbxRefTableFile? SoundRailContactFile { get => soundRailContactFile; set => soundRailContactFile = value; }
    public CPlugSound? GetSoundRailContact(GbxReadSettings settings = default, bool exceptions = false) => soundRailContactFile?.GetNode(ref soundRailContact, settings, exceptions) ?? soundRailContact;

    private CPlugSound? soundCollision;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugSound? SoundCollision { get => soundCollision; set => soundCollision = value; }

    private float wagonLength;
    [AppliedWithChunk<Chunk0911C000>]
    public float WagonLength { get => wagonLength; set => wagonLength = value; }

    private float wagonColOffset;
    [AppliedWithChunk<Chunk0911C000>]
    public float WagonColOffset { get => wagonColOffset; set => wagonColOffset = value; }

    private bool genLengthFromShape;
    [AppliedWithChunk<Chunk0911C000>]
    public bool GenLengthFromShape { get => genLengthFromShape; set => genLengthFromShape = value; }

    private CFuncKeysReal? accelCurve;
    [AppliedWithChunk<Chunk0911C000>]
    public CFuncKeysReal? AccelCurve { get => accelCurve; set => accelCurve = value; }

    private CPlugParticleEmitterModel? smokeEmitterModel;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugParticleEmitterModel? SmokeEmitterModel { get => smokeEmitterModel; set => smokeEmitterModel = value; }

    private CPlugParticleEmitterModel? dustEmitterModel;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugParticleEmitterModel? DustEmitterModel { get => dustEmitterModel; set => dustEmitterModel = value; }

    private CPlugParticleEmitterModel? sparkleParticle;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugParticleEmitterModel? SparkleParticle { get => sparkleParticle; set => sparkleParticle = value; }

    private CPlugAnimFile? animFile;
    [AppliedWithChunk<Chunk0911C000>]
    public CPlugAnimFile? AnimFile { get => animFile; set => animFile = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugTrainWagonModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugTrainWagonModel() { }


    /// <summary>
    /// CPlugTrainWagonModel 0x000 chunk
    /// </summary>
    [Chunk(0x0911C000)]
    public partial class Chunk0911C000 : Chunk<CPlugTrainWagonModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x0911C000;

        public int Version { get; set; }

        public Vec3 U01;
        public CFuncKeysReal? U02;
        public CMwNod? U03;

        public override void ReadWrite(CPlugTrainWagonModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSurface>(ref n.shape, ref n.shapeFile);
            rw.NodeRef<CPlugSolid2Model>(ref n.mesh, ref n.meshFile);
            if (Version >= 2)
            {
                rw.Boolean(ref n.isLoco);
                rw.Vec3(ref U01);
                if (Version >= 3)
                {
                    rw.NodeRef<CPlugSound>(ref n.soundEngine);
                    rw.NodeRef<CPlugSound>(ref n.soundBrake);
                    rw.NodeRef<CPlugSound>(ref n.soundRailContact, ref n.soundRailContactFile);
                    rw.NodeRef<CPlugSound>(ref n.soundCollision);
                    if (Version >= 5)
                    {
                        rw.Single(ref n.wagonLength);
                        rw.Single(ref n.wagonColOffset);
                        rw.Boolean(ref n.genLengthFromShape);
                        if (Version >= 6)
                        {
                            rw.NodeRef<CFuncKeysReal>(ref n.accelCurve);
                            if (Version >= 7)
                            {
                                rw.NodeRef<CPlugParticleEmitterModel>(ref n.smokeEmitterModel);
                                if (Version >= 8)
                                {
                                    rw.NodeRef<CFuncKeysReal>(ref U02);
                                    rw.NodeRef<CPlugParticleEmitterModel>(ref n.dustEmitterModel);
                                    rw.NodeRef<CPlugParticleEmitterModel>(ref n.sparkleParticle);
                                    if (Version >= 9)
                                    {
                                        rw.NodeRef<CMwNod>(ref U03);
                                        if (Version >= 10)
                                        {
                                            rw.NodeRef<CPlugAnimFile>(ref n.animFile);
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




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0911C000 => new Chunk0911C000(),
        _ => base.NewChunk(chunkId),
    };
}
