using GBX.NET.Builders.Engines.Control;

namespace GBX.NET.Engines.Control;

[Node(0x07010000)]
public class CControlEffectSimi : CControlEffect, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;
    private bool centered;
    private int colorBlendMode;
    private bool isContinousEffect;
    private bool isInterpolated;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> CGameCtnMediaBlock.IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>().ToList();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    [NodeMember]
    public bool Centered
    {
        get => centered;
        set => centered = value;
    }

    [NodeMember]
    public int ColorBlendMode
    {
        get => colorBlendMode;
        set => colorBlendMode = value;
    }

    [NodeMember]
    public bool IsContinousEffect
    {
        get => isContinousEffect;
        set => isContinousEffect = value;
    }

    [NodeMember]
    public bool IsInterpolated
    {
        get => isInterpolated;
        set => isInterpolated = value;
    }

    #endregion

    #region Constructors

    protected CControlEffectSimi()
    {
        keys = null!;
    }

    public static CControlEffectSimiBuilder Create()
    {
        return new CControlEffectSimiBuilder();
    }

    #endregion

    #region Chunks

    #region 0x002 chunk

    [Chunk(0x07010002)]
    public class Chunk07010002 : Chunk<CControlEffectSimi>
    {
        public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, (i, r) => new Key()
            {
                Time = r.ReadSingle_s(),
                Position = r.ReadVec2(),
                Rotation = r.ReadSingle(),
                Scale = r.ReadVec2(),
                Opacity = r.ReadSingle(),
                Depth = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Position);
                w.Write(x.Rotation);
                w.Write(x.Scale);
                w.Write(x.Opacity);
                w.Write(x.Depth);
            });

            rw.Boolean(ref n.centered);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CControlEffectSimi 0x004 chunk
    /// </summary>
    [Chunk(0x07010004)]
    public class Chunk07010004 : Chunk<CControlEffectSimi>
    {
        public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Position = r.ReadVec2(),
                Rotation = r.ReadSingle(),
                Scale = r.ReadVec2(),
                Opacity = r.ReadSingle(),
                Depth = r.ReadSingle(),
                U01 = r.ReadSingle(),
                IsContinuousEffect = r.ReadSingle(),
                U02 = r.ReadSingle(),
                U03 = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Position);
                w.Write(x.Rotation);
                w.Write(x.Scale);
                w.Write(x.Opacity);
                w.Write(x.Depth);
                w.Write(x.U01);
                w.Write(x.IsContinuousEffect);
                w.Write(x.U02);
                w.Write(x.U03);
            });

            rw.Boolean(ref n.centered);
            rw.Int32(ref n.colorBlendMode);
            rw.Boolean(ref n.isContinousEffect);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CControlEffectSimi 0x005 chunk
    /// </summary>
    [Chunk(0x07010005)]
    public class Chunk07010005 : Chunk<CControlEffectSimi>
    {
        public override void ReadWrite(CControlEffectSimi n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Position = r.ReadVec2(),
                Rotation = r.ReadSingle(),
                Scale = r.ReadVec2(),
                Opacity = r.ReadSingle(),
                Depth = r.ReadSingle(),
                U01 = r.ReadSingle(),
                IsContinuousEffect = r.ReadSingle(),
                U02 = r.ReadSingle(),
                U03 = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Position);
                w.Write(x.Rotation);
                w.Write(x.Scale);
                w.Write(x.Opacity);
                w.Write(x.Depth);
                w.Write(x.U01);
                w.Write(x.IsContinuousEffect);
                w.Write(x.U02);
                w.Write(x.U03);
            });

            rw.Boolean(ref n.centered);
            rw.Int32(ref n.colorBlendMode);
            rw.Boolean(ref n.isContinousEffect);
            rw.Boolean(ref n.isInterpolated);
        }
    }

    #endregion

    #endregion

    #region Other classes

    public class Key : CGameCtnMediaBlock.Key
    {
        public Vec2 Position { get; set; }
        /// <summary>
        /// Rotation in radians
        /// </summary>
        public float Rotation { get; set; }
        public Vec2 Scale { get; set; } = new(1, 1);
        public float Opacity { get; set; } = 1;
        public float Depth { get; set; } = 0.5f;
        public float IsContinuousEffect { get; set; }
        public float U01;
        public float U02;
        public float U03;
    }

    #endregion
}
