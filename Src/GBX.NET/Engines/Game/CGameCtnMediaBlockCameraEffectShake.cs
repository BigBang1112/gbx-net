using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Camera shake
/// </summary>
[Node(0x030A4000)]
public sealed class CGameCtnMediaBlockCameraEffectShake : CGameCtnMediaBlockCameraEffect, CGameCtnMediaBlock.IHasKeys
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

    private CGameCtnMediaBlockCameraEffectShake()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockCameraEffectShake 0x000 chunk
    /// </summary>
    [Chunk(0x030A4000)]
    public class Chunk030A4000 : Chunk<CGameCtnMediaBlockCameraEffectShake>
    {
        public override void ReadWrite(CGameCtnMediaBlockCameraEffectShake n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Intensity = r.ReadSingle(),
                Speed = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Intensity);
                w.Write(x.Speed);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Intensity { get; set; }
        public float Speed { get; set; }
    }

    #endregion
}
