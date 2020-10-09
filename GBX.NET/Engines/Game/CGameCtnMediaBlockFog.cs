using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03199000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockFog : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03199000)]
        public class Chunk03199000 : Chunk<CGameCtnMediaBlockFog>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockFog n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(9);

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
            private readonly CGameCtnMediaBlockFog node;

            public Key[] Keys => node.Keys;

            public DebugView(CGameCtnMediaBlockFog node) => this.node = node;
        }

        #endregion
    }
}
