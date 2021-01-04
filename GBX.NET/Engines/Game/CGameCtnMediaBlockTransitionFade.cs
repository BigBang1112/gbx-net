using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030AB000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockTransitionFade : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        [NodeMember]
        public Vec3 Color { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x030AB000)]
        public class Chunk030AB000 : Chunk<CGameCtnMediaBlockTransitionFade>
        {
            public override void ReadWrite(CGameCtnMediaBlockTransitionFade n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var opacity = rw.Reader.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Opacity = opacity
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Opacity);
                });

                n.Color = rw.Vec3(n.Color);
                rw.Single(Unknown);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Opacity { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockTransitionFade node;

            public Key[] Keys => node.Keys;
            public Vec3 Color => node.Color;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockTransitionFade node) => this.node = node;
        }

        #endregion
    }
}
