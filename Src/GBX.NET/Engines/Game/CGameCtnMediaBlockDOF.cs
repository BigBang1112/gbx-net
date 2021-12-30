namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Depth of field (0x03126000)
/// </summary>
[Node(0x03126000)]
public class CGameCtnMediaBlockDOF : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    protected CGameCtnMediaBlockDOF()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03126000)]
    public class Chunk03126000 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                ZFocus = r.ReadSingle(),
                LensSize = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.ZFocus);
                w.Write(x.LensSize);
            });
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03126001)]
    public class Chunk03126001 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                ZFocus = r.ReadSingle(),
                LensSize = r.ReadSingle(),
                U01 = r.ReadInt32()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.ZFocus);
                w.Write(x.LensSize);
                w.Write(x.U01.GetValueOrDefault());
            });
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x03126002)]
    public class Chunk03126002 : Chunk<CGameCtnMediaBlockDOF>
    {
        public override void ReadWrite(CGameCtnMediaBlockDOF n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                ZFocus = r.ReadSingle(),
                LensSize = r.ReadSingle(),
                U01 = r.ReadInt32(),
                U02 = r.ReadSingle(),
                U03 = r.ReadSingle(),
                U04 = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.ZFocus);
                w.Write(x.LensSize);
                w.Write(x.U01.GetValueOrDefault());
                w.Write(x.U02.GetValueOrDefault());
                w.Write(x.U03.GetValueOrDefault());
                w.Write(x.U04.GetValueOrDefault());
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float ZFocus { get; set; }
        public float LensSize { get; set; }

        public int? U01;
        public float? U02;
        public float? U03;
        public float? U04;
    }

    #endregion
}
