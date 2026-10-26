namespace GBX.NET.Engines.Plug;

public partial class CPlugCrystal
{
    public partial class Crystal
    {
        [Obsolete("Use BaseVisualLevel instead.")]
        public int U01 { get => BaseVisualLevel; set => BaseVisualLevel = value; }

        [Obsolete("Use MaxMaterialIndex instead.")]
        public int U02 { get => MaxMaterialIndex; set => MaxMaterialIndex = value; }

        [Obsolete("Use MaxGroupIndex instead.")]
        public int U03 { get => MaxGroupIndex; set => MaxGroupIndex = value; }
    }

    public partial class Part
    {
        [Obsolete("Use IsInUse instead.")]
        public int U02 { get => IsInUse ? 1 : 0; set => IsInUse = value != 0; }

        [Obsolete("Use ParentIndex instead.")]
        public int U03 { get => ParentIndex; set => ParentIndex = value; }

        [Obsolete("Use AnchorIndex instead.")]
        public int U04 { get => AnchorIndex; set => AnchorIndex = value; }

        [Obsolete("Use Children instead.")]
        public int[]? U05 { get => Children; set => Children = value; }
    }

    public partial class SmoothLayer
    {
        [Obsolete("Use SmoothFactor instead.")]
        public float U01 { get => SmoothFactor; set => SmoothFactor = value; }

        [Obsolete("Use Independently instead.")]
        public bool U02 { get => Independently; set => Independently = value; }
    }

    public partial class GeometryLayer
    {
        [Obsolete("Use GroupIndices instead.")]
        public int[]? U02 { get => GroupIndices; set => GroupIndices = value; }
    }

    public partial class MoveToGroundLayer
    {
        [Obsolete("Use Independently instead.")]
        public bool U01 { get => Independently; set => Independently = value; }
    }

    public partial class TriggerLayer
    {
        [Obsolete("Use GroupIndices instead.")]
        public int[]? U01 { get => GroupIndices; set => GroupIndices = value; }
    }

    public partial class ChaosLayer
    {
        [Obsolete("Use Seed instead.")]
        public int U01 { get => Seed; set => Seed = value; }
    }

    public partial class DeformationLayer
    {
        [Obsolete("Use Box instead.")]
        public BoxAligned U01 { get => Box; set => Box = value; }

        [Obsolete("Use Transform instead.")]
        public Iso4 U02 { get => Transform; set => Transform = value; }
    }
}
