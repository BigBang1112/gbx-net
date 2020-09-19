using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03080000)]
    public class CGameCtnMediaBlockFxColors : CGameCtnMediaBlockFx
    {
        [Chunk(0x03080003)]
        public class Chunk03080003 : Chunk<CGameCtnMediaBlockFxColors>
        {
            public override void Read(CGameCtnMediaBlockFxColors n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var numKeys = r.ReadInt32();
                r.ReadInt32();
                var intensity = r.ReadSingle();
                r.ReadInt32();
                var distance = r.ReadSingle();
                var farDistance = r.ReadSingle();
                var inverse = r.ReadSingle();
                var hue = r.ReadSingle();
                var saturation = r.ReadSingle(); // from center
                var brightness = r.ReadSingle(); // from center
                var contrast = r.ReadSingle(); // from center
                var rgb = r.ReadVec3();

                r.ReadTillFacade();
            }
        }
    }
}
