using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Scene
{
    [Node(0x0A005000)]
    public class CSceneObject : CMwNod
    {
        [Chunk(0x0A005001)]
        public class Chunk0A005001 : Chunk<CSceneObject>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSceneObject n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
            }
        }

        [Chunk(0x0A005003)]
        public class Chunk0A005003 : Chunk<CSceneObject>
        {
            private int u01;

            public int U01
            {
                get => u01;
                set => u01 = value;
            }

            public override void ReadWrite(CSceneObject n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref u01);
            }
        }
    }
}