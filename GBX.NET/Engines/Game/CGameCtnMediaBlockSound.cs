using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A7000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockSound : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public FileRef Sound { get; set; }

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030A7001)]
        public class Chunk030A7001 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                n.Sound = rw.FileRef(n.Sound);

                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(2);

                    return new Key()
                    {
                        Time = time,
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x030A7002)]
        public class Chunk030A7002 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                rw.Array<int>(Unknown, 2);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x030A7003)]
        public class Chunk030A7003 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                rw.Array<int>(Unknown, 7);
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x030A7004)]
        public class Chunk030A7004 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                n.Sound = rw.FileRef(n.Sound);
                rw.Int32(Unknown); // 1
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(5);

                    return new Key()
                    {
                        Time = time,
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float[] Unknown { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockSound node;

            public FileRef Sound => node.Sound;
            public Key[] Keys => node.Keys;

            public DebugView(CGameCtnMediaBlockSound node) => this.node = node;
        }

        #endregion
    }
}
