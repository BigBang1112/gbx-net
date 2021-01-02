using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030A7000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaBlockSound : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public FileRef Sound { get; set; }

        [NodeMember]
        public bool IsMusic { get; set; }

        [NodeMember]
        public bool IsLooping { get; set; }

        [NodeMember]
        public int PlayCount { get; set; }

        [NodeMember]
        public bool StopWithClip { get; set; }

        [NodeMember]
        public bool AudioToSpeech { get; set; }

        [NodeMember]
        public int AudioToSpeechTarget { get; set; }

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x030A7001)]
        public class Chunk030A7001 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                n.Sound = rw.FileRef(n.Sound);

                n.Keys = rw.Array(n.Keys, i =>
                {
                    return new Key()
                    {
                        Time = rw.Reader.ReadSingle(),
                        Volume = rw.Reader.ReadSingle(),
                        Pan = rw.Reader.ReadSingle()
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Volume);
                    rw.Writer.Write(x.Pan);
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
                n.PlayCount = rw.Int32(n.PlayCount);
                n.IsLooping = rw.Boolean(n.IsLooping);
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x030A7003)]
        public class Chunk030A7003 : Chunk<CGameCtnMediaBlockSound>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.PlayCount = rw.Int32(n.PlayCount);
                n.IsLooping = rw.Boolean(n.IsLooping);
                n.IsMusic = rw.Boolean(n.IsMusic);

                if(Version >= 1) // ManiaPlanet
                {
                    n.StopWithClip = rw.Boolean(n.StopWithClip);

                    if (Version >= 2)
                    {
                        n.AudioToSpeech = rw.Boolean(n.AudioToSpeech);
                        n.AudioToSpeechTarget = rw.Int32(n.AudioToSpeechTarget);
                    }
                }
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x030A7004)]
        public class Chunk030A7004 : Chunk<CGameCtnMediaBlockSound>
        {
            public override void ReadWrite(CGameCtnMediaBlockSound n, GameBoxReaderWriter rw)
            {
                n.Sound = rw.FileRef(n.Sound);
                rw.Int32(Unknown); // 1
                n.Keys = rw.Array(n.Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var volume = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadSingle();
                    var position = rw.Reader.ReadVec3();

                    return new Key()
                    {
                        Time = time,
                        Volume = volume,
                        Unknown = unknown,
                        Position = position
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Volume);
                    rw.Writer.Write(x.Unknown);
                    rw.Writer.Write(x.Position);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
        {
            public float Volume { get; set; }
            public float Pan { get; set; }
            public float Unknown { get; set; }
            public Vec3 Position { get; set; }
        }

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockSound node;

            public FileRef Sound => node.Sound;
            public bool IsMusic => node.IsMusic;
            public bool IsLooping => node.IsLooping;
            public int PlayCount => node.PlayCount;
            public bool StopWithClip => node.StopWithClip;
            public bool AudioToSpeech => node.AudioToSpeech;
            public int AudioToSpeechTarget => node.AudioToSpeechTarget;
            public Key[] Keys => node.Keys;

            public DebugView(CGameCtnMediaBlockSound node) => this.node = node;
        }

        #endregion
    }
}
