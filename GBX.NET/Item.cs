using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GBX.NET
{
    public class Item
    {
        public Meta Meta { get; set; }
        public Vec3? PitchYawRoll { get; set; }
        public Int3 BlockCoord { get; set; }
        public Vec3 Position { get; set; }
        public Vec3? PivotPosition { get; set; }
        public float? Scale { get; set; }
    }
}
