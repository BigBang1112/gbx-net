namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A05E000</remarks>
[Class(0x0A05E000)]
public partial class CSceneMobilLeaves : CSceneMobil, IClass
{
    [Hexadecimal] public static new uint Id => 0x0A05E000;




    private CPlugShader? leafShader;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public CPlugShader? LeafShader { get => leafShaderFile?.GetNode(ref leafShader) ?? leafShader; set => leafShader = value; }
    private Components.GbxRefTableFile? leafShaderFile;
    public Components.GbxRefTableFile? LeafShaderFile { get => leafShaderFile; set => leafShaderFile = value; }
    public CPlugShader? GetLeafShader(GbxReadSettings settings = default, bool exceptions = false) => leafShaderFile?.GetNode(ref leafShader, settings, exceptions) ?? leafShader;

    private float leafRadiusBase;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafRadiusBase { get => leafRadiusBase; set => leafRadiusBase = value; }

    private float leafRadiusRandom;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafRadiusRandom { get => leafRadiusRandom; set => leafRadiusRandom = value; }

    private int leafMaxCount;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public int LeafMaxCount { get => leafMaxCount; set => leafMaxCount = value; }

    private float leafFallingSpeedBase;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafFallingSpeedBase { get => leafFallingSpeedBase; set => leafFallingSpeedBase = value; }

    private float leafAlphaSpeedMax;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafAlphaSpeedMax { get => leafAlphaSpeedMax; set => leafAlphaSpeedMax = value; }

    private float leafBetaSpeedlMax;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafBetaSpeedlMax { get => leafBetaSpeedlMax; set => leafBetaSpeedlMax = value; }

    private Vec3 wind;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public Vec3 Wind { get => wind; set => wind = value; }

    private float respawnPeriod;
    [AppliedWithChunk<Chunk0A05E000>]
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float RespawnPeriod { get => respawnPeriod; set => respawnPeriod = value; }

    private float leafFallingSpeedRandom;
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafFallingSpeedRandom { get => leafFallingSpeedRandom; set => leafFallingSpeedRandom = value; }

    private float leafOscillationAmplitudeBase;
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafOscillationAmplitudeBase { get => leafOscillationAmplitudeBase; set => leafOscillationAmplitudeBase = value; }

    private float leafOscillationAmplitudeRandom;
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafOscillationAmplitudeRandom { get => leafOscillationAmplitudeRandom; set => leafOscillationAmplitudeRandom = value; }

    private float leafOscillationPeriodBase;
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafOscillationPeriodBase { get => leafOscillationPeriodBase; set => leafOscillationPeriodBase = value; }

    private float leafOscillationPeriodRandom;
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E002>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float LeafOscillationPeriodRandom { get => leafOscillationPeriodRandom; set => leafOscillationPeriodRandom = value; }

    private float farZ;
    [AppliedWithChunk<Chunk0A05E001>]
    [AppliedWithChunk<Chunk0A05E003>]
    public float FarZ { get => farZ; set => farZ = value; }

    private int leafEmitterMaxCount;
    [AppliedWithChunk<Chunk0A05E003>]
    public int LeafEmitterMaxCount { get => leafEmitterMaxCount; set => leafEmitterMaxCount = value; }

    private float curvature;
    [AppliedWithChunk<Chunk0A05E003>]
    public float Curvature { get => curvature; set => curvature = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CSceneMobilLeaves"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CSceneMobilLeaves() { }


    /// <summary>
    /// CSceneMobilLeaves 0x000 chunk
    /// </summary>
    [Chunk(0x0A05E000)]
    public partial class Chunk0A05E000 : Chunk<CSceneMobilLeaves>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A05E000;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;
        public float U08;
        public float U09;
        public float U10;

        public override void ReadWrite(CSceneMobilLeaves n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugShader>(ref n.leafShader, ref n.leafShaderFile);
            rw.Single(ref n.leafRadiusBase);
            rw.Single(ref n.leafRadiusRandom);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Int32(ref n.leafMaxCount);
            rw.Single(ref n.leafFallingSpeedBase);
            rw.Single(ref n.leafAlphaSpeedMax);
            rw.Single(ref n.leafBetaSpeedlMax);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Vec3(ref n.wind);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Single(ref n.respawnPeriod);
            rw.Single(ref U10);
        }
    }

