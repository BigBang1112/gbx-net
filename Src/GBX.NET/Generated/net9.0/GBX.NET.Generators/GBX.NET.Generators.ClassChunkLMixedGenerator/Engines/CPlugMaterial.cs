namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09079000</remarks>
[Class(0x09079000)]
public partial class CPlugMaterial : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09079000;




    private CPlugMaterialCustom? customMaterial;
    [AppliedWithChunk<Chunk09079007>]
    public CPlugMaterialCustom? CustomMaterial { get => customMaterial; set => customMaterial = value; }

    private CPlugSurface.MaterialId surfaceId;
    [AppliedWithChunk<Chunk0907900E>]
    [AppliedWithChunk<Chunk09079017>]
    public CPlugSurface.MaterialId SurfaceId { get => surfaceId; set => surfaceId = value; }

    private CPlug? shader;
    [AppliedWithChunk<Chunk09079015>]
    public CPlug? Shader { get => shaderFile?.GetNode(ref shader) ?? shader; set => shader = value; }
    private Components.GbxRefTableFile? shaderFile;
    public Components.GbxRefTableFile? ShaderFile { get => shaderFile; set => shaderFile = value; }
    public CPlug? GetShader(GbxReadSettings settings = default, bool exceptions = false) => shaderFile?.GetNode(ref shader, settings, exceptions) ?? shader;

    private CPlugSurface.GameplayId gameplayId;
    [AppliedWithChunk<Chunk09079017>]
    public CPlugSurface.GameplayId GameplayId { get => gameplayId; set => gameplayId = value; }


    /// <summary>
    /// CPlugMaterial 0x001 chunk
    /// </summary>
    [Chunk(0x09079001)]
    public partial class Chunk09079001 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079001;

        public CMwNod? U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x002 chunk
    /// </summary>
    [Chunk(0x09079002)]
    public partial class Chunk09079002 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079002;

        public int U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x004 chunk
    /// </summary>
    [Chunk(0x09079004)]
    public partial class Chunk09079004 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079004;


        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.ArrayReadableWritable<DeviceMat>(ref n.deviceMaterials!, version: 0x004);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x007 chunk
    /// </summary>
    [Chunk(0x09079007)]
    public partial class Chunk09079007 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079007;


        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterialCustom>(ref n.customMaterial);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x009 chunk
    /// </summary>
    [Chunk(0x09079009)]
    public partial class Chunk09079009 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079009;

    }

    /// <summary>
    /// CPlugMaterial 0x00A chunk
    /// </summary>
    [Chunk(0x0907900A)]
    public partial class Chunk0907900A : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x0907900A;

        public int U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x00D chunk
    /// </summary>
    [Chunk(0x0907900D)]
    public partial class Chunk0907900D : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x0907900D;

    }

    /// <summary>
    /// CPlugMaterial 0x00E chunk
    /// </summary>
    [Chunk(0x0907900E)]
    public partial class Chunk0907900E : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x0907900E;

        public short U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.EnumInt16<CPlugSurface.MaterialId>(ref n.surfaceId);
            rw.Int16(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x00F chunk
    /// </summary>
    [Chunk(0x0907900F)]
    public partial class Chunk0907900F : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x0907900F;

        public int U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x010 chunk
    /// </summary>
    [Chunk(0x09079010)]
    public partial class Chunk09079010 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079010;

        public float U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x011 chunk
    /// </summary>
    [Chunk(0x09079011)]
    public partial class Chunk09079011 : Chunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079011;

        public string[]? U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.ArrayId(ref U01!);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x012 skippable chunk
    /// </summary>
    [Chunk(0x09079012)]
    public partial class Chunk09079012 : SkippableChunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079012;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CPlugMaterial 0x013 skippable chunk
    /// </summary>
    [Chunk(0x09079013)]
    public partial class Chunk09079013 : SkippableChunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079013;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CPlugMaterial 0x014 skippable chunk
    /// </summary>
    [Chunk(0x09079014)]
    public partial class Chunk09079014 : SkippableChunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079014;

        /// <inheritdoc />
        public override bool Ignore => true;

    }

    /// <summary>
    /// CPlugMaterial 0x015 chunk
    /// </summary>
    [Chunk(0x09079015)]
    public partial class Chunk09079015 : Chunk<CPlugMaterial>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09079015;

        public int Version { get; set; }

        public int[]? U01;
        public int U02;
        public External<CPlugMaterialColorTargetTable>[]? U03;
        public Components.GbxRefTableFile? U03File;
        public CMwNod? U04;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.NodeRef<CPlug>(ref n.shader, ref n.shaderFile);
            if (n.Shader==null&&n.ShaderFile==null)
            {
                rw.ArrayReadableWritable<DeviceMat>(ref n.deviceMaterials!, version: 0x015);
                rw.Array<int>(ref U01!);
                if (Version >= 3)
                {
                    rw.Int32(ref U02);
                }
            }
            if (n.Shader!=null||n.ShaderFile!=null)
            {
                rw.ArrayNodeRef<CPlugMaterialColorTargetTable>(ref U03!);
                if (Version >= 7)
                {
                    rw.NodeRef<CMwNod>(ref U04);
                }
            }
        }
    }

    /// <summary>
    /// CPlugMaterial 0x016 chunk
    /// </summary>
    [Chunk(0x09079016)]
    public partial class Chunk09079016 : Chunk<CPlugMaterial>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09079016;

        public int Version { get; set; }

        public uint U01;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x017 chunk
    /// </summary>
    [Chunk(0x09079017)]
    public partial class Chunk09079017 : Chunk<CPlugMaterial>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x09079017;

        public int Version { get; set; }

        public int U01;
        public int U02;
        public short U03;
        public string? U04;

        public override void ReadWrite(CPlugMaterial n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version >= 1)
            {
                rw.EnumByte<CPlugSurface.MaterialId>(ref n.surfaceId);
                rw.EnumByte<CPlugSurface.GameplayId>(ref n.gameplayId);
                rw.Int32(ref U01);
                rw.Int32(ref U02);
                rw.Int16(ref U03);
                rw.String(ref U04);
            }
        }
    }

    /// <summary>
    /// CPlugMaterial 0x019 skippable chunk
    /// </summary>
    [Chunk(0x09079019)]
    public partial class Chunk09079019 : SkippableChunk<CPlugMaterial>
    {
        /// <inheritdoc />
        public override uint Id => 0x09079019;

        /// <inheritdoc />
        public override bool Ignore => true;

    }


    public sealed partial class DeviceMat : IReadableWritable
    {

        private short u01;
        public short U01 { get => u01; set => u01 = value; }

        private short u02;
        public short U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private CPlugShader? shader1;
        public CPlugShader? Shader1 { get => shader1File?.GetNode(ref shader1) ?? shader1; set => shader1 = value; }
        private Components.GbxRefTableFile? shader1File;
        public Components.GbxRefTableFile? Shader1File { get => shader1File; set => shader1File = value; }
        public CPlugShader? GetShader1(GbxReadSettings settings = default, bool exceptions = false) => shader1File?.GetNode(ref shader1, settings, exceptions) ?? shader1;

        private CPlugShader? shader2;
        public CPlugShader? Shader2 { get => shader2File?.GetNode(ref shader2) ?? shader2; set => shader2 = value; }
        private Components.GbxRefTableFile? shader2File;
        public Components.GbxRefTableFile? Shader2File { get => shader2File; set => shader2File = value; }
        public CPlugShader? GetShader2(GbxReadSettings settings = default, bool exceptions = false) => shader2File?.GetNode(ref shader2, settings, exceptions) ?? shader2;

        private CPlugShader? shader3;
        public CPlugShader? Shader3 { get => shader3File?.GetNode(ref shader3) ?? shader3; set => shader3 = value; }
        private Components.GbxRefTableFile? shader3File;
        public Components.GbxRefTableFile? Shader3File { get => shader3File; set => shader3File = value; }
        public CPlugShader? GetShader3(GbxReadSettings settings = default, bool exceptions = false) => shader3File?.GetNode(ref shader3, settings, exceptions) ?? shader3;

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int16(ref u01);
            rw.Int16(ref u02);
            if (v >= 4)
            {
                rw.Boolean(ref u03);
            }
            rw.NodeRef<CPlugShader>(ref shader1, ref shader1File);
            if (v >= 9)
            {
                rw.NodeRef<CPlugShader>(ref shader2, ref shader2File);
                rw.NodeRef<CPlugShader>(ref shader3, ref shader3File);
            }
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09079001 => new Chunk09079001(),
        0x09079002 => new Chunk09079002(),
        0x09079004 => new Chunk09079004(),
        0x09079007 => new Chunk09079007(),
        0x09079009 => new Chunk09079009(),
        0x0907900A => new Chunk0907900A(),
        0x0907900D => new Chunk0907900D(),
        0x0907900E => new Chunk0907900E(),
        0x0907900F => new Chunk0907900F(),
        0x09079010 => new Chunk09079010(),
        0x09079011 => new Chunk09079011(),
        0x09079012 => new Chunk09079012(),
        0x09079013 => new Chunk09079013(),
        0x09079014 => new Chunk09079014(),
        0x09079015 => new Chunk09079015(),
        0x09079016 => new Chunk09079016(),
        0x09079017 => new Chunk09079017(),
        0x09079019 => new Chunk09079019(),
        _ => base.NewChunk(chunkId),
    };
}
