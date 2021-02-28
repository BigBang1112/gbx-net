using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.TrackMania
{
    [Node(0x2407F000)]
    public class CCtnMediaBlockEventTrackMania : CGameCtnMediaBlock
    {
        #region Enums

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

        #endregion

        #region Properties

        [NodeMember]
        public float Start { get; set; }

        [NodeMember]
        public float End { get; set; }

        [NodeMember]
        public Stunt[] Stunts { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

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
                    Straight = rw.Reader.ReadBoolean(),
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
                    rw.Writer.Write(x.Straight);
                    rw.Writer.Write(x.U02);
                    rw.Writer.Write(x.U03);
                    rw.Writer.Write(x.U04);
                    rw.Writer.Write(x.Combo);
                    rw.Writer.Write(x.TotalScore);
                });
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x2407F003)]
        public class Chunk2407F003 : Chunk<CCtnMediaBlockEventTrackMania>
        {
            public override void Read(CCtnMediaBlockEventTrackMania n, GameBoxReader r, GameBoxWriter unknownW)
            {
                r.ReadTillFacade();
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Stunt
        {
            public float Time { get; set; }
            public EStuntFigure Figure { get; set; }
            public int Angle { get; set; }
            public int Score { get; set; }
            public float Factor { get; set; }
            public bool Straight { get; set; }
            public int U02 { get; set; }
            public int U03 { get; set; }
            public int U04 { get; set; }
            public int Combo { get; set; }
            public int TotalScore { get; set; }

            public override string ToString()
            {
                return $"[{Time}] {(Combo > 1 ? Combo + "x " : "")}{(Combo > 0 ? "Chained " : "")}{Figure} " +
                    $"{(Angle > 0 ? Angle + "° " : "")}({(Figure == EStuntFigure.TimePenalty ? "-" : "+")}{Score} = {TotalScore})";
            }
        }

        #endregion
    }
}
