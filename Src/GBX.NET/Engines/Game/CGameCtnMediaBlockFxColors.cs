namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockFxColors
{
    public partial class Key
    {
        [Obsolete("Use FarIntensity instead.")]
        public float BlendZ { get => FarIntensity; set => FarIntensity = value; }

        [Obsolete("Use ModulateRgb instead.")]
        public Vec3 Rgb { get => ModulateRgb; set => ModulateRgb = value; }

        [Obsolete("Use BlendRgb instead.")]
        public float U01 { get => BlendRgb.X; set => BlendRgb = BlendRgb with { X = value }; }

        [Obsolete("Use BlendRgb instead.")]
        public float U02 { get => BlendRgb.Y; set => BlendRgb = BlendRgb with { Y = value }; }

        [Obsolete("Use BlendRgb instead.")]
        public float U03 { get => BlendRgb.Z; set => BlendRgb = BlendRgb with { Z = value }; }

        [Obsolete("Use BlendAlpha instead.")]
        public float U04 { get => BlendAlpha; set => BlendAlpha = value; }

        [Obsolete("Use FarModulateRgb instead.")]
        public Vec3 FarRgb { get => FarModulateRgb; set => FarModulateRgb = value; }

        [Obsolete("Use FarBlendRgb instead.")]
        public float FarU01 { get => FarBlendRgb.X; set => FarBlendRgb = FarBlendRgb with { X = value }; }

        [Obsolete("Use FarBlendRgb instead.")]
        public float FarU02 { get => FarBlendRgb.Y; set => FarBlendRgb = FarBlendRgb with { Y = value }; }

        [Obsolete("Use FarBlendRgb instead.")]
        public float FarU03 { get => FarBlendRgb.Z; set => FarBlendRgb = FarBlendRgb with { Z = value }; }

        [Obsolete("Use FarBlendAlpha instead.")]
        public float FarU04 { get => FarBlendAlpha; set => FarBlendAlpha = value; }
    }
}
