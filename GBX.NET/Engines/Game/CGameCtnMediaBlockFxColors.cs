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
                n.Keys = r.ReadArray(() => new Key()
                {
                    Time = r.ReadSingle(),
                    Intensity = r.ReadSingle(),
                    BlendZ = r.ReadSingle(),
                    Distance = r.ReadSingle(),
                    FarDistance = r.ReadSingle(),
                    Inverse = r.ReadSingle(),
                    Hue = r.ReadSingle(),
                    Saturation = r.ReadSingle(), // from center
                    Brightness = r.ReadSingle(), // from center
                    Contrast = r.ReadSingle(), // from center
                    RGB = r.ReadVec3(),
                    U01 = r.ReadSingle(),
                    U02 = r.ReadSingle(),
                    U03 = r.ReadSingle(),
                    U04 = r.ReadSingle(),
                    FarInverse = r.ReadSingle(),
                    FarHue = r.ReadSingle(),
                    FarSaturation = r.ReadSingle(), // from center
                    FarBrightness = r.ReadSingle(), // from center
                    FarContrast = r.ReadSingle(), // from center
                    FarRGB = r.ReadVec3(),
                    FarU01 = r.ReadSingle(),
                    FarU02 = r.ReadSingle(),
                    FarU03 = r.ReadSingle(),
                    FarU04 = r.ReadSingle()
                });
            }

            public override void Write(CGameCtnMediaBlockFxColors n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                    w.Write(x.BlendZ);
                    w.Write(x.Distance);
                    w.Write(x.FarDistance);
                    w.Write(x.Inverse);
                    w.Write(x.Hue);
                    w.Write(x.Saturation);
                    w.Write(x.Brightness);
                    w.Write(x.Contrast);
                    w.Write(x.RGB);
                    w.Write(x.U01);
                    w.Write(x.U02);
                    w.Write(x.U03);
                    w.Write(x.U04);
                    w.Write(x.FarInverse);
                    w.Write(x.FarHue);
                    w.Write(x.FarSaturation);
                    w.Write(x.FarBrightness);
                    w.Write(x.FarContrast);
                    w.Write(x.FarRGB);
                    w.Write(x.FarU01);
                    w.Write(x.FarU02);
                    w.Write(x.FarU03);
                    w.Write(x.FarU04);
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
