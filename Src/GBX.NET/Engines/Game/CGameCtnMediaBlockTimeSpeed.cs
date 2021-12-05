namespace GBX.NET.Engines.Game;

[Node(0x03129000)]
public sealed class CGameCtnMediaBlockTimeSpeed : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

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

    #endregion

    #region Constructors

    private CGameCtnMediaBlockTimeSpeed()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    [Chunk(0x03085000)]
    public class Chunk03085000 : Chunk<CGameCtnMediaBlockTimeSpeed>
    {
        public override void ReadWrite(CGameCtnMediaBlockTimeSpeed n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Speed = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Speed);
            });
        }
    }

    [Chunk(0x03129000)]
    public class Chunk03129000 : Chunk<CGameCtnMediaBlockTimeSpeed>
    {
        public override void ReadWrite(CGameCtnMediaBlockTimeSpeed n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                Speed = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.Speed);
            });
        }
    }

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Speed { get; set; }
    }

    #endregion
}
