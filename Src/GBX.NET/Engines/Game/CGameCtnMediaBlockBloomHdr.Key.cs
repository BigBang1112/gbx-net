namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockBloomHdr
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float intensity;
        private float streaksIntensity;
        private float streaksAttenuation;

        public float Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        public float StreaksIntensity
        {
            get => streaksIntensity;
            set => streaksIntensity = value;
        }

        public float StreaksAttenuation
        {
            get => streaksAttenuation;
            set => streaksAttenuation = value;
        }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref intensity);
            rw.Single(ref streaksIntensity);
            rw.Single(ref streaksAttenuation);
        }
    }
}
