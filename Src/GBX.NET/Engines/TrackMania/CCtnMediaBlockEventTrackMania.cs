namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x2407F000</remarks>
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

    private TimeSingle start;
    private TimeSingle end = new(3);
    private Stunt[] stunts;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk2407F000))]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk2407F000))]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk2407F000))]
    public Stunt[] Stunts { get => stunts; set => stunts = value; }

    protected CCtnMediaBlockEventTrackMania()
    {
        stunts = Array.Empty<Stunt>();
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CCtnMediaBlockEventTrackMania 0x000 chunk
    /// </summary>
    [Chunk(0x2407F000)]
    public class Chunk2407F000 : Chunk<CCtnMediaBlockEventTrackMania>
    {
        public bool U01;

        public override void ReadWrite(CCtnMediaBlockEventTrackMania n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref U01);

            rw.Array(ref n.stunts!, (i, r) => new Stunt()
            {
                Time = r.ReadTimeSingle(),
                Figure = (EStuntFigure)r.ReadInt32(),
                Angle = r.ReadInt32(),
                Score = r.ReadInt32(),
                Factor = r.ReadSingle(),
                Straight = r.ReadBoolean(),
                U02 = r.ReadBoolean(),
                U03 = r.ReadBoolean(),
                U04 = r.ReadBoolean(),
                Combo = r.ReadInt32(),
                TotalScore = r.ReadInt32()
            },
            (x, w) =>
            {
                w.WriteTimeSingle(x.Time);
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

    /// <summary>
    /// CCtnMediaBlockEventTrackMania 0x003 chunk
    /// </summary>
    [Chunk(0x2407F003)]
    [AutoReadWriteChunk]
    public class Chunk2407F003 : Chunk<CCtnMediaBlockEventTrackMania>
    {

    }

    #endregion

    #endregion

    #region Other classes

    public class Stunt
    {
        public TimeSingle Time { get; set; }
        public EStuntFigure Figure { get; set; }
        public int Angle { get; set; }
        public int Score { get; set; }
        public float Factor { get; set; }
        public bool Straight { get; set; }
        public bool U02 { get; set; }
        public bool U03 { get; set; }
        public bool U04 { get; set; }
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
