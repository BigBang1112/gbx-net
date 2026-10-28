namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmap
{
    public partial class SpecularSubMapCat
    {
        [Obsolete("Use SpecularRGB.X instead.")]
        public float U01 { get => SpecularRGB.X; set => SpecularRGB = SpecularRGB with { X = value }; }

        [Obsolete("Use SpecularRGB.Y instead.")]
        public float U02 { get => SpecularRGB.Y; set => SpecularRGB = SpecularRGB with { Y = value }; }

        [Obsolete("Use SpecularRGB.Z instead.")]
        public float U03 { get => SpecularRGB.Z; set => SpecularRGB = SpecularRGB with { Z = value }; }

        [Obsolete("Use SpecularExp instead.")]
        public float U04 { get => SpecularExp; set => SpecularExp = value; }
    }
}
