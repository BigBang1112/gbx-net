namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockToneMapping
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float exposure;
        private float maxHDR;
        private float lightTrailScale;

        public int U01;

        public float Exposure { get => exposure; set => exposure = value; }
        public float MaxHDR { get => maxHDR; set => maxHDR = value; }
        public float LightTrailScale { get => lightTrailScale; set => lightTrailScale = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref exposure);
            rw.Single(ref maxHDR);
            rw.Single(ref lightTrailScale);
            rw.Int32(ref U01);
        }
    }
}
