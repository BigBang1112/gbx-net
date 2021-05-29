using GBX.NET.Engines.Game;

namespace GBX.NET.Engines.TrackMania
{
    [Node(0x24092000)]
    public class CCtnMediaBlockUiTMSimpleEvtsDisplay : CGameCtnMediaBlockUi
    {
        #region Enums

        public enum EDisplayMode
        {
            OnlyTarget,
            Always,
            Never
        }

        #endregion

        #region Fields

        private EDisplayMode displayMode;
        private bool stuntFigures;
        private bool checkpoints;
        private bool endOfRace;
        private bool endOfLaps;
        private bool ghostsName;

        #endregion

        #region Properties

        public EDisplayMode DisplayMode
        {
            get => displayMode;
            set => displayMode = value;
        }

        public bool StuntFigures
        {
            get => stuntFigures;
            set => stuntFigures = value;
        }

        public bool Checkpoints
        {
            get => checkpoints;
            set => checkpoints = value;
        }

        public bool EndOfRace
        {
            get => endOfRace;
            set => endOfRace = value;
        }

        public bool EndOfLaps
        {
            get => endOfLaps;
            set => endOfLaps = value;
        }

        public bool GhostsName
        {
            get => ghostsName;
            set => ghostsName = value;
        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x24092000)]
        public class Chunk24092000 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
        {
            public override void Read(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.displayMode = (EDisplayMode)r.ReadInt32();
            }

            public override void Write(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxWriter w, GameBoxReader unknownR)
            {
                if (n.displayMode == EDisplayMode.Always || n.displayMode == EDisplayMode.Never)
                    w.Write(true);
                else
                    w.Write(false);
            }
        }

        #endregion

        #region 0x001 chunk

        [Chunk(0x24092001)]
        public class Chunk24092001 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
        {
            public override void ReadWrite(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxReaderWriter rw)
            {
                rw.Boolean(ref n.stuntFigures);
                rw.Boolean(ref n.checkpoints);
                rw.Boolean(ref n.endOfRace);
                rw.Boolean(ref n.endOfLaps);
                rw.Boolean(ref n.ghostsName);
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x24092002)]
        public class Chunk24092002 : Chunk<CCtnMediaBlockUiTMSimpleEvtsDisplay>
        {
            public override void Read(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var displayMode = r.ReadBoolean();
                if (displayMode) n.displayMode = EDisplayMode.Never;
            }

            public override void Write(CCtnMediaBlockUiTMSimpleEvtsDisplay n, GameBoxWriter w, GameBoxReader unknownR)
            {
                if (n.displayMode == EDisplayMode.Never)
                    w.Write(true);
                else
                    w.Write(false);
            }
        }

        #endregion

        #endregion
    }
}
