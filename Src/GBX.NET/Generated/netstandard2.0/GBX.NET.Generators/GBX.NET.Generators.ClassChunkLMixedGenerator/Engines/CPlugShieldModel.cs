namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09111000</remarks>
[Class(0x09111000)]
public partial class CPlugShieldModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x09111000;




    private CPlugSound? shieldActiveSound;
    [AppliedWithChunk<Chunk09111000>]
    public CPlugSound? ShieldActiveSound { get => shieldActiveSound; set => shieldActiveSound = value; }

    private CPlugSound? shieldTouchSound;
    [AppliedWithChunk<Chunk09111000>]
    public CPlugSound? ShieldTouchSound { get => shieldTouchSound; set => shieldTouchSound = value; }

    private CPlugSound? shieldDestroySound;
    [AppliedWithChunk<Chunk09111000>]
    public CPlugSound? ShieldDestroySound { get => shieldDestroySound; set => shieldDestroySound = value; }

    private string? shieldTouchParticleRef;
    [AppliedWithChunk<Chunk09111000>]
    public string? ShieldTouchParticleRef { get => shieldTouchParticleRef; set => shieldTouchParticleRef = value; }

    private CPlugParticleEmitterModel? shieldTouchParticle;
    [AppliedWithChunk<Chunk09111000>]
    public CPlugParticleEmitterModel? ShieldTouchParticle { get => shieldTouchParticle; set => shieldTouchParticle = value; }

    private int textureNotches;
    [AppliedWithChunk<Chunk09111000>]
    public int TextureNotches { get => textureNotches; set => textureNotches = value; }

    private Vec3 relativePos;
    [AppliedWithChunk<Chunk09111000>]
    public Vec3 RelativePos { get => relativePos; set => relativePos = value; }

    private bool needActivation;
    [AppliedWithChunk<Chunk09111000>]
    public bool NeedActivation { get => needActivation; set => needActivation = value; }

    private bool isBouncing;
    [AppliedWithChunk<Chunk09111000>]
    public bool IsBouncing { get => isBouncing; set => isBouncing = value; }

    private int shieldArmor;
    [AppliedWithChunk<Chunk09111000>]
    public int ShieldArmor { get => shieldArmor; set => shieldArmor = value; }

    private int shieldDuration;
    [AppliedWithChunk<Chunk09111000>]
    public int ShieldDuration { get => shieldDuration; set => shieldDuration = value; }

    private string? shapeRef;
    [AppliedWithChunk<Chunk09111000>]
    public string? ShapeRef { get => shapeRef; set => shapeRef = value; }

    private CPlugSurface? shape;
    [AppliedWithChunk<Chunk09111000>]
    public CPlugSurface? Shape { get => shape; set => shape = value; }

    private string? shapeVisModelRef;
    [AppliedWithChunk<Chunk09111000>]
    public string? ShapeVisModelRef { get => shapeVisModelRef; set => shapeVisModelRef = value; }

    private CPlugSolid2Model? shapeVisModel;
    [AppliedWithChunk<Chunk09111000>]
    public CPlugSolid2Model? ShapeVisModel { get => shapeVisModel; set => shapeVisModel = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugShieldModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugShieldModel() { }


    /// <summary>
    /// CPlugShieldModel 0x000 chunk
    /// </summary>
    [Chunk(0x09111000)]
    public partial class Chunk09111000 : Chunk<CPlugShieldModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09111000;

        public int Version { get; set; }

        public int U01;
        public float U02;
        public float U03;
        public int U04;
        public float U05;
        public bool U06;
        public bool U07;

        public override void ReadWrite(CPlugShieldModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 7)
            {
                rw.NodeRef<CPlugSound>(ref n.shieldActiveSound);
                rw.NodeRef<CPlugSound>(ref n.shieldTouchSound);
                rw.NodeRef<CPlugSound>(ref n.shieldDestroySound);
                rw.String(ref n.shieldTouchParticleRef);
                if (n.ShieldTouchParticleRef==null||n.ShieldTouchParticleRef=="")
                {
                    rw.NodeRef<CPlugParticleEmitterModel>(ref n.shieldTouchParticle);
                }
            }
            if (Version >= 6)
            {
                rw.Int32(ref U01);
                rw.Single(ref U02);
                rw.Single(ref U03);
                rw.Int32(ref U04);
            }
            if (Version >= 5)
            {
                rw.Single(ref U05);
                rw.Boolean(ref U06);
                rw.Boolean(ref U07);
            }
            if (Version >= 4)
            {
                rw.Int32(ref n.textureNotches);
            }
            if (Version >= 3)
            {
                rw.Vec3(ref n.relativePos);
            }
            if (Version >= 2)
            {
                rw.Boolean(ref n.needActivation);
            }
            if (Version >= 1)
            {
                rw.Boolean(ref n.isBouncing);
                rw.Int32(ref n.shieldArmor);
                rw.Int32(ref n.shieldDuration);
            }
            rw.String(ref n.shapeRef);
            if (n.ShapeRef==null||n.ShapeRef=="")
            {
                rw.NodeRef<CPlugSurface>(ref n.shape);
            }
            rw.String(ref n.shapeVisModelRef);
            if (n.ShapeVisModelRef==null||n.ShapeVisModelRef=="")
            {
                rw.NodeRef<CPlugSolid2Model>(ref n.shapeVisModel);
            }
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09111000 => new Chunk09111000(),
        _ => base.NewChunk(chunkId),
    };
}
