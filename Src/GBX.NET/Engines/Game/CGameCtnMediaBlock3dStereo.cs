namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - 3D stereo.
/// </summary>
/// <remarks>ID: 0x03024000</remarks>
[Node(0x03024000)]
[NodeExtension("GameCtnMediaBlock3dStereo")]
public partial class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03024000))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlock3dStereo()
    {
        keys = Array.Empty<Key>();
    }

    /// <summary>
    /// CGameCtnMediaBlock3dStereo 0x000 chunk
    /// </summary>
    [Chunk(0x03024000)]
    public class Chunk03024000 : Chunk<CGameCtnMediaBlock3dStereo>
    {
        public override void ReadWrite(CGameCtnMediaBlock3dStereo n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }
}
