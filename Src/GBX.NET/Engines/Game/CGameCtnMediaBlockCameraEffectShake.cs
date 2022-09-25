namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera shake.
/// </summary>
/// <remarks>ID: 0x030A4000</remarks>
[Node(0x030A4000)]
[NodeExtension("CtnMediaBlockCamFxShake")]
public partial class CGameCtnMediaBlockCameraEffectShake : CGameCtnMediaBlockCameraEffect, CGameCtnMediaBlock.IHasKeys
{
    private IList<Key> keys;

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk030A4000))]
    public IList<Key> Keys { get => keys; set => keys = value; }

    internal CGameCtnMediaBlockCameraEffectShake()
    {
        keys = Array.Empty<Key>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraEffectShake 0x000 chunk
    /// </summary>
    [Chunk(0x030A4000)]
    public class Chunk030A4000 : Chunk<CGameCtnMediaBlockCameraEffectShake>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraEffectShake n, GameBoxReaderWriter rw)
        {
            rw.ListKey(ref n.keys!);
        }
    }

    #endregion
}
