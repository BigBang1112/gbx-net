using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03127000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x004 chunk

        [Chunk(0x03127004)]
        public class Chunk03127004 : Chunk<CGameCtnMediaBlockToneMapping>
        {
            public override void ReadWrite(CGameCtnMediaBlockToneMapping n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        Exposure = rw.Reader.ReadSingle(),
                        MaxHDR = rw.Reader.ReadSingle(),
                        LightTrailScale = rw.Reader.ReadSingle(),
                        Unknown = rw.Reader.ReadInt32()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Exposure);
                    rw.Writer.Write(x.MaxHDR);
                    rw.Writer.Write(x.LightTrailScale);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Exposure { get; set; }
            public float MaxHDR { get; set; }
            public float LightTrailScale { get; set; }
            public int Unknown { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockToneMapping node;

            public Key[] Keys => node.Keys;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockToneMapping node) => this.node = node;
        }

        #endregion
    }
}
