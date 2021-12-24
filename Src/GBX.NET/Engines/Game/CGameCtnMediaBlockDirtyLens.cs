namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Dirty lens (0x03165000)
/// </summary>
[Node(0x03165000)]
public sealed class CGameCtnMediaBlockDirtyLens : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
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

    private CGameCtnMediaBlockDirtyLens()
    {
        keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03165000)]
    public class Chunk03165000 : Chunk<CGameCtnMediaBlockDirtyLens>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnMediaBlockDirtyLens n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.keys = r.ReadArray(r1 => new Key()
            {
                Time = r1.ReadSingle_s(),
                Intensity = r1.ReadSingle()
            });
        }

        public override void Write(CGameCtnMediaBlockDirtyLens n, GameBoxWriter w)
        {
            w.Write(Version);

            w.Write(n.keys, (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.Intensity);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float Intensity { get; set; }
    }

    #endregion
}
