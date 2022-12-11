namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A0000</remarks>
[Node(0x030A0000)]
public class CGameCtnMediaBlockCameraOrbital : CGameCtnMediaBlock
{
    internal CGameCtnMediaBlockCameraOrbital()
    {

    }

    /// <summary>
    /// CGameCtnMediaBlockCameraOrbital 0x001 chunk
    /// </summary>
    [Chunk(0x030A0001)]
    [AutoReadWriteChunk]
    public class Chunk030A0001 : Chunk<CGameCtnMediaBlockCameraOrbital>
    {
        
    }
}
