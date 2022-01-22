namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockFxCameraBlend
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float captureWeight;

        public float CaptureWeight
        {
            get => captureWeight;
            set => captureWeight = value;
        }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref captureWeight);
        }
    }
}
