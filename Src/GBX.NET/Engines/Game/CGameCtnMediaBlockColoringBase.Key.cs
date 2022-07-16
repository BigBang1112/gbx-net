namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockColoringBase
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float hue;
        private float intensity;

        public short U01;

        public float Hue { get => hue; set => hue = value; }
        public float Intensity { get => intensity; set => intensity = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref hue);
            rw.Single(ref intensity);

            if (version >= 2)
            {
                rw.Int16(ref U01);
            }
        }
    }
}
