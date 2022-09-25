namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09079000</remarks>
[Node(0x09079000), WritingNotSupported]
[NodeExtension("Material")]
public class CPlugMaterial : CPlug
{
    private CPlugMaterialCustom? customMaterial;
    private CPlug? shader;
    private GameBoxRefTable.File? shaderFile;
    private SDeviceMat[]? deviceMaterials;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk09079007))]
    public CPlugMaterialCustom? CustomMaterial { get => customMaterial; set => customMaterial = value; }

    [NodeMember]
    public CPlug? Shader
    {
        get => shader = GetNodeFromRefTable(shader, shaderFile) as CPlug;
        set => shader = value;
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk09079004))]
    [AppliedWithChunk(typeof(Chunk09079009))]
    [AppliedWithChunk(typeof(Chunk0907900D))]
    public SDeviceMat[]? DeviceMaterials { get => deviceMaterials; set => deviceMaterials = value; }

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
        public override void Read(CPlugMaterial n, GameBoxReader r)
        {
            n.deviceMaterials = r.ReadArray(r =>
            {
                var u01 = r.ReadInt16();
                var u02 = r.ReadInt16();
                var u03 = r.ReadInt32();

                var shader1 = r.ReadNodeRef<CPlugShader>();

                return new SDeviceMat(n, shader1)
                {
                    U01 = u01,
                    U02 = u02,
                    U03 = u03
                };
            });
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
        public CPlug? U01;
        public object[]? U02;
        public CMwNod? U03;
        public CMwNod? U04;

        public override void Read(CPlugMaterial n, GameBoxReader r)
        {
            _ = r.ReadNodeRef<CPlug>(out n.shaderFile);

            if (n.shaderFile is not null)
            {
                return;
            }

            n.deviceMaterials = r.ReadArray(r =>
            {
                var u01 = r.ReadInt16();
                var u02 = r.ReadInt16();
                var u03 = r.ReadInt32();

                _ = r.ReadNodeRef<CPlugShader>(out GameBoxRefTable.File? shader1File);
                _ = r.ReadNodeRef<CPlugShader>(out GameBoxRefTable.File? shader2File);
                _ = r.ReadNodeRef<CPlugShader>(out GameBoxRefTable.File? shader3File);

                return new SDeviceMat(n, shader1File, shader2File, shader3File)
                {
                    U01 = u01,
                    U02 = u02,
                    U03 = u03
                };
            });
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
        public Node? U01;
        public GameBoxRefTable.File? U01File;
        public int[]? U02;

        public override void Read(CPlugMaterial n, GameBoxReader r)
        {
            U01 = r.ReadNodeRef(out U01File);

            if (U01 is not null || U01File is not null)
            {
                return;
            }
            
            n.deviceMaterials = r.ReadArray(r =>
            {
                // UPlugRenderDevice
                var u01 = r.ReadInt16();
                var u02 = r.ReadByte();
                var u03 = r.ReadByte();
                //

                var u04 = r.ReadBoolean();

                var shader = default(CPlugShader);

                if (!u04)
                {
                    shader = r.ReadNodeRef<CPlugShader>();
                }
                else
                {
                    var u08 = r.ReadInt32();
                }

                var u06 = r.ReadInt32();
                var u07 = r.ReadInt32();

                return new SDeviceMat(n, shader);
            });

            U02 = r.ReadArray<int>();
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

    public class SDeviceMat
    {
        private readonly Node node;

        private CPlugShader? shader1;
        private readonly GameBoxRefTable.File? shader1File;
        private CPlugShader? shader2;
        private readonly GameBoxRefTable.File? shader2File;
        private CPlugShader? shader3;
        private readonly GameBoxRefTable.File? shader3File;

        public short U01 { get; set; }
        public short U02 { get; set; }
        public int U03 { get; set; }

        public CPlugShader? Shader1
        {
            get => shader1 = node.GetNodeFromRefTable(shader1, shader1File) as CPlugShader;
            set => shader1 = value;
        }

        public CPlugShader? Shader2
        {
            get => shader2 = node.GetNodeFromRefTable(shader2, shader2File) as CPlugShader;
            set => shader2 = value;
        }

        public CPlugShader? Shader3
        {
            get => shader3 = node.GetNodeFromRefTable(shader3, shader3File) as CPlugShader;
            set => shader3 = value;
        }
        
        public SDeviceMat(Node node, CPlugShader? shader1)
        {
            this.node = node;
            this.shader1 = shader1;
        }

        public SDeviceMat(Node node, GameBoxRefTable.File? shader1File, GameBoxRefTable.File? shader2File, GameBoxRefTable.File? shader3File)
        {
            this.node = node;
            this.shader1File = shader1File;
            this.shader2File = shader2File;
            this.shader3File = shader3File;
        }
    }
}
