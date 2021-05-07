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

                n.Stunts = rw.Array(n.Stunts, (i, r) => new Stunt()
                {
                    Time = TimeSpan.FromSeconds(r.ReadSingle()),
                    Figure = (EStuntFigure)r.ReadInt32(),
                    Angle = r.ReadInt32(),
                    Score = r.ReadInt32(),
                    Factor = r.ReadSingle(),
                    Straight = r.ReadBoolean(),
                    U02 = r.ReadInt32(),
                    U03 = r.ReadInt32(),
                    U04 = r.ReadInt32(),
                    Combo = r.ReadInt32(),
                    TotalScore = r.ReadInt32()
                },
                (x, w) =>
                {
                    w.Write((float)x.Time.TotalSeconds);
                    w.Write((int)x.Figure);
                    w.Write(x.Angle);
                    w.Write(x.Score);
                    w.Write(x.Factor);
                    w.Write(x.Straight);
                    w.Write(x.U02);
                    w.Write(x.U03);
                    w.Write(x.U04);
                    w.Write(x.Combo);
                    w.Write(x.TotalScore);
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
            public TimeSpan Time { get; set; }
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
