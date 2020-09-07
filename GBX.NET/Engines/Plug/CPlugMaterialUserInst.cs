using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Plug
{
    [Node(0x090FD000)]
    public class CPlugMaterialUserInst : Node
    {
        public string MaterialFile { get; set; }

        public CPlugMaterialUserInst(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x090FD000)]
        public class Chunk090FD000 : Chunk<CPlugMaterialUserInst>
        {
            /// <summary>
            /// Version 10: TM®, version 8/9: ManiaPlanet
            /// </summary>
            public int Version { get; set; }
            public Node Unknown1 { get; set; }
            public Node Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public byte Unknown4 { get; set; }
            public Node Unknown5 { get; set; }

            public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Unknown1 = rw.NodeRef(Unknown1);
                Unknown2 = rw.NodeRef(Unknown2);
                Unknown3 = rw.Int32(Unknown3);
                Unknown4 = rw.Byte(Unknown4);
                if (Version < 9)
                    n.MaterialFile = rw.LookbackString(n.MaterialFile);
                if (Version >= 9)
                {
                    if(Version >= 10)
                        rw.Byte(Unknown);
                    n.MaterialFile = rw.String(n.MaterialFile);
                }
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                Unknown5 = rw.NodeRef(Unknown5);
            }
        }

        [Chunk(0x090FD001)]
        public class Chunk090FD001 : Chunk<CPlugMaterialUserInst>
        {
            public int Version { get; set; }
            public Node Unknown1 { get; set; }

            public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Unknown1 = rw.NodeRef(Unknown1);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Single(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x090FD002)]
        public class Chunk090FD002 : Chunk<CPlugMaterialUserInst>
        {
            public override void ReadWrite(CPlugMaterialUserInst n, GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }
    }
}
