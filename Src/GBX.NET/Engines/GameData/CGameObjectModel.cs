namespace GBX.NET.Engines.GameData;

/// <summary>
/// CGameObjectModel (0x2E01D000)
/// </summary>
[Node(0x2E01D000)]
public class CGameObjectModel : CMwNod
{
#pragma warning disable IDE1006
    public uint m_InventoryParams_InventoryOccupation { get; set; }
#pragma warning restore IDE1006

    protected CGameObjectModel()
    {

    }

    [Chunk(0x2E01D000)]
    public class Chunk2E01D000 : Chunk<CGameObjectModel>
    {
        public override void ReadWrite(CGameObjectModel n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            n.m_InventoryParams_InventoryOccupation = rw.UInt32(n.m_InventoryParams_InventoryOccupation);
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }
}
