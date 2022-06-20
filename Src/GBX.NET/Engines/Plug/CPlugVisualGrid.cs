namespace GBX.NET.Engines.Plug;

[Node(0x0903B000)]
public class CPlugVisualGrid : CPlugVisual3D
{
    private int nbPointX;
    private int nbPointZ;
    private float rangeX;
    private float rangeZ;

    protected CPlugVisualGrid()
    {
        
    }

    [NodeMember]
    public int NbPointX { get => nbPointX; set => nbPointX = value; }

    [NodeMember]
    public int NbPointZ { get => nbPointZ; set => nbPointZ = value; }

    [NodeMember]
    public float RangeX { get => rangeX; set => rangeX = value; }

    [NodeMember]
    public float RangeZ { get => rangeZ; set => rangeZ = value; }

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
