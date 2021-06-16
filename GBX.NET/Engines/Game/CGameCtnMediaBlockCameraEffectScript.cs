using System;
using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// MediaTracker block - Camera effect script
    /// </summary>
    [Node(0x03161000)]
    public class CGameCtnMediaBlockCameraEffectScript : CGameCtnMediaBlockCameraEffect,
        CGameCtnMediaBlock.IHasKeys,
        CGameCtnMediaBlock.IHasTwoKeys
    {
        #region Fields

        private string script;
        private IList<Key> keys = new List<Key>();
        private TimeSpan start;
        private TimeSpan end = TimeSpan.FromSeconds(3);

        #endregion

        #region Properties

        IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
        {
            get => keys.Cast<CGameCtnMediaBlock.Key>();
            set => keys = value.Cast<Key>().ToList();
        }

        [NodeMember]
        public string Script
        {
            get => script;
            set => script = value;
        }

        [NodeMember]
        public IList<Key> Keys
        {
            get => keys;
            set => keys = value;
        }

        [NodeMember]
        public TimeSpan Start
        {
            get => start;
            set => start = value;
        }

        [NodeMember]
        public TimeSpan End
        {
            get => end;
            set => end = value;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CGameCtnMediaBlockCameraEffectScript 0x000 chunk
        /// </summary>
        [Chunk(0x03161000)]
        public class Chunk03161000 : Chunk<CGameCtnMediaBlockCameraEffectScript>, IVersionable
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaBlockCameraEffectScript n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.String(ref n.script);

                if (version == 0) // Unverified
                {
                    rw.Single_s(ref n.start);
                    rw.Single_s(ref n.end);
                }

                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    A = r.ReadSingle(),
                    B = r.ReadSingle(),
                    C = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.A);
                    w.Write(x.B);
                    w.Write(x.C);
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
