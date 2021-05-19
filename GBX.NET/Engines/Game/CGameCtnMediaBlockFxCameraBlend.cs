using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0316D000)]
    public class CGameCtnMediaBlockFxCameraBlend : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x0316D000)]
        public class Chunk0316D000 : Chunk<CGameCtnMediaBlockFxCameraBlend>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockFxCameraBlend n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Keys = rw.Array(n.Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        CaptureWeight = rw.Reader.ReadSingle()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.CaptureWeight);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float CaptureWeight { get; set; }
        }

        #endregion
    }
}
