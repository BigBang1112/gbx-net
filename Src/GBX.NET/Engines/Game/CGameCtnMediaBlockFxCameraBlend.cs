using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    [Node(0x0316D000)]
    public sealed class CGameCtnMediaBlockFxCameraBlend : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
    {
        #region Fields

        private IList<Key> keys;

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

        #endregion

        #region Constructors

        private CGameCtnMediaBlockFxCameraBlend()
        {
            keys = null!;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x0316D000)]
        public class Chunk0316D000 : Chunk<CGameCtnMediaBlockFxCameraBlend>, IVersionable
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaBlockFxCameraBlend n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);

                rw.List(ref n.keys!, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    CaptureWeight = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.CaptureWeight);
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
