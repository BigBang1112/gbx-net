using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Camera effect script
    /// </summary>
    [Node(0x03161000)]
    public class CGameCtnMediaBlockCameraEffectScript : CGameCtnMediaBlockCameraEffect
    {
        #region Properties

        [NodeMember]
        public string Script { get; set; }

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraEffectScript 0x000 chunk
        /// </summary>
        [Chunk(0x03161000)]
        public class Chunk03161000 : Chunk<CGameCtnMediaBlockCameraEffectScript>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockCameraEffectScript n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Script = rw.String(n.Script);

                if(Version == 0) // Unverified
                {
                    rw.Single(Unknown);
                    rw.Single(Unknown);
                }

                n.Keys = rw.Array(n.Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        A = rw.Reader.ReadSingle(),
                        B = rw.Reader.ReadSingle(),
                        C = rw.Reader.ReadSingle()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.A);
                    rw.Writer.Write(x.B);
                    rw.Writer.Write(x.C);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float A { get; set; }
            public float B { get; set; }
            public float C { get; set; }
        }

        #endregion
    }
}
