namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraOrbital
{
    public partial class Key
    {
        [Obsolete("Use ArchiveMarker instead.")]
        public byte U01 { get => ArchiveMarker; set => ArchiveMarker = value; }

        [Obsolete("Use ClipEntId instead.")]
        public int U02 { get => ClipEntId; set => ClipEntId = value; }

        [Obsolete("Use TargetOffset.X instead.")]
        public float U04 { get => TargetOffset.X; set => TargetOffset = TargetOffset with { X = value }; }

        [Obsolete("Use TargetOffset.Y instead.")]
        public float U05 { get => TargetOffset.Y; set => TargetOffset = TargetOffset with { Y = value }; }

        [Obsolete("Use TargetOffset.Z instead.")]
        public float U06 { get => TargetOffset.Z; set => TargetOffset = TargetOffset with { Z = value }; }
    }
}
