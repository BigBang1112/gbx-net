namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09128000</remarks>
[Node(0x09128000)]
public class CPlugRoadChunk : CMwNod
{
    protected CPlugRoadChunk()
    {

    }

    /// <summary>
    /// CPlugRoadChunk 0x000 chunk
    /// </summary>
    [Chunk(0x09128000)]
    public class Chunk09128000 : Chunk<CPlugRoadChunk>, IVersionable
    {
        public int Version { get; set; }
        public byte[]? UnknownData { get; set; }

        public override void ReadWrite(CPlugRoadChunk n, GameBoxReaderWriter rw)
        {
            Version = rw.Int32(Version);
            UnknownData = rw.Reader!.ReadUntilFacade().ToArray();
        }
    }
}
