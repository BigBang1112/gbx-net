using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Collections.Specialized;

namespace GBX.NET
{
    public class Sample
    {
        public TimeSpan? Timestamp { get; internal set; }

        public byte BufferType { get; set; }
        public Vec3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vec3 PitchYawRoll => Rotation.ToPitchYawRoll();
        public float Speed { get; set; }
        public Vec3 Velocity { get; set; }
        public byte[] Unknown { get; set; }

        public override string ToString()
        {
            if (BufferType == 0)
            {
                if (Timestamp.HasValue)
                    return $"Sample: {Timestamp.Value.ToStringTM()} {Position}";
                return $"Sample: {Position}";
            }

            return $"Sample: {BufferType}";
        }
    }
}
