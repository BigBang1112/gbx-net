namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockSkel
{
    public partial class Key
    {
        [Obsolete("Use RootTranslation instead.")]
        public float U01 { get => RootTranslation.X; set => RootTranslation = RootTranslation with { X = value }; }

        [Obsolete("Use RootTranslation instead.")]
        public float U02 { get => RootTranslation.Y; set => RootTranslation = RootTranslation with { Y = value }; }

        [Obsolete("Use RootTranslation instead.")]
        public float U03 { get => RootTranslation.Z; set => RootTranslation = RootTranslation with { Z = value }; }

        [Obsolete("Use RootYaw instead.")]
        public float U04 { get => RootYaw; set => RootYaw = value; }

        [Obsolete("Use JointTransforms instead.")]
        public TransQuat[]? U05 { get => JointTransforms; set => JointTransforms = value; }
    }
}
