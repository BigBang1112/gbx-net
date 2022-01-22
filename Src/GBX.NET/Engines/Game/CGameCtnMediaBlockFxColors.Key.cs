namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockFxColors
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float intensity;
        private float blendZ;
        private float distance;
        private float farDistance;
        private float inverse;
        private float hue;
        private float saturation;
        private float brightness;
        private float contrast;
        private Vec3 rgb;
        private float farInverse;
        private float farHue;
        private float farSaturation;
        private float farBrightness;
        private float farContrast;
        private Vec3 farRgb;

        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float FarU01;
        public float FarU02;
        public float FarU03;
        public float FarU04;

        public float Intensity { get => intensity; set => intensity = value; }
        public float BlendZ { get => blendZ; set => blendZ = value; }
        public float Distance { get => distance; set => distance = value; }
        public float FarDistance { get => farDistance; set => farDistance = value; }
        public float Inverse { get => inverse; set => inverse = value; }
        public float Hue { get => hue; set => hue = value; }
        public float Saturation { get => saturation; set => saturation = value; }
        public float Brightness { get => brightness; set => brightness = value; }
        public float Contrast { get => contrast; set => contrast = value; }
        public Vec3 Rgb { get => rgb; set => rgb = value; }
        public float FarInverse { get => farInverse; set => farInverse = value; }
        public float FarHue { get => farHue; set => farHue = value; }
        public float FarSaturation { get => farSaturation; set => farSaturation = value; }
        public float FarBrightness { get => farBrightness; set => farBrightness = value; }
        public float FarContrast { get => farContrast; set => farContrast = value; }
        public Vec3 FarRgb { get => farRgb; set => farRgb = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref intensity);
            rw.Single(ref blendZ);
            rw.Single(ref distance);
            rw.Single(ref farDistance);
            rw.Single(ref inverse);
            rw.Single(ref hue);
            rw.Single(ref saturation); // from center
            rw.Single(ref brightness); // from center
            rw.Single(ref contrast); // from center
            rw.Vec3(ref rgb);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref farInverse);
            rw.Single(ref farHue);
            rw.Single(ref farSaturation); // from center
            rw.Single(ref farBrightness); // from center
            rw.Single(ref farContrast); // from center
            rw.Vec3(ref farRgb);
            rw.Single(ref FarU01);
            rw.Single(ref FarU02);
            rw.Single(ref FarU03);
            rw.Single(ref FarU04);
        }
    }
}
