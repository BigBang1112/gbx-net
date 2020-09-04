using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A6000)]
    public class CGameCtnMediaBlockMusicEffect : CGameCtnMediaBlock
    {
        public Key[] Keys
        {
            get => GetValue<Chunk001>(x => x.Keys) as Key[];
            set => SetValue<Chunk001>(x => x.Keys = value);
        }

        public CGameCtnMediaBlockMusicEffect(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030A6000)]
        public class Chunk000 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk000(CGameCtnMediaBlockMusicEffect node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
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

        [Chunk(0x030A6001)]
        public class Chunk001 : Chunk
        {
            public Key[] Keys { get; set; }

            public Chunk001(CGameCtnMediaBlockMusicEffect node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Keys = rw.Array(Keys, i =>
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

        public class Key : MediaBlockKey
        {
            public float MusicVolume { get; set; }
            public float SoundVolume { get; set; }
        }
    }
}
