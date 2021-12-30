namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - 3D stereo (0x03024000)
/// </summary>
[Node(0x03024000)]
public class CGameCtnMediaBlock3dStereo : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlock3dStereo()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlock3dStereo 0x000 chunk
    /// </summary>
    [Chunk(0x03024000)]
    public class Chunk03024000 : Chunk<CGameCtnMediaBlock3dStereo>
    {
        public override void ReadWrite(CGameCtnMediaBlock3dStereo n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                UpToMax = r.ReadSingle(),
                ScreenDist = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.UpToMax);
                w.Write(x.ScreenDist);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float UpToMax { get; set; }
        public float ScreenDist { get; set; }
    }

    #endregion
}
