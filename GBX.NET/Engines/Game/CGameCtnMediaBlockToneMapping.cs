using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    [Node(0x03127000)]
    public sealed class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
    {
        #region Fields

        private IList<Key> keys;

        #endregion

        #region Properties

        IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
        {
            get => Keys.Cast<CGameCtnMediaBlock.Key>();
            set => Keys = value.Cast<Key>().ToList();
        }

        [NodeMember]
        public IList<Key> Keys
        {
            get => keys;
            set => keys = value;
        }

        #endregion

        #region Constructors

        private CGameCtnMediaBlockToneMapping()
        {
            keys = null!;
        }

        #endregion

        #region Chunks

        #region 0x004 chunk

        [Chunk(0x03127004)]
        public class Chunk03127004 : Chunk<CGameCtnMediaBlockToneMapping>
        {
            public override void ReadWrite(CGameCtnMediaBlockToneMapping n, GameBoxReaderWriter rw)
            {
                rw.List(ref n.keys!, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    Exposure = r.ReadSingle(),
                    MaxHDR = r.ReadSingle(),
                    LightTrailScale = r.ReadSingle(),
                    Unknown = r.ReadInt32()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.Exposure);
                    w.Write(x.MaxHDR);
                    w.Write(x.LightTrailScale);
                    w.Write(x.Unknown);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Exposure { get; set; }
            public float MaxHDR { get; set; }
            public float LightTrailScale { get; set; }
            public int Unknown { get; set; }
        }

        #endregion
    }
}
