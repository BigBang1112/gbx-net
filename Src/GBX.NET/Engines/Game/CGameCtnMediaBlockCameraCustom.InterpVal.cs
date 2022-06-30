namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraCustom
{
    public class InterpVal : IReadableWritable
    {
        private Vec3 position;
        private Vec3? pitchYawRoll;
        private float? fov;
        private Vec3? targetPosition;
        private float? nearZ;

        public Vec3 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Pitch, yaw and roll in radians.
        /// </summary>
        public Vec3? PitchYawRoll
        {
            get => pitchYawRoll;
            set => pitchYawRoll = value;
        }

        public float? Fov
        {
            get => fov;
            set => fov = value;
        }

        public Vec3? TargetPosition
        {
            get => targetPosition;
            set => targetPosition = value;
        }

        public float? NearZ
        {
            get => nearZ;
            set => nearZ = value;
        }

        public void ReadWrite(GameBoxReaderWriter rw, int version)
        {
            rw.Vec3(ref position);

            if (version < 6)
            {
                return;
            }

            rw.Vec3(ref pitchYawRoll);
            rw.Single(ref fov);
            rw.Vec3(ref targetPosition);

            if (version >= 7)
            {
                rw.Single(ref nearZ);
            }
        }

        public static void ReadWrite(GameBoxReaderWriter rw, int version, ref InterpVal? interpVal)
        {
            var t = interpVal ?? new InterpVal();

            (t as IReadableWritable).ReadWrite(rw, version);

            if (rw.Reader is not null) // I don't understand this xd
            {
                interpVal = t;
            }
        }

        public override string ToString()
        {
            return $"InterpVal {{ {position} }}";
        }

        public static explicit operator Vec3(InterpVal interpVal)
        {
            return interpVal.position;
        }
    }
}
