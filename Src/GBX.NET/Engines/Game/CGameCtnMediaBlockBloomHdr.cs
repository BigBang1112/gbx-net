using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Bloom HDR (0x03128000)
/// </summary>
[Node(0x03128000)]
public sealed class CGameCtnMediaBlockBloomHdr : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    private CGameCtnMediaBlockBloomHdr()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x001 chunk
    /// </summary>
    [Chunk(0x03128001)]
    public class Chunk03128001 : Chunk<CGameCtnMediaBlockBloomHdr>
    {
        public override void ReadWrite(CGameCtnMediaBlockBloomHdr n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Intensity = r.ReadSingle(),
                StreaksIntensity = r.ReadSingle(),
                StreaksAttenuation = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Intensity);
                w.Write(x.StreaksIntensity);
                w.Write(x.StreaksAttenuation);
            });
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaBlockBloomHdr 0x002 chunk
    /// </summary>
    [Chunk(0x03128002)]
    public class Chunk03128002 : Chunk<CGameCtnMediaBlockBloomHdr>
    {
        public override void ReadWrite(CGameCtnMediaBlockBloomHdr n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Intensity = r.ReadSingle(),
                StreaksIntensity = r.ReadSingle(),
                StreaksAttenuation = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Intensity);
                w.Write(x.StreaksIntensity);
                w.Write(x.StreaksAttenuation);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Intensity { get; set; }
        public float StreaksIntensity { get; set; }
        public float StreaksAttenuation { get; set; }
    }

    #endregion
}
