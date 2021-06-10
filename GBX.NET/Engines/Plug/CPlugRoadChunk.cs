using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Plug
{
    [Node(0x09128000)]
    public class CPlugRoadChunk : CMwNod
    {
        [Chunk(0x09128000)]
        public class Chunk09128000 : Chunk<CPlugRoadChunk>
        {
            public int Version { get; set; }
            public byte[] UnknownData { get; set; }

            public override void ReadWrite(CPlugRoadChunk n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                UnknownData = rw.Reader.ReadUntilFacade();
            }
        }
    }
}
