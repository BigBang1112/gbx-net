using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    [Node(0x03080000)]
    public class CGameCtnMediaBlockFxColors : CGameCtnMediaBlockFx
    {
        #region Properties

        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x003 chunk

        [Chunk(0x03080003)]
        public class Chunk03080003 : Chunk<CGameCtnMediaBlockFxColors>
        {
            public override void Read(CGameCtnMediaBlockFxColors n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 => new Key()
                {
                    Time = r1.ReadSingle(),
                    Intensity = r1.ReadSingle(),
                    BlendZ = r1.ReadSingle(),
                    Distance = r1.ReadSingle(),
                    FarDistance = r1.ReadSingle(),
                    Inverse = r1.ReadSingle(),
                    Hue = r1.ReadSingle(),
                    Saturation = r1.ReadSingle(), // from center
                    Brightness = r1.ReadSingle(), // from center
                    Contrast = r1.ReadSingle(), // from center
                    RGB = r1.ReadVec3(),
                    U01 = r1.ReadSingle(),
                    U02 = r1.ReadSingle(),
                    U03 = r1.ReadSingle(),
                    U04 = r1.ReadSingle(),
                    FarInverse = r1.ReadSingle(),
                    FarHue = r1.ReadSingle(),
                    FarSaturation = r1.ReadSingle(), // from center
                    FarBrightness = r1.ReadSingle(), // from center
                    FarContrast = r1.ReadSingle(), // from center
                    FarRGB = r1.ReadVec3(),
                    FarU01 = r1.ReadSingle(),
                    FarU02 = r1.ReadSingle(),
                    FarU03 = r1.ReadSingle(),
                    FarU04 = r1.ReadSingle()
                });
            }

            public override void Write(CGameCtnMediaBlockFxColors n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.Intensity);
                    w1.Write(x.BlendZ);
                    w1.Write(x.Distance);
                    w1.Write(x.FarDistance);
                    w1.Write(x.Inverse);
                    w1.Write(x.Hue);
                    w1.Write(x.Saturation);
                    w1.Write(x.Brightness);
                    w1.Write(x.Contrast);
                    w1.Write(x.RGB);
                    w1.Write(x.U01);
                    w1.Write(x.U02);
                    w1.Write(x.U03);
                    w1.Write(x.U04);
                    w1.Write(x.FarInverse);
                    w1.Write(x.FarHue);
                    w1.Write(x.FarSaturation);
                    w1.Write(x.FarBrightness);
                    w1.Write(x.FarContrast);
                    w1.Write(x.FarRGB);
                    w1.Write(x.FarU01);
                    w1.Write(x.FarU02);
                    w1.Write(x.FarU03);
                    w1.Write(x.FarU04);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Intensity { get; set; }
            public float BlendZ { get; set; }
            public float Distance { get; set; }
            public float FarDistance { get; set; }
            public float Inverse { get; set; }
            public float Hue { get; set; }
            public float Saturation { get; set; }
            public float Brightness { get; set; }
            public float Contrast { get; set; }
            public Vec3 RGB { get; set; }
            public float U01 { get; set; }
            public float U02 { get; set; }
            public float U03 { get; set; }
            public float U04 { get; set; }
            public float FarInverse { get; set; }
            public float FarHue { get; set; }
            public float FarSaturation { get; set; }
            public float FarBrightness { get; set; }
            public float FarContrast { get; set; }
            public Vec3 FarRGB { get; set; }
            public float FarU01 { get; set; }
            public float FarU02 { get; set; }
            public float FarU03 { get; set; }
            public float FarU04 { get; set; }
        }

        #endregion
    }
}
