using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03084000)]
    public class CGameCtnMediaBlockCameraGame : CGameCtnMediaBlockCamera
    {
        public float? Start
        {
            get => GetValue<Chunk007>(x => x.Start) as float?;
            set => SetValue<Chunk007>(x => x.Start = value.GetValueOrDefault());
        }

        public float? End
        {
            get => GetValue<Chunk007>(x => x.End) as float?;
            set => SetValue<Chunk007>(x => x.End = value.GetValueOrDefault());
        }

        public CGameCtnMediaBlockCameraGame(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x03084000)]
        public class Chunk000_084 : Chunk
        {
            public float Start { get; set; }
            public float End { get; set; }

            public Chunk000_084(CGameCtnMediaBlockCamera node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Start = rw.Single(Start);
                End = rw.Single(End);
                rw.Int32(Unknown);
            }
        }

        [Chunk(0x03084001)]
        public class Chunk001 : Chunk
        {
            public enum GameCam : int
            {
                Behind,
                Close,
                Internal,
                Orbital
            }

            public float Start { get; set; }
            public float End { get; set; }
            public GameCam Type { get; set; }
            public int Target { get; set; }

            public Chunk001(CGameCtnMediaBlockCamera node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Start = rw.Single(Start);
                End = rw.Single(End);
                Type = (GameCam)rw.Int32((int)Type);
                Target = rw.Int32(Target);
            }
        }

        [Chunk(0x03084003)]
        public class Chunk003 : Chunk
        {
            public float Start { get; set; }
            public float End { get; set; }
            public string GameCam { get; set; }
            public int Target { get; set; }

            public Chunk003(CGameCtnMediaBlockCamera node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Start = rw.Single(Start);
                End = rw.Single(End);
                GameCam = rw.LookbackString(GameCam);
                Target = rw.Int32(Target);
            }
        }

        [Chunk(0x03084007)]
        public class Chunk007 : Chunk
        {
            public enum GameCam : int
            {
                Default,
                Internal,
                External,
                Helico,
                Free,
                Spectator,
                External_2
            }

            public GameCam Type { get; set; }
            public float Start { get; set; }
            public float End { get; set; }

            public Chunk007(CGameCtnMediaBlockCameraGame node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Type = (GameCam)rw.Int32((int)Type);
                Start = rw.Single(Start);
                End = rw.Single(End);

                // not right

                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Array<float>(Unknown, 6);
                rw.Array<float>(Unknown, 5);
                rw.Array<uint>(Unknown, 5);
            }
        }
    }
}
