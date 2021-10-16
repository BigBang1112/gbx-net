using System;
using TmEssentials;

namespace GBX.NET
{
    public class Sample
    {
        public TimeSpan? Timestamp { get; internal set; }

        public byte? BufferType { get; set; }
        public Vec3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vec3 PitchYawRoll => Rotation.ToPitchYawRoll();
        public float Speed { get; set; }
        public Vec3 Velocity { get; set; }
        public byte[]? Unknown { get; set; }

        public override string ToString()
        {
            if (!BufferType.HasValue || BufferType == 0 || BufferType == 2 || BufferType == 4)
            {
                if (Timestamp.HasValue)
                    return $"Sample: {Timestamp.Value.ToTmString()} {Position}";
                return $"Sample: {Position}";
            }

            return $"Sample: {BufferType.ToString() ?? "unknown"}";
        }
    }
}
