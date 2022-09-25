namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker block - Time.
/// </summary>
/// <remarks>ID: 0x03085000</remarks>
[Node(0x03085000)]
public partial class CGameCtnMediaBlockTime : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => Keys.Cast<CGameCtnMediaBlock.Key>();
        set => Keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03085000))]
    public IList<Key> Keys { get; set; }

    [NodeMember]
    public bool IsTM2 { get; set; }

    internal CGameCtnMediaBlockTime()
    {
        Keys = Array.Empty<Key>();
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCtnMediaBlockTime 0x000 chunk
    /// </summary>
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

                n.Keys = r.ReadList(numKeys, r => new Key()
                {
                    Time = r.ReadTimeSingle(),
                    TimeValue = r.ReadSingle()
                });

                return;
            }

            n.IsTM2 = false;

            r.BaseStream.Seek(-bytes.Length, SeekOrigin.Current);

            n.Keys = r.ReadList(numKeys, r => new Key()
            {
                Time = r.ReadTimeSingle(),
                TimeValue = r.ReadSingle(),
                Tangent = r.ReadSingle()
            });
        }

        public override void Write(CGameCtnMediaBlockTime n, GameBoxWriter w)
        {
            if (n.IsTM2)
            {
                w.WriteList(n.Keys, (x, w) =>
                {
                    w.WriteTimeSingle(x.Time);
                    w.Write(x.TimeValue);
                });
            }
            else
            {
                w.WriteList(n.Keys, (x, w) =>
                {
                    w.WriteTimeSingle(x.Time);
                    w.Write(x.TimeValue);
                    w.Write(x.Tangent);
                });
            }
        }
    }

    #endregion
}
