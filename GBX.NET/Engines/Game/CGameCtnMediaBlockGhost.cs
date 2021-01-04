using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x030E5000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockGhost : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public Key[] Keys { get; set; }

        [NodeMember]
        public CGameCtnGhost GhostModel { get; set; }

        [NodeMember]
        public float StartOffset { get; set; }

        [NodeMember]
        public bool NoDamage { get; set; }

        [NodeMember]
        public bool ForceLight { get; set; }

        [NodeMember]
        public bool ForceHue { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030E5001)]
        public class Chunk030E5001 : Chunk<CGameCtnMediaBlockGhost>
        {
            public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                n.GhostModel = rw.NodeRef<CGameCtnGhost>(n.GhostModel);
                n.StartOffset = rw.Single(n.StartOffset);
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x030E5002)]
        public class Chunk030E5002 : Chunk<CGameCtnMediaBlockGhost>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Unknown = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });

                n.GhostModel = rw.NodeRef<CGameCtnGhost>(n.GhostModel);
                n.StartOffset = rw.Single(n.StartOffset);
                n.NoDamage = rw.Boolean(n.NoDamage);
                n.ForceLight = rw.Boolean(n.ForceLight);
                n.ForceHue = rw.Boolean(n.ForceHue);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Unknown { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockGhost node;

            public float Start => node.Start;
            public float End => node.End;
            public Key[] Keys => node.Keys;
            public CGameGhost GhostModel => node.GhostModel;
            public float StartOffset => node.StartOffset;
            public bool NoDamage => node.NoDamage;
            public bool ForceLight => node.ForceLight;
            public bool ForceHue => node.ForceHue;

            public ChunkSet Chunks => node.Chunks;

            public DebugView(CGameCtnMediaBlockGhost node) => this.node = node;
        }

        #endregion
    }
}
