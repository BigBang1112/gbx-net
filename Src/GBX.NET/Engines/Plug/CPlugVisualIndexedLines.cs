namespace GBX.NET.Engines.Plug;

/// <summary>
/// Indexed visual as lines (0x09009000)
/// </summary>
/// <remarks>Handles indicies of a 3D mesh by line connections.</remarks>
[Node(0x09009000)]
public class CPlugVisualIndexedLines : CPlugVisualIndexed
{
    protected CPlugVisualIndexedLines()
    {

    }

    [Chunk(0x09009001)]
    public class Chunk09009001 : Chunk<CPlugVisualIndexedLines>
    {
        public int U01;

        public override void ReadWrite(CPlugVisualIndexedLines n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }
}
