namespace GBX.NET.Engines.Plug;

/// <summary>
/// CPlugMaterial (0x09079000)
/// </summary>
[Node(0x09079000), WritingNotSupported]
[NodeExtension("Material")]
public class CPlugMaterial : CPlug
{
    private CPlugMaterialCustom? customMaterial;
    private CPlug? shader;
    private int shaderIndex;
    private SDeviceMat[]? deviceMaterials;

    public CPlugMaterialCustom? CustomMaterial
    {
        get => customMaterial; 
        set => customMaterial = value;
    }

    public CPlug? Shader
    {
        get => shader = GetNodeFromRefTable(shader, shaderIndex) as CPlug;
        set => shader = value;
    }

    public SDeviceMat[]? DeviceMaterials
    {
        get => deviceMaterials;
        set => deviceMaterials = value;
    }

    protected CPlugMaterial()
    {

    }

    [Chunk(0x09079001)]
    public class Chunk09079001 : Chunk<CPlugMaterial>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    [Chunk(0x09079002)]
    public class Chunk09079002 : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // DoData
        }
    }

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

                return new SDeviceMat(n.GBX, shader1)
                {
                    U01 = u01,
                    U02 = u02,
                    U03 = u03
                };
            });
        }
    }

    [Chunk(0x09079007)]
    public class Chunk09079007 : Chunk<CPlugMaterial>
    {
        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef<CPlugMaterialCustom>(ref n.customMaterial);
        }
    }

    [Chunk(0x09079009)]
    public class Chunk09079009 : Chunk<CPlugMaterial>
    {
        public CPlug? U01;
        public object[]? U02;
        public CMwNod? U03;
        public CMwNod? U04;

        public override void Read(CPlugMaterial n, GameBoxReader r)
        {
            _ = r.ReadNodeRef<CPlug>(out n.shaderIndex);

            if (n.shaderIndex >= 0)
                return;

            n.deviceMaterials = r.ReadArray(r =>
            {
                var u01 = r.ReadInt16();
                var u02 = r.ReadInt16();
                var u03 = r.ReadInt32();

                _ = r.ReadNodeRef<CPlugShader>(out int shader1Index);
                _ = r.ReadNodeRef<CPlugShader>(out int shader2Index);
                _ = r.ReadNodeRef<CPlugShader>(out int shader3Index);

                return new SDeviceMat(n.GBX, shader1Index, shader2Index, shader3Index)
                {
                    U01 = u01,
                    U02 = u02,
                    U03 = u03
                };
            });
        }
    }

    [Chunk(0x0907900A)]
    public class Chunk0907900A : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    public class SDeviceMat
    {
        private CPlugShader? shader1;
        private int? shader1Index;
        private CPlugShader? shader2;
        private int? shader2Index;
        private CPlugShader? shader3;
        private int? shader3Index;

        public GameBox? Gbx { get; }

        public short U01 { get; set; }
        public short U02 { get; set; }
        public int U03 { get; set; }

        public CPlugShader? Shader1
        {
            get => shader1 = Gbx?.RefTable?.GetNode(shader1, shader1Index, Gbx.FileName) as CPlugShader;
            set => shader1 = value;
        }

        public CPlugShader? Shader2
        {
            get => shader2 = Gbx?.RefTable?.GetNode(shader2, shader2Index, Gbx.FileName) as CPlugShader;
            set => shader2 = value;
        }

        public CPlugShader? Shader3
        {
            get => shader3 = Gbx?.RefTable?.GetNode(shader3, shader3Index, Gbx.FileName) as CPlugShader;
            set => shader3 = value;
        }

        public SDeviceMat(GameBox? gbx, CPlugShader? shader1)
        {
            Gbx = gbx;

            this.shader1 = shader1;
        }

        public SDeviceMat(GameBox? gbx, int shader1Index, int shader2Index, int shader3Index)
        {
            Gbx = gbx;

            this.shader1Index = shader1Index;
            this.shader2Index = shader2Index;
            this.shader3Index = shader3Index;
        }
    }
}
