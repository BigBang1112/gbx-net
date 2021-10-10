using System;

namespace GBX.NET.Engines.Game
{
    [Node(0x03195000)]
    public sealed class CGameCtnMediaBlockInterface : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasTwoKeys
    {
        #region Fields

        private TimeSpan start;
        private TimeSpan end = TimeSpan.FromSeconds(3);
        private bool showInterface;
        private string manialink;

        #endregion

        #region Properties

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

        [NodeMember]
        public bool ShowInterface
        {
            get => showInterface;
            set => showInterface = value;
        }

        [NodeMember]
        public string Manialink
        {
            get => manialink;
            set => manialink = value;
        }

        #endregion

        #region Constructors

        private CGameCtnMediaBlockInterface()
        {
            manialink = null!;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03195000)]
        public class Chunk03195000 : Chunk<CGameCtnMediaBlockInterface>, IVersionable
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaBlockInterface n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Single_s(ref n.start);
                rw.Single_s(ref n.end);
                rw.Boolean(ref n.showInterface);
                rw.String(ref n.manialink!);
            }
        }

        #endregion

        #endregion
    }
}
