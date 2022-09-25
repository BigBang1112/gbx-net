namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09009000</remarks>
[Node(0x09009000)]
public class CPlugVisualIndexedLines : CPlugVisualIndexed
{
    internal CPlugVisualIndexedLines()
    {

    }

    /// <summary>
    /// CPlugVisualIndexedLines 0x001 chunk
    /// </summary>
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
