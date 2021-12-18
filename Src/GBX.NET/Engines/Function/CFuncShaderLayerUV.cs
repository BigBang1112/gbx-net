namespace GBX.NET.Engines.Function;

[Node(0x05015000)]
public class CFuncShaderLayerUV : CFuncShader
{
    private string? layerName;

    public string? LayerName
    {
        get => layerName;
        set => layerName = value;
    }

    protected CFuncShaderLayerUV()
    {

    }

    [Chunk(0x05015005)]
    public class Chunk05015005 : Chunk<CFuncShaderLayerUV>
    {
        public int U01;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.layerName);
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x05015009)]
    public class Chunk05015009 : Chunk<CFuncShaderLayerUV>
    {
        public Vec2 U01;
        public Vec2 U02;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref U01);
            rw.Vec2(ref U02);
        }
    }

    [Chunk(0x05015012)]
    public class Chunk05015012 : Chunk<CFuncShaderLayerUV>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    [Chunk(0x0501500D)]
    public class Chunk0501500D : Chunk<CFuncShaderLayerUV>
    {
        public Vec2 U01;
        public Vec2 U02;
        public Vec2 U03;

        public override void ReadWrite(CFuncShaderLayerUV n, GameBoxReaderWriter rw)
        {
            rw.Vec2(ref U01);
            rw.Vec2(ref U02);
            rw.Vec2(ref U03);
        }
    }
}
