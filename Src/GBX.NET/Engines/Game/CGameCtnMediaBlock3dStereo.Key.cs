namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlock3dStereo
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float upToMax;
        private float screenDist;

        public float UpToMax
        {
            get => upToMax;
            set => upToMax = value;
        }

        public float ScreenDist
        {
            get => screenDist;
            set => screenDist = value;
        }

        protected internal override void ReadWrite<TChunk>(GameBoxReaderWriter rw)
        {
            base.ReadWrite<TChunk>(rw);

            rw.Single(ref upToMax);
            rw.Single(ref screenDist);
        }
    }
}
