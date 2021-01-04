using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker Bloom block for TMUF and older games (0x03083000). This node causes "Couldn't load map" in ManiaPlanet.
    /// </summary>
    [Node(0x03083000)]
    public class CGameCtnMediaBlockFxBloom : CGameCtnMediaBlockFx
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMediaBlockFxBloom 0x001 chunk
        /// </summary>
        [Chunk(0x03083001)]
        public class Chunk03083001 : Chunk<CGameCtnMediaBlockFxBloom>
        {
            public override void Read(CGameCtnMediaBlockFxBloom n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(i =>
                {
                    return new Key()
                    {
                        Time = r.ReadSingle(),
                        Intensity = r.ReadSingle(),
                        Sensitivity = r.ReadSingle()
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockFxBloom n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                    w.Write(x.Sensitivity);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
            public float Sensitivity { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockFxBloom node;

            public Key[] Keys => node.Keys;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockFxBloom node) => this.node = node;
        }

        #endregion
    }
}
