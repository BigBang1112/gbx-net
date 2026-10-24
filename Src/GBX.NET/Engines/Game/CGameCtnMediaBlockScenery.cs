namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockScenery
{
    public partial class Key
    {
        [Obsolete("Use VortexRadius instead.")]
        public float U01 { get => VortexRadius; set => VortexRadius = value; }

        [Obsolete("Use VortexCenterXZ.X instead.")]
        public float U02 { get => VortexCenterXZ.X; set => VortexCenterXZ = VortexCenterXZ with { X = value }; }

        [Obsolete("Use VortexCenterXZ.Y instead.")]
        public float U03 { get => VortexCenterXZ.Y; set => VortexCenterXZ = VortexCenterXZ with { Y = value }; }
    }
}
