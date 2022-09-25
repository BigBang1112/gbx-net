namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Transition fade.
/// </summary>
/// <remarks>ID: 0x030AB000</remarks>
[Node(0x030AB000)]
[NodeExtension("CtnMediaBlockTransFade")]
public partial class CGameCtnMediaBlockTransitionFade : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;
    private Vec3 color;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys { get => keys; set => keys = value; }

    [NodeMember]
    public Vec3 Color { get => color; set => color = value; }

    internal CGameCtnMediaBlockTransitionFade()
    {
        keys = Array.Empty<Key>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockTransitionFade 0x000 chunk
    /// </summary>
    [Chunk(0x030AB000)]
    public class Chunk030AB000 : Chunk<CGameCtnMediaBlockTransitionFade>
    {
        public float U01;

        public override void ReadWrite(CGameCtnMediaBlockTransitionFade n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
            rw.Vec3(ref n.color);
            rw.Single(ref U01);
        }
    }

    #endregion
}
