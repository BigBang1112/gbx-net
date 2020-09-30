using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GBX.NET
{
    public class FreeBlock
    {
        public CGameCtnBlock Block { get; set; }
        public Vec3 Position { get; set; }
        public Vec3 PitchYawRoll { get; set; }
        public Vec3[] SnapPoints { get; set; }

        public FreeBlock(CGameCtnBlock block)
        {
            Block = block;
        }
    }
}
