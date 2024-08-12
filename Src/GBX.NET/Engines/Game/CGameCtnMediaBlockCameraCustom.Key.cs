namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraCustom
{
    public new partial class Key : CGameCtnMediaBlock.Key
    {
        private Interpolation interpolation;
        private Vec3 position;
        private Vec3 pitchYawRoll;
        private float fov;
        private Vec3 targetPosition;
        private int anchor;
        private int target;
        private bool anchorVis;
        private bool anchorRot;
        private float? nearZ;

        private InterpVal? leftTangent;
        private InterpVal? rightTangent;

        public int? U01;
        public int? U02;
        public float? U03;
        public float? U04;
        public float? U05;
        public Quat? U06;
        public int? U07;
        public int? U08;
        public int? U09;

        public Interpolation Interpolation { get => interpolation; set => interpolation = value; }
        public Vec3 Position { get => position; set => position = value; }

        /// <summary>
        /// Pitch, yaw and roll in radians.
        /// </summary>
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        public float Fov { get => fov; set => fov = value; }
        public Vec3 TargetPosition { get => targetPosition; set => targetPosition = value; }
        public float? NearZ { get => nearZ; set => nearZ = value; }
        public int Anchor { get => anchor; set => anchor = value; }
        public int Target { get => target; set => target = value; }
        public bool AnchorVis { get => anchorVis; set => anchorVis = value; }
        public bool AnchorRot { get => anchorRot; set => anchorRot = value; }
        public InterpVal? LeftTangent { get => leftTangent; set => leftTangent = value; }
        public InterpVal? RightTangent { get => rightTangent; set => rightTangent = value; }

        /// <exception cref="ChunkVersionNotSeenException"></exception>
        /// <exception cref="ChunkVersionNotSupportedException"></exception>
        public override void ReadWrite(GameBoxReaderWriter rw, int version)
        {
            base.ReadWrite(rw, version);

            rw.EnumInt32<Interpolation>(ref interpolation);
            
            if (version < 6)
            {
                rw.Int32(ref U01); // 0
                rw.Int32(ref U02); // 0
                rw.Vec3(ref position);
                rw.Vec3(ref pitchYawRoll); // in radians
                rw.Single(ref fov);
            }

            rw.Boolean(ref anchorRot);
            rw.Int32(ref anchor); // -1, entity number (SGameClipEntityId) or Id if 0x004
            rw.Boolean(ref anchorVis);
            rw.Int32(ref target); // -1, entity number (SGameClipEntityId) or Id if 0x004

            if (version < 6)
            {
                rw.Vec3(ref targetPosition);

                if (version == 1)
                {
                    rw.Single(ref U03); // 60
                    rw.Single(ref U04); // 30

                    return; // this is why leftTangent and rightTangent is nullable
                }

                //SGameCtnMediaBlockCameraCustomInterpVal::Reset
                InterpVal.ReadWrite(rw, version, ref leftTangent);
                InterpVal.ReadWrite(rw, version, ref rightTangent);

                if (version == 3)
                {
                    rw.Single(ref U05);
                    rw.Quat(ref U06);

                    // In theory this should throw VersionNotSeenException but this is already handled by the chunk exceptions
                }
            }

            if (version >= 6)
            {
                ReadWriteVersion6AndAbove(rw, version);
            }
        }

        /// <exception cref="ChunkVersionNotSeenException"></exception>
        /// <exception cref="ChunkVersionNotSupportedException"></exception>
        private void ReadWriteVersion6AndAbove(GameBoxReaderWriter rw, int version)
        {
            // SGameCtnMediaBlockCameraCustomInterpVal::Archive
            rw.Vec3(ref position);
            rw.Vec3(ref pitchYawRoll);
            rw.Single(ref fov);
            rw.Vec3(ref targetPosition);

            if (version >= 7)
            {
                rw.Single(ref nearZ);
            }

            // SGameCtnMediaBlockCameraCustomInterpVal::Archive
            InterpVal.ReadWrite(rw, version, ref leftTangent);

            // SGameCtnMediaBlockCameraCustomInterpVal::Archive
            InterpVal.ReadWrite(rw, version, ref rightTangent);

            if (version == 8)
            {
                rw.Int32(ref U07);
                rw.Int32(ref U08);

                ChunkVersionNotSeenExceptionHelper.ThrowIfVersionNotSeen(version);
            }

            if (version >= 10)
            {
                rw.Int32(ref U09);

                if (version >= 11)
                {
                    throw new ChunkVersionNotSupportedException(version);
                }
            }
        }
    }
}
