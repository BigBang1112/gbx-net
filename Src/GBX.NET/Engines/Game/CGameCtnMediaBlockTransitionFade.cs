using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game;

[Node(0x030AB000)]
public sealed class CGameCtnMediaBlockTransitionFade : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;
    private Vec3 color;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    [NodeMember]
    public Vec3 Color
    {
        get => color;
        set => color = value;
    }

    #endregion

    #region Constructors

    private CGameCtnMediaBlockTransitionFade()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x030AB000)]
    public class Chunk030AB000 : Chunk<CGameCtnMediaBlockTransitionFade>
    {
        public float U01;

        public override void ReadWrite(CGameCtnMediaBlockTransitionFade n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Opacity = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Opacity);
            });

            rw.Vec3(ref n.color);
            rw.Single(ref U01);
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Opacity { get; set; }
    }

    #endregion
}
