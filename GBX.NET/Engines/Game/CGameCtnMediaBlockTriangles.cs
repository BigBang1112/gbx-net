using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03029000)]
    public class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock
    {
        [Chunk(0x03029001)]
        public class Chunk03029001 : Chunk<CGameCtnMediaBlockTriangles>
        {
            public override void Read(CGameCtnMediaBlockTriangles n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var keys = r.ReadArray(i => new Key() { Time = r.ReadSingle() });

                var numKeys = r.ReadInt32();
                var verticies = r.ReadArray(i => r.ReadVec3());

                for (var i = 1; i < numKeys; i++)
                    for (var j = 0; j < verticies.Length; j++)
                        r.ReadVec3();

                var colors = r.ReadArray(i => r.ReadVec4());

                r.ReadTillFacade();
            }
        }

        public class Key : MediaBlockKey
        {

        }
    }
}
