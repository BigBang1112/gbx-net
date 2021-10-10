using GBX.NET.Engines.Hms;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Scene
{
    [Node(0x0A011000)]
    public class CSceneMobil : CSceneObject
    {
        private CHmsItem item;

        public CHmsItem Item
        {
            get => item;
            set => item = value;
        }

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
            public override void Read(CSceneMobil n, GameBoxReader r)
            {
                n.item = Parse<CHmsItem>(r, 0x06003000);
                var gds = r.ReadBytes(14);
                var gdshsd = r.ReadInt32();
                var str = r.ReadString();
                var gdshsd1 = r.ReadInt32();
                var gdshsd2 = r.ReadInt32();
                var gdshsd3 = r.ReadInt32();
                var gdshsd4 = r.ReadInt32();
                var str2 = r.ReadString();
                var gdshsd5 = r.ReadInt32();
                var gdshsd6 = r.ReadInt32();
                var gdshsd7 = r.ReadInt32();
                var gdshsd8 = r.ReadInt32();
                var str3 = r.ReadString();
                var dgsg = r.ReadArray<int>(17);
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
