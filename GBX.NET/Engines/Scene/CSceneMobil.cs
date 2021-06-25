using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Scene
{
    [Node(0x0A011000)]
    public class CSceneMobil : CSceneObject
    {
        [Chunk(0x0A011003)]
        public class Chunk0A011003 : Chunk<CSceneMobil>
        {
            public int U01;

            public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
            }
        }

        [Chunk(0x0A011005)]
        public class Chunk0A011005 : Chunk<CSceneMobil>
        {

            public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(); // CHmsItem
                rw.Reader.ReadNodeRef<CPlugSolid>();

                var hmsItem = rw.Reader.ReadInt32(); // CHmsItem 0x11

                rw.Byte();
                rw.Byte();
                rw.Int32();
                rw.Int32();

                var facade = rw.Reader.ReadInt32();
            }
        }

        [Chunk(0x0A011006)]
        public class Chunk0A011006 : Chunk<CSceneMobil>
        {
            public int U01;

            public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
            }
        }
    }
}
