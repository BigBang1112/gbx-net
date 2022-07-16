namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockFog
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float intensity;
        private float skyIntensity;
        private float distance;
        private float? coefficient;
        private Vec3? color;
        private float? cloudsOpacity;
        private float? cloudsSpeed;

        public float Intensity { get => intensity; set => intensity = value; }
        public float SkyIntensity { get => skyIntensity; set => skyIntensity = value; }
        public float Distance { get => distance; set => distance = value; }
        public float? Coefficient { get => coefficient; set => coefficient = value; }
        public Vec3? Color { get => color; set => color = value; }
        public float? CloudsOpacity { get => cloudsOpacity; set => cloudsOpacity = value; }
        public float? CloudsSpeed { get => cloudsSpeed; set => cloudsSpeed = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref intensity);
            rw.Single(ref skyIntensity);
            rw.Single(ref distance);

            if (version >= 1)
            {
                rw.Single(ref coefficient);
                rw.Vec3(ref color);

                if (version >= 2)
                {
                    rw.Single(ref cloudsOpacity);
                    rw.Single(ref cloudsSpeed);
                }
            }
        }
    }
}
