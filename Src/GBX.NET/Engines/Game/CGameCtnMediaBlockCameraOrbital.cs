using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Game;

[Node(0x030A0000)]
public class CGameCtnMediaBlockCameraOrbital : CGameCtnMediaBlock
{
    private CGameCtnMediaBlockCameraOrbital()
    {

    }

    [Chunk(0x030A0001)]
    [AutoReadWriteChunk]
    public class Chunk030A0001 : Chunk<CGameCtnMediaBlockCameraOrbital>
    {
        
    }
}
