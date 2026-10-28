namespace GBX.NET.Engines.Hms;

public partial class CHmsLightMapCache
{
    public partial class SFrame
    {
        [Obsolete("Use LAmbient instead.")]
        public Vec3 U09 { get => LAmbient; set => LAmbient = value; }

        [Obsolete("Use StoreLAmbient instead.")]
        public int U10 { get => StoreLAmbient ? 1 : 0; set => StoreLAmbient = value != 0; }

        [Obsolete("Use LocalLight_Switch instead.")]
        public int U12 { get => (int)LocalLight_Switch; set => LocalLight_Switch = (ELocalLightSwitch)value; }

        [Obsolete("Use U01 instead.")]
        public float U13 { get => U01; set => U01 = value; }

        [Obsolete("Use U02 instead.")]
        public float U14 { get => U02; set => U02 = value; }

        [Obsolete("Use U03 instead.")]
        public float U15 { get => U03; set => U03 = value; }

        [Obsolete("Use U04 instead.")]
        public float U16 { get => U04; set => U04 = value; }

        [Obsolete("Use U05 instead.")]
        public float U17 { get => U05; set => U05 = value; }

        [Obsolete("Use U06 instead.")]
        public float U18 { get => U06; set => U06 = value; }

        [Obsolete("Use U07 instead.")]
        public float U19 { get => U07; set => U07 = value; }

        [Obsolete("Use MaxHDR_HBasisScaled234.X instead.")]
        public float U20 { get => MaxHDR_HBasisScaled234.X; set => MaxHDR_HBasisScaled234 = MaxHDR_HBasisScaled234 with { X = value }; }

        [Obsolete("Use MaxHDR_HBasisScaled234.Y instead.")]
        public float U21 { get => MaxHDR_HBasisScaled234.Y; set => MaxHDR_HBasisScaled234 = MaxHDR_HBasisScaled234 with { Y = value }; }

        [Obsolete("Use MaxHDR_HBasisScaled234.Z instead.")]
        public float U22 { get => MaxHDR_HBasisScaled234.Z; set => MaxHDR_HBasisScaled234 = MaxHDR_HBasisScaled234 with { Z = value }; }

        [Obsolete("Use Bump instead.")]
        public int U23 { get => (int)Bump; set => Bump = (EBump)value; }
    }
}
