namespace GBX.NET.Engines.Control;

public partial class CControlEffectSimi
{
    public partial class Key
    {
        [Obsolete("Use ColorBlend instead.")]
        public float U01 { get => ColorBlend; set => ColorBlend = value; }

        [Obsolete("Use Color.X instead.")]
        public float U02 { get => Color.X; set => Color = Color with { X = value }; }

        [Obsolete("Use Color.Y instead.")]
        public float U03 { get => Color.Y; set => Color = Color with { Y = value }; }

        [Obsolete("Use Color.Z instead.")]
        public float U04 { get => Color.Z; set => Color = Color with { Z = value }; }
    }
}
