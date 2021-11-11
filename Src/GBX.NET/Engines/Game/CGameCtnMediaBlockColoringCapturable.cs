using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Coloring capturable
/// </summary>
[Node(0x0316C000)]
public sealed class CGameCtnMediaBlockColoringCapturable : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    private CGameCtnMediaBlockColoringCapturable()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockColoringCapturable 0x000 chunk
    /// </summary>
    [Chunk(0x0316C000)]
    public class Chunk0316C000 : Chunk<CGameCtnMediaBlockColoringCapturable>, IVersionable
    {
        private int version;

        public int U01;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockColoringCapturable n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);

            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Hue = r.ReadSingle(),
                Gauge = r.ReadSingle(),
                Emblem = r.ReadInt32()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Hue);
                w.Write(x.Gauge);
                w.Write(x.Emblem);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Hue { get; set; }
        public float Gauge { get; set; }
        public int Emblem { get; set; }
    }

    #endregion
}
