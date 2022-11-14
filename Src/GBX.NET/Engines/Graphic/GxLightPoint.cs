namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04003000</remarks>
[Node(0x04003000)]
public class GxLightPoint : GxLightNotAmbient
{
    private float flareSize;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04003003>]
    public float FlareSize { get => flareSize; set => flareSize = value; }

    internal GxLightPoint()
    {

    }

    /// <summary>
    /// GxLightPoint 0x003 chunk
    /// </summary>
    [Chunk(0x04003003)]
    public class Chunk04003003 : Chunk<GxLightPoint>
    {
        public override void ReadWrite(GxLightPoint n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.flareSize);
        }
    }
}