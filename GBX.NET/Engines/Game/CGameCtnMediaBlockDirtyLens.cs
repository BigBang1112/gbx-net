using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03165000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockDirtyLens : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03165000)]
        public class Chunk03165000 : Chunk<CGameCtnMediaBlockDirtyLens>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockDirtyLens n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var intensity = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        Intensity = intensity
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockDirtyLens n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockDirtyLens node;

            public Key[] Keys => node.Keys;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockDirtyLens node) => this.node = node;
        }

        #endregion
    }
}