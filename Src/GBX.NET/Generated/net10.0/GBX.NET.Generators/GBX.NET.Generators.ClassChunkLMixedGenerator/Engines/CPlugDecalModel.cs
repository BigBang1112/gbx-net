namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090A7000</remarks>
[Class(0x090A7000)]
public partial class CPlugDecalModel : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x090A7000;




    private CPlugBitmap? diffuseA;
    [AppliedWithChunk<Chunk090A7002>]
    [AppliedWithChunk<Chunk090A7002>]
    public CPlugBitmap? DiffuseA { get => diffuseAFile?.GetNode(ref diffuseA) ?? diffuseA; set => diffuseA = value; }
    private Components.GbxRefTableFile? diffuseAFile;
    public Components.GbxRefTableFile? DiffuseAFile { get => diffuseAFile; set => diffuseAFile = value; }
    public CPlugBitmap? GetDiffuseA(GbxReadSettings settings = default, bool exceptions = false) => diffuseAFile?.GetNode(ref diffuseA, settings, exceptions) ?? diffuseA;

    private CPlugBitmap? normal;
    [AppliedWithChunk<Chunk090A7002>]
    [AppliedWithChunk<Chunk090A7002>]
    public CPlugBitmap? Normal { get => normalFile?.GetNode(ref normal) ?? normal; set => normal = value; }
    private Components.GbxRefTableFile? normalFile;
    public Components.GbxRefTableFile? NormalFile { get => normalFile; set => normalFile = value; }
    public CPlugBitmap? GetNormal(GbxReadSettings settings = default, bool exceptions = false) => normalFile?.GetNode(ref normal, settings, exceptions) ?? normal;

    private float texelByMeter;
    [AppliedWithChunk<Chunk090A7002>]
    public float TexelByMeter { get => texelByMeter; set => texelByMeter = value; }

    private bool fadeNormalAndZ;
    [AppliedWithChunk<Chunk090A7002>]
    public bool FadeNormalAndZ { get => fadeNormalAndZ; set => fadeNormalAndZ = value; }

    private CPlugBitmap? specular;
    [AppliedWithChunk<Chunk090A7002>]
    [AppliedWithChunk<Chunk090A7002>]
    public CPlugBitmap? Specular { get => specularFile?.GetNode(ref specular) ?? specular; set => specular = value; }
    private Components.GbxRefTableFile? specularFile;
    public Components.GbxRefTableFile? SpecularFile { get => specularFile; set => specularFile = value; }
    public CPlugBitmap? GetSpecular(GbxReadSettings settings = default, bool exceptions = false) => specularFile?.GetNode(ref specular, settings, exceptions) ?? specular;

    private string? diffuseARef;
    [AppliedWithChunk<Chunk090A7002>]
    public string? DiffuseARef { get => diffuseARef; set => diffuseARef = value; }

    private string? normalRef;
    [AppliedWithChunk<Chunk090A7002>]
    public string? NormalRef { get => normalRef; set => normalRef = value; }

    private string? specularRef;
    [AppliedWithChunk<Chunk090A7002>]
    public string? SpecularRef { get => specularRef; set => specularRef = value; }

    private string? roughnessRef;
    [AppliedWithChunk<Chunk090A7002>]
    public string? RoughnessRef { get => roughnessRef; set => roughnessRef = value; }

    private CPlugBitmap? roughness;
    [AppliedWithChunk<Chunk090A7002>]
    public CPlugBitmap? Roughness { get => roughness; set => roughness = value; }

    private CPlugBitmap? icon;
    [AppliedWithChunk<Chunk090A7003>]
    public CPlugBitmap? Icon { get => iconFile?.GetNode(ref icon) ?? icon; set => icon = value; }
    private Components.GbxRefTableFile? iconFile;
    public Components.GbxRefTableFile? IconFile { get => iconFile; set => iconFile = value; }
    public CPlugBitmap? GetIcon(GbxReadSettings settings = default, bool exceptions = false) => iconFile?.GetNode(ref icon, settings, exceptions) ?? icon;

    private bool randomInstances;
    [AppliedWithChunk<Chunk090A7004>]
    public bool RandomInstances { get => randomInstances; set => randomInstances = value; }

    private CPlugBitmap? sprite3dBitmap;
    [AppliedWithChunk<Chunk090A7004>]
    public CPlugBitmap? Sprite3dBitmap { get => sprite3dBitmapFile?.GetNode(ref sprite3dBitmap) ?? sprite3dBitmap; set => sprite3dBitmap = value; }
    private Components.GbxRefTableFile? sprite3dBitmapFile;
    public Components.GbxRefTableFile? Sprite3dBitmapFile { get => sprite3dBitmapFile; set => sprite3dBitmapFile = value; }
    public CPlugBitmap? GetSprite3dBitmap(GbxReadSettings settings = default, bool exceptions = false) => sprite3dBitmapFile?.GetNode(ref sprite3dBitmap, settings, exceptions) ?? sprite3dBitmap;

    private string? sprite3dGroupId;
    [AppliedWithChunk<Chunk090A7004>]
    public string? Sprite3dGroupId { get => sprite3dGroupId; set => sprite3dGroupId = value; }

    private string? svgRef;
    [AppliedWithChunk<Chunk090A7004>]
    public string? SvgRef { get => svgRef; set => svgRef = value; }

    private CMwNod? svg;
    [AppliedWithChunk<Chunk090A7004>]
    public CMwNod? Svg { get => svgFile?.GetNode(ref svg) ?? svg; set => svg = value; }
    private Components.GbxRefTableFile? svgFile;
    public Components.GbxRefTableFile? SvgFile { get => svgFile; set => svgFile = value; }
    public CMwNod? GetSvg(GbxReadSettings settings = default, bool exceptions = false) => svgFile?.GetNode(ref svg, settings, exceptions) ?? svg;

    private float svgSize;
    [AppliedWithChunk<Chunk090A7004>]
    public float SvgSize { get => svgSize; set => svgSize = value; }

    private float svgAlpha;
    [AppliedWithChunk<Chunk090A7004>]
    public float SvgAlpha { get => svgAlpha; set => svgAlpha = value; }

    private int minAngleN3d;
    [AppliedWithChunk<Chunk090A7004>]
    public int MinAngleN3d { get => minAngleN3d; set => minAngleN3d = value; }

    private CPlugDecalModel[]? decalModels;
    [AppliedWithChunk<Chunk090A7004>]
    public CPlugDecalModel[]? DecalModels { get => decalModels; set => decalModels = value; }

    private MacroDecalSet[]? macroDecalSets;
    [AppliedWithChunk<Chunk090A7004>]
    public MacroDecalSet[]? MacroDecalSets { get => macroDecalSets; set => macroDecalSets = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CPlugDecalModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugDecalModel() { }


    /// <summary>
    /// CPlugDecalModel 0x002 chunk
    /// </summary>
    [Chunk(0x090A7002)]
    public partial class Chunk090A7002 : Chunk<CPlugDecalModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090A7002;

        public int Version { get; set; }

        public float U01;

        public override void ReadWrite(CPlugDecalModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version <= 4)
            {
                rw.NodeRef<CPlugBitmap>(ref n.diffuseA, ref n.diffuseAFile);
                rw.NodeRef<CPlugBitmap>(ref n.normal, ref n.normalFile);
            }
            if (Version >= 1)
            {
                rw.Single(ref n.texelByMeter);
                if (Version >= 2)
                {
                    rw.Boolean(ref n.fadeNormalAndZ);
                    if (Version >= 3)
                    {
                        rw.Single(ref U01);
                        if (Version == 4)
                        {
                            rw.NodeRef<CPlugBitmap>(ref n.specular, ref n.specularFile);
                        }
                        if (Version >= 5)
                        {
                            rw.String(ref n.diffuseARef);
                            if (n.DiffuseARef==null||n.DiffuseARef=="")
                            {
                                rw.NodeRef<CPlugBitmap>(ref n.diffuseA, ref n.diffuseAFile);
                            }
                            rw.String(ref n.normalRef);
                            if (n.NormalRef==null||n.NormalRef=="")
                            {
                                rw.NodeRef<CPlugBitmap>(ref n.normal, ref n.normalFile);
                            }
                            rw.String(ref n.specularRef);
                            if (n.SpecularRef==null||n.SpecularRef=="")
                            {
                                rw.NodeRef<CPlugBitmap>(ref n.specular, ref n.specularFile);
                            }
                            if (Version >= 6)
                            {
                                rw.String(ref n.roughnessRef);
                                if (n.RoughnessRef==null||n.RoughnessRef=="")
                                {
                                    rw.NodeRef<CPlugBitmap>(ref n.roughness);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugDecalModel 0x003 chunk
    /// </summary>
    [Chunk(0x090A7003)]
    public partial class Chunk090A7003 : Chunk<CPlugDecalModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090A7003;


        public override void ReadWrite(CPlugDecalModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.icon, ref n.iconFile);
        }
    }

    /// <summary>
    /// CPlugDecalModel 0x004 chunk
    /// </summary>
    [Chunk(0x090A7004)]
    public partial class Chunk090A7004 : Chunk<CPlugDecalModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090A7004;

        public int Version { get; set; }

        public CPlugSolid? U01;
        public float U02;

        public override void ReadWrite(CPlugDecalModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlugSolid>(ref U01);
            rw.Boolean(ref n.randomInstances);
            if (Version >= 2)
            {
                rw.Single(ref U02);
                if (Version >= 3)
                {
                    rw.NodeRef<CPlugBitmap>(ref n.sprite3dBitmap, ref n.sprite3dBitmapFile);
                    rw.Id(ref n.sprite3dGroupId);
                    if (Version >= 4)
                    {
                        rw.String(ref n.svgRef);
                        if (n.SvgRef==null||n.SvgRef=="")
                        {
                            rw.NodeRef<CMwNod>(ref n.svg, ref n.svgFile);
                        }
                        if (Version >= 5)
                        {
                            rw.Single(ref n.svgSize);
                            if (Version >= 6)
                            {
                                rw.Single(ref n.svgAlpha);
                                if (Version >= 7)
                                {
                                    rw.Int32(ref n.minAngleN3d);
                                    if (Version >= 8)
                                    {
                                        rw.ArrayNodeRef_deprec<CPlugDecalModel>(ref n.decalModels!);
                                        rw.ArrayReadableWritable<MacroDecalSet>(ref n.macroDecalSets!);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// CPlugDecalModel 0x006 chunk
    /// </summary>
    [Chunk(0x090A7006)]
    public partial class Chunk090A7006 : Chunk<CPlugDecalModel>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x090A7006;

        public int Version { get; set; }

        public uint U01;

        public override void ReadWrite(CPlugDecalModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt32(ref U01);
        }
    }


    public sealed partial class MacroDecalSet : IReadableWritable
    {

        private string? u01;
        public string? U01 { get => u01; set => u01 = value; }

        private BoxAligned u02;
        public BoxAligned U02 { get => u02; set => u02 = value; }

        private MacroDecal3d[]? decals;
        public MacroDecal3d[]? Decals { get => decals; set => decals = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Id(ref u01);
            rw.BoxAligned(ref u02);
            rw.ArrayReadableWritable<MacroDecal3d>(ref decals!);
        }
    }

    public sealed partial class MacroDecal3d : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private string? u02;
        public string? U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Id(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090A7002 => new Chunk090A7002(),
        0x090A7003 => new Chunk090A7003(),
        0x090A7004 => new Chunk090A7004(),
        0x090A7006 => new Chunk090A7006(),
        _ => base.NewChunk(chunkId),
    };
}
