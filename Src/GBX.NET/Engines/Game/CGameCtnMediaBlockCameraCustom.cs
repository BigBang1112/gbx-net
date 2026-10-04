namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraCustom
{
    public partial class Key
    {
        [Obsolete("Use LegacyTargetSceneUId instead.")]
        public int? U07 { get => LegacyTargetSceneUId; set => LegacyTargetSceneUId = value; }

        [Obsolete("Use LegacyAnchorSceneUId instead.")]
        public int? U08 { get => LegacyAnchorSceneUId; set => LegacyAnchorSceneUId = value; }

        [Obsolete("Use PssmDistScale instead.")]
        public float? U09 { get => PssmDistScale; set => PssmDistScale = value; }
    }

    public partial class InterpVal
    {
        [Obsolete("Use PssmDistScale instead.")]
        public float? U01 { get => PssmDistScale; set => PssmDistScale = value; }
    }
}
