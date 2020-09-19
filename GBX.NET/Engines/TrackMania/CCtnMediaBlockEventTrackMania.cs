using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.TrackMania
{
    [Node(0x2407F000)]
    public class CCtnMediaBlockEventTrackMania : CGameCtnMediaBlock
    {
        public enum EStuntFigure
        {
            None,
            StraightJump,
            Flip,
            BackFlip,
            Spin,
            Aerial,
            AlleyOop,
            Roll,
            Corkscrew,
            SpinOff,
            Rodeo,
            FlipFlap,
            Twister,
            FreeStyle,
            SpinningMix,
            FlippingChaos,
            RollingMadness,
            WreckNone,
            WreckStraightJump,
            WreckFlip,
            WreckBackFlip,
            WreckSpin,
            WreckAerial,
            WreckAlleyOop,
            WreckRoll,
            WreckCorkscrew,
            WreckSpinOff,
            WreckRodeo,
            WreckFlipFlap,
            WreckTwister,
            WreckFreeStyle,
            WreckSpinningMix,
            WreckFlippingChaos,
            WreckRollingMadness,
            TimePenalty,
            RespawnPenalty,
            Grind,
            Reset
        }

        public float Start { get; set; }
        public float End { get; set; }
        public Stunt[] Stunts { get; set; }

        [Chunk(0x2407F000)]
        public class Chunk2407F000 : Chunk<CCtnMediaBlockEventTrackMania>
        {
            public int U01 { get; set; }

            public override void ReadWrite(CCtnMediaBlockEventTrackMania n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start);
                n.End = rw.Single(n.End);
                U01 = rw.Int32(U01);

                n.Stunts = rw.Array(n.Stunts, i => new Stunt()
                {
                    Time = rw.Reader.ReadSingle(),
                    Figure = (EStuntFigure)rw.Reader.ReadInt32(),
                    Angle = rw.Reader.ReadInt32(),
                    Score = rw.Reader.ReadInt32(),
                    Factor = rw.Reader.ReadSingle(),
                    U01 = rw.Reader.ReadInt32(),
                    U02 = rw.Reader.ReadInt32(),
                    U03 = rw.Reader.ReadInt32(),
                    U04 = rw.Reader.ReadInt32(),
                    Combo = rw.Reader.ReadInt32(),
                    TotalScore = rw.Reader.ReadInt32()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write((int)x.Figure);
                    rw.Writer.Write(x.Angle);
                    rw.Writer.Write(x.Score);
                    rw.Writer.Write(x.Factor);
                    rw.Writer.Write(x.U01);
                    rw.Writer.Write(x.U02);
                    rw.Writer.Write(x.U03);
                    rw.Writer.Write(x.U04);
                    rw.Writer.Write(x.Combo);
                    rw.Writer.Write(x.TotalScore);
                });
            }
        }

        [Chunk(0x2407F003)]
        public class Chunk2407F003 : Chunk<CCtnMediaBlockEventTrackMania>
        {
            public override void Read(CCtnMediaBlockEventTrackMania n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadTillFacade();
            }
        }

        public class Stunt
        {
            public float Time { get; set; }
            public EStuntFigure Figure { get; set; }
            public int Angle { get; set; }
            public int Score { get; set; }
            public float Factor { get; set; }
            public int U01 { get; set; }
            public int U02 { get; set; }
            public int U03 { get; set; }
            public int U04 { get; set; }
            public int Combo { get; set; }
            public int TotalScore { get; set; }

            public override string ToString()
            {
                return $"[{Time}] {Figure} {Angle}° (+{Score} = {TotalScore})";
            }
        }
    }
}
