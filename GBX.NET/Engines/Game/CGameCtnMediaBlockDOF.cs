using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03126000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockDOF : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x03126002)]
        public class Chunk03126002 : Chunk<CGameCtnMediaBlockDOF>
        {
            public override void Read(CGameCtnMediaBlockDOF n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var zFocus = r.ReadSingle();
                    var lensSize = r.ReadSingle();
                    var a = r.ReadInt32();
                    var b = r.ReadInt32();
                    var c = r.ReadInt32();
                    var d = r.ReadInt32();

                    return new Key()
                    {
                        Time = time,
                        ZFocus = zFocus,
                        LensSize = lensSize,
                        Unknown = new object[] { a, b, c, d }
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockDOF n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.ZFocus);
                    w.Write(x.LensSize);
                    w.Write((int)x.Unknown[0]);
                    w.Write((int)x.Unknown[1]);
                    w.Write((int)x.Unknown[2]);
                    w.Write((int)x.Unknown[3]);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float ZFocus { get; set; }
            public float LensSize { get; set; }

            public object[] Unknown { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockDOF node;

            public Key[] Keys => node.Keys;

            public DebugView(CGameCtnMediaBlockDOF node) => this.node = node;
        }

        #endregion
    }
}
