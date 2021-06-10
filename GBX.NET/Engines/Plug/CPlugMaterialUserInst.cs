using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Plug
{
    [Node(0x090FD000)]
    public class CPlugMaterialUserInst : CMwNod
    {
        public string MaterialFile { get; set; }

        public override string ToString()
        {
            return MaterialFile;
        }

        [Chunk(0x090FD000)]
        public class Chunk090FD000 : Chunk<CPlugMaterialUserInst>
        {
            /// <summary>
            /// Version 10: TM®, version 8/9: ManiaPlanet
            /// </summary>
            public int Version { get; set; }
            public CMwNod U01 { get; set; }
            public CMwNod U02 { get; set; }
            public int U03 { get; set; }
            public byte U04 { get; set; }
            public CMwNod U05 { get; set; }

            public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                U01 = rw.NodeRef(U01);
                U02 = rw.NodeRef(U02);
                U03 = rw.Int32(U03);
                U04 = rw.Byte(U04);
                if (Version < 9)
                    n.MaterialFile = rw.Id(n.MaterialFile);
                if (Version >= 9)
                {
                    if(Version >= 10)
                        rw.Byte();
                    n.MaterialFile = rw.String(n.MaterialFile);
                }
                rw.Int32();
                rw.Int32();
                rw.Int32();
                rw.Int32();
                rw.Int32();
                U05 = rw.NodeRef(U05);
            }
        }

        [Chunk(0x090FD001)]
        public class Chunk090FD001 : Chunk<CPlugMaterialUserInst>
        {
            public int Version { get; set; }
            public CMwNod Unknown1 { get; set; }

            public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Unknown1 = rw.NodeRef(Unknown1);
                rw.Int32();
                rw.Int32();
                rw.Single();
                rw.Int32();
                rw.Int32();
            }
        }

        [Chunk(0x090FD002)]
        public class Chunk090FD002 : Chunk<CPlugMaterialUserInst>
        {
            public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
            {
                rw.Int32();
                rw.Int32();
            }
        }
    }
}
