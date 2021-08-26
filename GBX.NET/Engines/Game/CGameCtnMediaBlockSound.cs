using System.Collections.Generic;
using System.Linq;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A7000)]
    public class CGameCtnMediaBlockSound : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
    {
        #region Fields

        private FileRef sound;
        private bool isMusic;
        private bool isLooping;
        private int playCount = 1;
        private bool stopWithClip;
        private bool audioToSpeech;
        private int audioToSpeechTarget;
        private IList<Key> keys = new List<Key>();

        #endregion

        #region Properties

        IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
        {
            get => keys.Cast<CGameCtnMediaBlock.Key>();
            set => keys = value.Cast<Key>().ToList();
        }

        [NodeMember]
        public FileRef Sound
        {
            get => sound;
            set => sound = value;
        }

        [NodeMember]
        public bool IsMusic
        {
            get => isMusic;
            set => isMusic = value;
        }

        [NodeMember]
        public bool IsLooping
        {
            get => isLooping;
            set => isLooping = value;
        }

        [NodeMember]
        public int PlayCount
        {
            get => playCount;
            set => playCount = value;
        }

        [NodeMember]
        public bool StopWithClip
        {
            get => stopWithClip;
            set => stopWithClip = value;
        }

        [NodeMember]
        public bool AudioToSpeech
        {
            get => audioToSpeech;
            set => audioToSpeech = value;
        }

        [NodeMember]
        public int AudioToSpeechTarget
        {
            get => audioToSpeechTarget;
            set => audioToSpeechTarget = value;
        }

        [NodeMember]
        public IList<Key> Keys
        {
            get => keys;
            set => keys = value;
        }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030A7001)]
        public class Chunk030A7001 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                rw.FileRef(ref n.sound);

                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    Volume = r.ReadSingle(),
                    Pan = r.ReadSingle()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.Volume);
                    w.Write(x.Pan);
                });
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x030A7002)]
        public class Chunk030A7002 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref n.playCount);
                rw.Boolean(ref n.isLooping);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x030A7003)]
        public class Chunk030A7003 : Chunk<CGameCtnMediaBlockSound>, IVersionable
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Int32(ref n.playCount);
                rw.Boolean(ref n.isLooping);
                rw.Boolean(ref n.isMusic);

                if (version >= 1) // ManiaPlanet
                {
                    rw.Boolean(ref n.stopWithClip);

                    if (version >= 2)
                    {
                        rw.Boolean(ref n.audioToSpeech);
                        rw.Int32(ref n.audioToSpeechTarget);
                    }
                }
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x030A7004)]
        public class Chunk030A7004 : Chunk<CGameCtnMediaBlockSound>
        {
            public int U01 = 1;

            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                rw.FileRef(ref n.sound);
                rw.Int32(ref U01); // 1

                rw.List(ref n.keys, r => new Key()
                {
                    Time = r.ReadSingle_s(),
                    Volume = r.ReadSingle(),
                    U01 = r.ReadSingle(),
                    Position = r.ReadVec3()
                },
                (x, w) =>
                {
                    w.WriteSingle_s(x.Time);
                    w.Write(x.Volume);
                    w.Write(x.U01);
                    w.Write(x.Position);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Volume { get; set; }
            public float Pan { get; set; }
            public float U01 { get; set; }
            public Vec3 Position { get; set; }
        }

        #endregion
    }
}
