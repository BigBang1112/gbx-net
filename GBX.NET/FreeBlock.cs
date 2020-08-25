using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GBX.NET
{
    public class FreeBlock
    {
        public Block Block { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 PitchYawRoll { get; set; }
        public Vector3[] SnapPoints { get; set; }

        public FreeBlock(Block block)
        {
            Block = block;
        }
    }
}
