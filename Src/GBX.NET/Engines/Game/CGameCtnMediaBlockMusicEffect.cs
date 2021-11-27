namespace GBX.NET.Engines.Game;

[Node(0x030A6000)]
public sealed class CGameCtnMediaBlockMusicEffect : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    private CGameCtnMediaBlockMusicEffect()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x030A6000)]
    public class Chunk030A6000 : Chunk<CGameCtnMediaBlockMusicEffect>
    {
        public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                MusicVolume = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.MusicVolume);
            });
        }
    }

    #endregion

    #region 0x001 chunk

    [Chunk(0x030A6001)]
    public class Chunk030A6001 : Chunk<CGameCtnMediaBlockMusicEffect>
    {
        public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GameBoxReaderWriter rw)
        {
            rw.List(ref n.keys!, r => new Key()
            {
                Time = r.ReadSingle_s(),
                MusicVolume = r.ReadSingle(),
                SoundVolume = r.ReadSingle()
            },
            (x, w) =>
            {
                w.WriteSingle_s(x.Time);
                w.Write(x.MusicVolume);
                w.Write(x.SoundVolume);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }
    }

    #endregion
}
