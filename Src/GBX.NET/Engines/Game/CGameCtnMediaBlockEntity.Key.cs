namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockEntity
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        public enum ELights
        {
            Auto,
            On,
            Off
        }
        
        private ELights lights;
        private float? u01;
        private int? u02;
        private int? u03;
        private float trailIntensity = 1;
        private float selfIllumIntensity = 1;

        public ELights Lights { get => lights; set => lights = value; }
        public float? U01 { get => u01; set => u01 = value; }
        public int? U02 { get => u02; set => u02 = value; }
        public int? U03 { get => u03; set => u03 = value; }
        public float TrailIntensity { get => trailIntensity; set => trailIntensity = value; }
        public float SelfIllumIntensity { get => selfIllumIntensity; set => selfIllumIntensity = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.EnumInt32<ELights>(ref lights);

            if (version >= 6)
            {
                rw.Single(ref u01);
                rw.Int32(ref u02);
                rw.Int32(ref u03);
                rw.Single(ref trailIntensity);

                if (version >= 9)
                {
                    rw.Single(ref selfIllumIntensity);
                }
            }
        }
    }
}
