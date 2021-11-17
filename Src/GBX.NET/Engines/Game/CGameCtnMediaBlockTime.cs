namespace GBX.NET.Engines.Game;

[Node(0x03085000)]
public class CGameCtnMediaBlockTime : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys { get; set; }

    public bool IsTM2 { get; set; }

    #endregion

    #region Constructors

    protected CGameCtnMediaBlockTime()
    {
        Keys = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03085000)]
    public class Chunk03085000 : Chunk<CGameCtnMediaBlockTime>
    {
        public override void Read(CGameCtnMediaBlockTime n, GameBoxReader r)
        {
            if (!r.BaseStream.CanSeek)
                throw new NotSupportedException("Can't read CGameCtnMediaBlockTime in a stream that cannot seek.");

            var numKeys = r.ReadInt32();
            var bytes = r.ReadBytes(sizeof(float) * 2 * numKeys);

            if (r.PeekUInt32() == 0xFACADE01)
            {
                n.IsTM2 = true;

                r.BaseStream.Seek(-bytes.Length, SeekOrigin.Current);

                n.Keys = r.ReadList(numKeys, r1 => new Key()
                {
                    Time = r1.ReadSingle_s(),
                    TimeValue = r1.ReadSingle()
                });

                return;
            }

            n.IsTM2 = false;

            r.BaseStream.Seek(-bytes.Length, SeekOrigin.Current);

            n.Keys = r.ReadList(numKeys, r1 => new Key()
            {
                Time = r1.ReadSingle_s(),
                TimeValue = r1.ReadSingle(),
                Tangent = r1.ReadSingle()
            });
        }

        public override void Write(CGameCtnMediaBlockTime n, GameBoxWriter w)
        {
            if (n.IsTM2)
            {
                w.Write(n.Keys, (x, w1) =>
                {
                    w1.WriteSingle_s(x.Time);
                    w1.Write(x.TimeValue);
                });

                return;
            }

            w.Write(n.Keys, (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
                w1.Write(x.TimeValue);
                w1.Write(x.Tangent);
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public new class Key : CGameCtnMediaBlock.Key
    {
        public float TimeValue { get; set; }
        public float Tangent { get; set; }
    }

    #endregion
}
