namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x04003000</remarks>
[Node(0x04003000)]
public class GxLightPoint : GxLightNotAmbient
{
    private float flareSize;
    private float flareBiasZ;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04003003>]
    [AppliedWithChunk<Chunk04003004>]
    public float FlareSize { get => flareSize; set => flareSize = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk04003004>]
    public float FlareBiasZ { get => flareBiasZ; set => flareBiasZ = value; }

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

    #region 0x004 chunk

    /// <summary>
    /// GxLightPoint 0x004 chunk
    /// </summary>
    [Chunk(0x04003004)]
    public class Chunk04003004 : Chunk<GxLightPoint>
    {
        public override void ReadWrite(GxLightPoint n, GameBoxReaderWriter rw)
        {
            rw.Single(ref n.flareSize);
            rw.Single(ref n.flareBiasZ);
        }
    }

    #endregion
}