    /// <summary>
    /// CSceneMobilLeaves 0x001 chunk
    /// </summary>
    [Chunk(0x0A05E001)]
    public partial class Chunk0A05E001 : Chunk<CSceneMobilLeaves>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A05E001;


        public override void ReadWrite(CSceneMobilLeaves n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugShader>(ref n.leafShader, ref n.leafShaderFile);
            rw.Single(ref n.leafRadiusBase);
            rw.Single(ref n.leafRadiusRandom);
            rw.Int32(ref n.leafMaxCount);
            rw.Single(ref n.leafFallingSpeedBase);
            rw.Single(ref n.leafFallingSpeedRandom);
            rw.Single(ref n.leafAlphaSpeedMax);
            rw.Single(ref n.leafBetaSpeedlMax);
            rw.Single(ref n.leafOscillationAmplitudeBase);
            rw.Single(ref n.leafOscillationAmplitudeRandom);
            rw.Single(ref n.leafOscillationPeriodBase);
            rw.Single(ref n.leafOscillationPeriodRandom);
            rw.Vec3(ref n.wind);
            rw.Single(ref n.respawnPeriod);
            rw.Single(ref n.farZ);
        }
    }

    /// <summary>
    /// CSceneMobilLeaves 0x002 chunk
    /// </summary>
    [Chunk(0x0A05E002)]
    public partial class Chunk0A05E002 : Chunk<CSceneMobilLeaves>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A05E002;


        public override void ReadWrite(CSceneMobilLeaves n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugShader>(ref n.leafShader, ref n.leafShaderFile);
            rw.Single(ref n.leafRadiusBase);
            rw.Single(ref n.leafRadiusRandom);
            rw.Int32(ref n.leafMaxCount);
            rw.Single(ref n.leafFallingSpeedBase);
            rw.Single(ref n.leafFallingSpeedRandom);
            rw.Single(ref n.leafAlphaSpeedMax);
            rw.Single(ref n.leafBetaSpeedlMax);
            rw.Single(ref n.leafOscillationAmplitudeBase);
            rw.Single(ref n.leafOscillationAmplitudeRandom);
            rw.Single(ref n.leafOscillationPeriodBase);
            rw.Single(ref n.leafOscillationPeriodRandom);
            rw.Vec3(ref n.wind);
            rw.Single(ref n.respawnPeriod);
        }
    }

    /// <summary>
    /// CSceneMobilLeaves 0x003 chunk
    /// </summary>
    [Chunk(0x0A05E003)]
    public partial class Chunk0A05E003 : Chunk<CSceneMobilLeaves>
    {
        /// <inheritdoc />
        public override uint Id => 0x0A05E003;


        public override void ReadWrite(CSceneMobilLeaves n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugShader>(ref n.leafShader, ref n.leafShaderFile);
            rw.Single(ref n.leafRadiusBase);
            rw.Single(ref n.leafRadiusRandom);
            rw.Int32(ref n.leafMaxCount);
            rw.Int32(ref n.leafEmitterMaxCount);
            rw.Single(ref n.leafFallingSpeedBase);
            rw.Single(ref n.leafFallingSpeedRandom);
            rw.Single(ref n.leafAlphaSpeedMax);
            rw.Single(ref n.leafBetaSpeedlMax);
            rw.Single(ref n.leafOscillationAmplitudeBase);
            rw.Single(ref n.leafOscillationAmplitudeRandom);
            rw.Single(ref n.leafOscillationPeriodBase);
            rw.Single(ref n.leafOscillationPeriodRandom);
            rw.Vec3(ref n.wind);
            rw.Single(ref n.respawnPeriod);
            rw.Single(ref n.farZ);
            rw.Single(ref n.curvature);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0A05E000 => new Chunk0A05E000(),
        0x0A05E001 => new Chunk0A05E001(),
        0x0A05E002 => new Chunk0A05E002(),
        0x0A05E003 => new Chunk0A05E003(),
        _ => base.NewChunk(chunkId),
    };
}
