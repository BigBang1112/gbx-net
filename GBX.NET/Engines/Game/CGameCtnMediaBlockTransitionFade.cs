using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030AB000)]
    public class CGameCtnMediaBlockTransitionFade : CGameCtnMediaBlock
    {
        #region Fields

        private Key[] keys;
        private Vec3 color;

        #endregion

        #region Properties

        [NodeMember]
        public Key[] Keys
        {
            get => keys;
            set => keys = value;
        }

        [NodeMember]
        public Vec3 Color
        {
            get => color;
            set => color = value;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x030AB000)]
        public class Chunk030AB000 : Chunk<CGameCtnMediaBlockTransitionFade>
        {
            public override void ReadWrite(CGameCtnMediaBlockTransitionFade n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, r => new Key()
                {
                    Time = r.ReadSingle(),
                    Opacity = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.Opacity);
                });

                rw.Vec3(ref n.color);
                rw.Single(Unknown);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Opacity { get; set; }
        }

        #endregion
    }
}
