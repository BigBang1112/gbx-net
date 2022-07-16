namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraPath
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private Vec3 position;
        private Vec3 pitchYawRoll;
        private float fov;
        private float? nearZ;
        private int anchor;
        private bool anchorVis;
        private bool anchorRot;
        private int target;
        private Vec3 targetPosition;
        private float weight;

        public Quat U01;
        public int? U02;
        public int? U03;

        public Vec3 Position { get => position; set => position = value; }

        /// <summary>
        /// Pitch, yaw and roll in radians.
        /// </summary>
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        public float Fov { get => fov; set => fov = value; }
        public float? NearZ { get => nearZ; set => nearZ = value; }
        public int Anchor { get => anchor; set => anchor = value; }
        public bool AnchorVis { get => anchorVis; set => anchorVis = value; }
        public bool AnchorRot { get => anchorRot; set => anchorRot = value; }
        public int Target { get => target; set => target = value; }
        public Vec3 TargetPosition { get => targetPosition; set => targetPosition = value; }
        public float Weight { get => weight; set => weight = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Vec3(ref position);
            rw.Vec3(ref pitchYawRoll); // in radians
            rw.Single(ref fov);

            if (version >= 3)
            {
                rw.Single(ref nearZ);
            }

            rw.Boolean(ref anchorRot);
            rw.Int32(ref anchor);
            rw.Boolean(ref anchorVis);
            rw.Int32(ref target);
            rw.Vec3(ref targetPosition);

            rw.Single(ref weight);
            rw.Quat(ref U01);

            if (version >= 4)
            {
                rw.Int32(ref U02, -1); // 5 or -1
                rw.Int32(ref U03, -1); // 1699124 or -1
            }
        }
    }
}
