namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraPath
{
    public partial class Key
    {
        [Obsolete("Use Rotation instead.")]
        public Quat U01 { get => Rotation; set => Rotation = value; }

        [Obsolete("Use LegacyTargetSceneUId instead.")]
        public int U02 { get => LegacyTargetSceneUId; set => LegacyTargetSceneUId = value; }

        [Obsolete("Use LegacyAnchorSceneUId instead.")]
        public int U03 { get => LegacyAnchorSceneUId; set => LegacyAnchorSceneUId = value; }
    }
}
