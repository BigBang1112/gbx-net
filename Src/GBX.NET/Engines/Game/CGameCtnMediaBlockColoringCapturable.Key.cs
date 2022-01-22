namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockColoringCapturable
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float hue;
        private float gauge;
        private int emblem;

        public float Hue
        {
            get => hue;
            set => hue = value;
        }

        public float Gauge
        {
            get => gauge;
            set => gauge = value;
        }

        public int Emblem
        {
            get => emblem;
            set => emblem = value;
        }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref hue);
            rw.Single(ref gauge);
            rw.Int32(ref emblem);
        }
    }
}
