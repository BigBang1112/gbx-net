namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker Bloom block for TMUF and older games (0x03083000). This node causes "Couldn't load map" in ManiaPlanet.
/// </summary>
[Node(0x03083000)]
public sealed class CGameCtnMediaBlockFxBloom : CGameCtnMediaBlockFx, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Constructors

    private CGameCtnMediaBlockFxBloom()
    {
        keys = null!;
    }

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

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockFxBloom 0x001 chunk
    /// </summary>
    [Chunk(0x03083001)]
    public class Chunk03083001 : Chunk<CGameCtnMediaBlockFxBloom>
    {
        public override void ReadWrite(CGameCtnMediaBlockFxBloom n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r1 => new Key()
            {
                Time = r1.ReadSingle_s(),
                Intensity = r1.ReadSingle(),
                Sensitivity = r1.ReadSingle()
            },
            (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.Intensity);
                w1.Write(x.Sensitivity);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Intensity { get; set; }
        public float Sensitivity { get; set; }
    }

    #endregion
}
