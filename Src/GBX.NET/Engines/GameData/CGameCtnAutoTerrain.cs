namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x03120000</remarks>
[Node(0x03120000)]
public class CGameCtnAutoTerrain : CMwNod
{
    private Int3 offset;
    private CGameCtnZoneGenealogy? genealogy;

    [NodeMember]
    public Int3 Offset { get => offset; set => offset = value; }

    [NodeMember(ExactlyNamed = true)]
    public CGameCtnZoneGenealogy? Genealogy { get => genealogy; set => genealogy = value; }

    protected CGameCtnAutoTerrain()
    {

    }

    /// <summary>
    /// CGameCtnAutoTerrain 0x001 chunk
    /// </summary>
    [Chunk(0x03120001)]
    public class Chunk03120001 : Chunk<CGameCtnAutoTerrain>
    {
        public override void ReadWrite(CGameCtnAutoTerrain n, GameBoxReaderWriter rw)
        {
            rw.Int3(ref n.offset);
            rw.NodeRef<CGameCtnZoneGenealogy>(ref n.genealogy);
        }
    }
}
