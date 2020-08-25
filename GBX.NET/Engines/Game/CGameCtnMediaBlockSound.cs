using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A7000)]
    public class CGameCtnMediaBlockSound : CGameCtnMediaBlock
    {
        public FileRef Sound
        {
            get => GetValue<Chunk004>(x => x.Sound) as FileRef;
            set => SetValue<Chunk004>(x => x.Sound = value);
        }

        public Key[] Keys
        {
            get => GetValue<Chunk004>(x => x.Keys) as Key[];
            set => SetValue<Chunk004>(x => x.Keys = value);
        }

        public CGameCtnMediaBlockSound(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030A7001)]
        public class Chunk001 : Chunk
        {
            public FileRef Sound { get; set; }
            public Key[] Keys { get; set; }

            public Chunk001(CGameCtnMediaBlockSound node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Sound = rw.FileRef(Sound);

                Keys = rw.Array(Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(2);

                    return new Key()
                    {
                        Time = time,
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x030A7002)]
        public class Chunk002 : Chunk
        {
            public Chunk002(CGameCtnMediaBlockSound node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Array<int>(Unknown, 2);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x030A7003)]
        public class Chunk003 : Chunk
        {
            public Chunk003(CGameCtnMediaBlockSound node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Array<int>(Unknown, 7);
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x030A7004)]
        public class Chunk004 : Chunk
        {
            public FileRef Sound { get; set; }
            public Key[] Keys { get; set; }

            public Chunk004(CGameCtnMediaBlockSound node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Sound = rw.FileRef(Sound);
                rw.Int32(Unknown); // 1
                Keys = rw.Array(Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadArray<float>(5);

                    return new Key()
                    {
                        Time = time,
                        Unknown = unknown
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });
            }
        }

        #endregion

        #endregion

        public class Key
        {
            public float Time { get; set; }
            public float[] Unknown { get; set; }
        }
    }
}
