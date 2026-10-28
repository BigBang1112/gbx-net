namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraSimple
{
    public partial class Key
    {
        [Obsolete("Use ArchiveMarker instead.")]
        public byte U01 { get => ArchiveMarker; set => ArchiveMarker = value; }

        [Obsolete("Use Transform instead.")]
        public Iso4 U02 { get => Transform; set => Transform = value; }

        [Obsolete("Use Fov instead.")]
        public float U03 { get => Fov; set => Fov = value; }

        [Obsolete("Use NearZ instead.")]
        public float U04 { get => NearZ; set => NearZ = value; }

        [Obsolete("Use FarZ instead.")]
        public float U05 { get => FarZ; set => FarZ = value; }
    }
}
