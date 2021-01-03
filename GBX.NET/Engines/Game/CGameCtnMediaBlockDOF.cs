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

        #region 0x000 chunk

        [Chunk(0x03126000)]
        public class Chunk03126000 : Chunk<CGameCtnMediaBlockDOF>
        {
            public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, (i, r) =>
                {
                    var time = r.ReadSingle();
                    var zFocus = r.ReadSingle();
                    var lensSize = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        ZFocus = zFocus,
                        LensSize = lensSize
                    };
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.ZFocus);
                    w.Write(x.LensSize);
                });
            }
        }

        #endregion

        #region 0x001 chunk

        [Chunk(0x03126001)]
        public class Chunk03126001 : Chunk<CGameCtnMediaBlockDOF>
        {
            public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, (i, r) =>
                {
                    var time = r.ReadSingle();
                    var zFocus = r.ReadSingle();
                    var lensSize = r.ReadSingle();
                    var u01 = r.ReadInt32();

                    return new Key()
                    {
                        Time = time,
                        ZFocus = zFocus,
                        LensSize = lensSize,
                        Unknown = new object[] { u01 }
                    };
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.ZFocus);
                    w.Write(x.LensSize);
                    w.Write((int)x.Unknown[0]);
                });
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x03126002)]
        public class Chunk03126002 : Chunk<CGameCtnMediaBlockDOF>
        {
            public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, (i, r) =>
                {
                    var time = r.ReadSingle();
                    var zFocus = r.ReadSingle();
                    var lensSize = r.ReadSingle();
                    var u01 = r.ReadInt32();
                    var u02 = r.ReadSingle();
                    var u03 = r.ReadSingle();
                    var u04 = r.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        ZFocus = zFocus,
                        LensSize = lensSize,
                        Unknown = new object[] { u01, u02, u03, u04 }
                    };
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.ZFocus);
                    w.Write(x.LensSize);
                    w.Write((int)x.Unknown[0]);
                    w.Write((float)x.Unknown[1]);
                    w.Write((float)x.Unknown[2]);
                    w.Write((float)x.Unknown[3]);
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
