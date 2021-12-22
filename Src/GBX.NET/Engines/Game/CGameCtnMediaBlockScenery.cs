namespace GBX.NET.Engines.Game;

[Node(0x03188000)]
public class CGameCtnMediaBlockScenery : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;
    private CPlugDataTape? dataTape;

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    public CPlugDataTape? DataTape
    {
        get => dataTape;
        set => dataTape = value;
    }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockScenery()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03188000)]
    public class Chunk03188000 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        private int version;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                U01 = r.ReadSingle(),
                U02 = r.ReadSingle(),
                U03 = r.ReadSingle(),
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.U01);
                w.Write(x.U02);
                w.Write(x.U03);
            });
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x03188001)]
    public class Chunk03188001 : Chunk<CGameCtnMediaBlockScenery>, IVersionable
    {
        private int version;
        private int U01;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCtnMediaBlockScenery n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Int32(ref U01);
            rw.NodeRef<CPlugDataTape>(ref n.dataTape);
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float U01 { get; set; }
        public float U02 { get; set; }
        public float U03 { get; set; }
        public Vec3 Position { get; set; }
    }

    #endregion
}
