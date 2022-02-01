namespace GBX.NET.Engines.Graphic;

[Node(0x04003000)]
public class GxLightPoint : GxLightNotAmbient
{
    private float flareSize;

    public float FlareSize
    {
        get => flareSize;
        set => flareSize = value;
    }

    protected GxLightPoint()
    {

    }

    [Chunk(0x04003003)]
    public class Chunk04003003 : Chunk<GxLightPoint>
    {
        public override void ReadWrite(GxLightPoint n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.flareSize);
        }
    }
}