namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09128000</remarks>
[Node(0x09128000)]
public class CPlugRoadChunk : CMwNod
{
    internal CPlugRoadChunk()
    {

    }

    /// <summary>
    /// CPlugRoadChunk 0x000 chunk
    /// </summary>
    [Chunk(0x09128000)]
    public class Chunk09128000 : Chunk<CPlugRoadChunk>, IVersionable
    {
        private int version;

        public byte[]? U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugRoadChunk n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            U01 = rw.Reader!.ReadUntilFacade().ToArray();
        }
    }
}
