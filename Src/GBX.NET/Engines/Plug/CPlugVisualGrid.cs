namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0903B000</remarks>
[Node(0x0903B000)]
public class CPlugVisualGrid : CPlugVisual3D
{
    private int nbPointX;
    private int nbPointZ;
    private float rangeX;
    private float rangeZ;

    internal CPlugVisualGrid()
    {
        
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0903B000))]
    public int NbPointX { get => nbPointX; set => nbPointX = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0903B000))]
    public int NbPointZ { get => nbPointZ; set => nbPointZ = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0903B000))]
    public float RangeX { get => rangeX; set => rangeX = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0903B000))]
    public float RangeZ { get => rangeZ; set => rangeZ = value; }

    /// <summary>
    /// CPlugVisualGrid 0x000 chunk
    /// </summary>
    [Chunk(0x0903B000)]
    public class Chunk0903B000 : Chunk<CPlugVisualGrid>
    {
        public override void ReadWrite(CPlugVisualGrid n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.nbPointX);
            rw.Int32(ref n.nbPointZ);
            rw.Single(ref n.rangeX);
            rw.Single(ref n.rangeZ);
        }
    }
}
