using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game
{
    [Node(0x0329F000)]
    public class CGameCtnMediaBlockEntity : CGameCtnMediaBlock
    {
        public CPlugEntRecordData RecordData { get; set; }

        [Chunk(0x0329F000)]
        [IgnoreChunk]
        public class Chunk0329F000 : Chunk<CGameCtnMediaBlockEntity>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockEntity n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.RecordData = rw.NodeRef<CPlugEntRecordData>(n.RecordData);
            }
        }

        public class Key : MediaBlockKey
        {
            public float[] Unknown { get; set; }
        }
    }
}
