namespace GBX.NET.Engines.Plug;

public partial class CPlugDayTime
{
    public partial class FogMatterId
    {
        [Obsolete("Use Density instead.")]
        public float U01 { get => Density; set => Density = value; }

        [Obsolete("Use Color.X instead.")]
        public float U02 { get => Color.X; set => Color = Color with { X = value }; }

        [Obsolete("Use Color.Y instead.")]
        public float U03 { get => Color.Y; set => Color = Color with { Y = value }; }

        [Obsolete("Use Color.Z instead.")]
        public float U04 { get => Color.Z; set => Color = Color with { Z = value }; }
    }
}
