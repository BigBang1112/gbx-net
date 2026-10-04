namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public partial class Chunk0310D005
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.ArrayString(ref n.legacyDecalModelPaths!);
            // The decal count precedes the version, rather than the decal array itself.
            U01 = rw.Int32(n.legacyDecals?.Length ?? 0);
            rw.Int32(ref U02);
            rw.ArrayReadableWritable(ref n.legacyDecals!, U01);
        }
    }

    public partial class BlockSpawn
    {
        [Obsolete("Use YawPitchRoll instead. This property does not swap Pitch and Yaw.")]
        public Vec3 PitchYawRoll
        {
            get => YawPitchRoll;
            set => YawPitchRoll = value;
        }
    }

    public partial class ObjectSpawn
    {
        [Obsolete("Use YawPitchRoll instead. This property does not swap Pitch and Yaw.")]
        public Vec3 PitchYawRoll
        {
            get => YawPitchRoll;
            set => YawPitchRoll = value;
        }
    }
}
