namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x030A0000</remarks>
[Node(0x030A0000)]
public partial class CGameCtnMediaBlockCameraOrbital : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys = Array.Empty<Key>();

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk<Chunk030A0001>]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockCameraOrbital()
    {

    }

    /// <summary>
    /// CGameCtnMediaBlockCameraOrbital 0x001 chunk
    /// </summary>
    [Chunk(0x030A0001)]
    public class Chunk030A0001 : Chunk<CGameCtnMediaBlockCameraOrbital>, IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnMediaBlockCameraOrbital n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ListKey(ref n.keys!, Version);
        }
    }
}
