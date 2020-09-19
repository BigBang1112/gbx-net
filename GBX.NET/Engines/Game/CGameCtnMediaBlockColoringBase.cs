using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Coloring base
    /// </summary>
    [Node(0x03172000)]
    public class CGameCtnMediaBlockColoringBase : CGameCtnMediaBlock
    {
        public Key[] Keys { get; set; }
        public int BaseIndex { get; set; }

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockColoringBase 0x000 chunk
        /// </summary>
        [Chunk(0x03172000)]
        public class Chunk03172000 : Chunk<CGameCtnMediaBlockColoringBase>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockColoringBase n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                rw.Int32(Unknown);

                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Hue = rw.Reader.ReadSingle(),
                    Intensity = rw.Reader.ReadSingle(),
                    Unknown = rw.Reader.ReadInt16()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Hue);
                    rw.Writer.Write(x.Intensity);
                    rw.Writer.Write(x.Unknown);
                });
                
                n.BaseIndex = rw.Int32(n.BaseIndex);
            }
        }

        #endregion

        #endregion

        public class Key : MediaBlockKey
        {
            public float Hue { get; set; }
            public float Intensity { get; set; }
            public short Unknown { get; set; }
        }
    }
}
