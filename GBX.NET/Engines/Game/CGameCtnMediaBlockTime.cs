using System;
using System.IO;

namespace GBX.NET.Engines.Game
{
    [Node(0x03085000)]
    public class CGameCtnMediaBlockTime : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        public bool IsTM2 { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03085000)]
        public class Chunk03085000 : Chunk<CGameCtnMediaBlockTime>
        {
            public override void Read(CGameCtnMediaBlockTime n, GameBoxReader r)
            {
                if (!r.BaseStream.CanSeek)
                    throw new NotSupportedException("Can't read CGameCtnMediaBlockTime in a stream that can't seek.");

                var numKeys = r.ReadInt32();
                var bytes = r.ReadBytes(sizeof(float) * 2 * numKeys);

                if (r.PeekUInt32() == 0xFACADE01)
                {
                    n.IsTM2 = true;

                    r.BaseStream.Seek(-bytes.Length, SeekOrigin.Current);

                    n.Keys = r.ReadArray(numKeys, r1 => new Key()
                    {
                        Time = r1.ReadSingle(),
                        TimeValue = r1.ReadSingle()
                    });
                }
                else
                {
                    n.IsTM2 = false;

                    r.BaseStream.Seek(-bytes.Length, SeekOrigin.Current);

                    n.Keys = r.ReadArray(numKeys, r1 => new Key()
                    {
                        Time = r1.ReadSingle(),
                        TimeValue = r1.ReadSingle(),
                        Tangent = r1.ReadSingle()
                    });
                }
            }

            public override void Write(CGameCtnMediaBlockTime n, GameBoxWriter w)
            {
                if (n.IsTM2)
                {
                    w.Write(n.Keys, (x, w1) =>
                    {
                        w1.Write(x.Time);
                        w1.Write(x.TimeValue);
                    });
                }
                else
                {
                    w.Write(n.Keys, (x, w1) =>
                    {
                        w1.Write(x.Time);
                        w1.Write(x.TimeValue);
                        w1.Write(x.Tangent);
                    });
                }
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
}
