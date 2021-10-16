using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Coloring base
    /// </summary>
    [Node(0x03172000)]
    public sealed class CGameCtnMediaBlockColoringBase : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
    {
        #region Fields

        private IList<Key> keys;
        private int baseIndex;

        #endregion

        #region Properties

        IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
        {
            get => keys.Cast<CGameCtnMediaBlock.Key>();
            set => keys = value.Cast<Key>().ToList();
        }

        [NodeMember]
        public IList<Key> Keys
        {
            get => keys;
            set => keys = value;
        }

        [NodeMember]
        public int BaseIndex
        {
            get => baseIndex;
            set => baseIndex = value;
        }

        #endregion

        #region Constructors

        private CGameCtnMediaBlockColoringBase()
        {
            keys = null!;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockColoringBase 0x000 chunk
        /// </summary>
        [Chunk(0x03172000)]
        public class Chunk03172000 : Chunk<CGameCtnMediaBlockColoringBase>, IVersionable
        {
            private int version;

            public int U01;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaBlockColoringBase n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref U01);

                rw.List(ref n.keys!, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    Hue = r.ReadSingle(),
                    Intensity = r.ReadSingle(),
                    Unknown = r.ReadInt16()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.Hue);
                    w.Write(x.Intensity);
                    w.Write(x.Unknown);
                });
                
                rw.Int32(ref n.baseIndex);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Hue { get; set; }
            public float Intensity { get; set; }
            public short Unknown { get; set; }
        }

        #endregion
    }
}
