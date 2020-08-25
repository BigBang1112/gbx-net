using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GBX.NET
{
    public class Item
    {
        public Meta Meta { get; set; }
        public Vector3? PitchYawRoll { get; set; }
        public Int3 BlockCoord { get; set; }
        public Vector3 Position { get; set; }
        public Vector3? PivotPosition { get; set; }
        public float? Scale { get; set; }
    }
}
