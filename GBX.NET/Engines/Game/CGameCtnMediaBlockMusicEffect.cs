using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A6000)]
    public class CGameCtnMediaBlockMusicEffect : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x030A6000)]
        public class Chunk030A6000 : Chunk<CGameCtnMediaBlockMusicEffect>
        {
            public override void ReadWrite(CGameCtnMediaBlockMusicEffect n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var musicVol = rw.Reader.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        MusicVolume = musicVol
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.MusicVolume);
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
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var musicVol = rw.Reader.ReadSingle();
                    var soundVol = rw.Reader.ReadSingle();

                    return new Key()
                    {
                        Time = time,
                        MusicVolume = musicVol,
                        SoundVolume = soundVol
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.MusicVolume);
                    rw.Writer.Write(x.SoundVolume);
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
}
