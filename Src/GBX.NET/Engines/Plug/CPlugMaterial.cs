namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09079000</remarks>
[Node(0x09079000)]
[NodeExtension("Material")]
public class CPlugMaterial : CPlug
{
    private CPlugMaterialCustom? customMaterial;
    private CPlug? shader;
    private GameBoxRefTable.File? shaderFile;
    private DeviceMat[]? deviceMaterials;

    [NodeMember]
    [AppliedWithChunk<Chunk09079007>]
    public CPlugMaterialCustom? CustomMaterial { get => customMaterial; set => customMaterial = value; }

    [NodeMember]
    public CPlug? Shader
    {
        get => shader = GetNodeFromRefTable(shader, shaderFile) as CPlug;
        set => shader = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk09079004>]
    [AppliedWithChunk<Chunk09079009>]
    [AppliedWithChunk<Chunk0907900D>]
    public DeviceMat[]? DeviceMaterials { get => deviceMaterials; set => deviceMaterials = value; }

    internal CPlugMaterial()
    {

    }

    /// <summary>
    /// CPlugMaterial 0x001 chunk
    /// </summary>
    [Chunk(0x09079001)]
    public class Chunk09079001 : Chunk<CPlugMaterial>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x002 chunk
    /// </summary>
    [Chunk(0x09079002)]
    public class Chunk09079002 : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // DoData
        }
    }

    /// <summary>
    /// CPlugMaterial 0x004 chunk
    /// </summary>
    [Chunk(0x09079004)]
    public class Chunk09079004 : Chunk<CPlugMaterial>
    {
        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.ArrayArchiveWithGbx(ref n.deviceMaterials, version: 4);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x007 chunk
    /// </summary>
    [Chunk(0x09079007)]
    public class Chunk09079007 : Chunk<CPlugMaterial>
    {
        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterialCustom>(ref n.customMaterial);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x009 chunk
    /// </summary>
    [Chunk(0x09079009)]
    public class Chunk09079009 : Chunk<CPlugMaterial>
    {
        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.shader, ref n.shaderFile);

            if (n.shaderFile is not null)
            {
                return;
            }

            rw.ArrayArchiveWithGbx(ref n.deviceMaterials, version: 9);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x00A chunk
    /// </summary>
    [Chunk(0x0907900A)]
    public class Chunk0907900A : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x00D chunk
    /// </summary>
    [Chunk(0x0907900D)]
    public class Chunk0907900D : Chunk<CPlugMaterial>
    {
        public int[]? U02;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref n.shader, ref n.shaderFile);

            if (n.shaderFile is not null)
            {
                return;
            }

            rw.ArrayArchiveWithGbx(ref n.deviceMaterials, version: 0xD);

            rw.Array<int>(ref U02);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x00E chunk
    /// </summary>
    [Chunk(0x0907900E)]
    public class Chunk0907900E : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x00F chunk
    /// </summary>
    [Chunk(0x0907900F)]
    public class Chunk0907900F : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x010 chunk
    /// </summary>
    [Chunk(0x09079010)]
    public class Chunk09079010 : Chunk<CPlugMaterial>
    {
        public float U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugMaterial 0x011 chunk
    /// </summary>
    [Chunk(0x09079011)]
    public class Chunk09079011 : Chunk<CPlugMaterial>
    {
        public string[] U01 = Array.Empty<string>();

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.ArrayId(ref U01!);
        }
    }

    #region 0x012 skippable chunk

    /// <summary>
    /// CPlugMaterial 0x012 skippable chunk
    /// </summary>
    [Chunk(0x09079012), IgnoreChunk]
    public class Chunk09079012 : SkippableChunk<CPlugMaterial>
    {

    }

    #endregion

    #region 0x013 skippable chunk

    /// <summary>
    /// CPlugMaterial 0x013 skippable chunk
    /// </summary>
    [Chunk(0x09079013), IgnoreChunk]
    public class Chunk09079013 : SkippableChunk<CPlugMaterial>
    {

    }

    #endregion

    #region 0x014 skippable chunk

    /// <summary>
    /// CPlugMaterial 0x014 skippable chunk
    /// </summary>
    [Chunk(0x09079014), IgnoreChunk]
    public class Chunk09079014 : SkippableChunk<CPlugMaterial>
    {

    }

    #endregion

    public class DeviceMat : IReadableWritableWithGbx
    {
        private Node? node;

        private short? u01;
        private short? u02;
        private bool? u03;
        private short? u04;
        private byte? u05;
        private byte? u06;
        private int? u07;

        private CPlugShader? shader1;
        private GameBoxRefTable.File? shader1File;
        private CPlugShader? shader2;
        private GameBoxRefTable.File? shader2File;
        private CPlugShader? shader3;
        private GameBoxRefTable.File? shader3File;

        public short? U01 { get => u01; set => u01 = value; }
        public short? U02 { get => u02; set => u02 = value; }
        public bool? U03 { get => u03; set => u03 = value; }
        public short? U04 { get => u04; set => u04 = value; }
        public byte? U05 { get => u05; set => u05 = value; }
        public byte? U06 { get => u06; set => u06 = value; }
        public int? U07 { get => u07; set => u07 = value; }

        public CPlugShader? Shader1
        {
            get => shader1 = node?.GetNodeFromRefTable(shader1, shader1File) as CPlugShader;
            set => shader1 = value;
        }
        public GameBoxRefTable.File? Shader1File { get => shader1File; set => shader1File = value; }

        public CPlugShader? Shader2
        {
            get => shader2 = node?.GetNodeFromRefTable(shader2, shader2File) as CPlugShader;
            set => shader2 = value;
        }
        public GameBoxRefTable.File? Shader2File { get => shader2File; set => shader2File = value; }

        public CPlugShader? Shader3
        {
            get => shader3 = node?.GetNodeFromRefTable(shader3, shader3File) as CPlugShader;
            set => shader3 = value;
        }
        public GameBoxRefTable.File? Shader3File { get => shader3File; set => shader3File = value; }

        public void ReadWrite(GameBoxReaderWriter rw, GameBox? gbx, int version = 0)
        {
            node = gbx?.Node;
            ReadWrite(rw, version);
        }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            switch (version)
            {
                case 0:
                    rw.Int16(ref u01);
                    rw.Int16(ref u02);
                    rw.NodeRef(ref shader1, ref shader1File);
                    break;
                case 4:
                case 8:
                    rw.Int16(ref u01);
                    rw.Int16(ref u02);
                    rw.Boolean(ref u03);
                    rw.NodeRef(ref shader1, ref shader1File);
                    break;
                case 9:
                case 0xC:
                    rw.Int16(ref u01);
                    rw.Int16(ref u02);

                    rw.Boolean(ref u03);
                    rw.NodeRef(ref shader1, ref shader1File); // u03 == true: external node, false: internal node

                    rw.NodeRef(ref shader2, ref shader2File);
                    rw.NodeRef(ref shader3, ref shader3File);
                    break;
                case 0xD:
                    rw.Int16(ref u04);
                    rw.Byte(ref u05);
                    rw.Byte(ref u06);
                    
                    rw.Boolean(ref u03);
                    rw.NodeRef(ref shader1, ref shader1File); // u03 == true: external node, false: internal node

                    rw.NodeRef(ref shader2, ref shader2File);
                    rw.NodeRef(ref shader3, ref shader3File);
                    break;
            }
        }
    }
}
