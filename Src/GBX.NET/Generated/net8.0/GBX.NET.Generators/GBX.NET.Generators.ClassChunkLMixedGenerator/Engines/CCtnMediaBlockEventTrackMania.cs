namespace GBX.NET.Engines.TrackMania;

/// <remarks>ID: 0x2407F000</remarks>
[Class(0x2407F000)]
public partial class CCtnMediaBlockEventTrackMania : CGameCtnMediaBlock, IClass, CGameCtnMediaBlock.IHasTwoKeys
{
    [Hexadecimal] public static new uint Id => 0x2407F000;

    TimeSingle IHasTwoKeys.Start { get => Start; set => Start = value; }
    TimeSingle IHasTwoKeys.End { get => End; set => End = value; }



    private TimeSingle start;
    [AppliedWithChunk<Chunk2407F000>]
    [AppliedWithChunk<Chunk2407F003>]
    public TimeSingle Start { get => start; set => start = value; }

    private TimeSingle end;
    [AppliedWithChunk<Chunk2407F000>]
    [AppliedWithChunk<Chunk2407F003>]
    public TimeSingle End { get => end; set => end = value; }

    private Stunt[]? stunts;
    [AppliedWithChunk<Chunk2407F000>]
    public Stunt[]? Stunts { get => stunts; set => stunts = value; }

    private Event[]? events;
    [AppliedWithChunk<Chunk2407F003>]
    public Event[]? Events { get => events; set => events = value; }


    /// <summary>
    /// CCtnMediaBlockEventTrackMania 0x000 chunk
    /// </summary>
    [Chunk(0x2407F000)]
    public partial class Chunk2407F000 : Chunk<CCtnMediaBlockEventTrackMania>
    {
        /// <inheritdoc />
        public override uint Id => 0x2407F000;

        public bool U01;

        public override void ReadWrite(CCtnMediaBlockEventTrackMania n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref U01);
            rw.ArrayReadableWritable<Stunt>(ref n.stunts!);
        }
    }

    /// <summary>
    /// CCtnMediaBlockEventTrackMania 0x003 chunk
    /// </summary>
    [Chunk(0x2407F003)]
    public partial class Chunk2407F003 : Chunk<CCtnMediaBlockEventTrackMania>
    {
        /// <inheritdoc />
        public override uint Id => 0x2407F003;

        public bool U01;

        public override void ReadWrite(CCtnMediaBlockEventTrackMania n, GbxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref U01);
            rw.ArrayReadableWritable<Event>(ref n.events!);
        }
    }


    public sealed partial class Checkpoint : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            if (U01!=0)
            {
                return;
            }
            rw.TimeSingle(ref time);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
        }
    }

    public sealed partial class Stunt : IReadableWritable
    {

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private EStuntFigure figure;
        public EStuntFigure Figure { get => figure; set => figure = value; }

        private int angle;
        public int Angle { get => angle; set => angle = value; }

        private int score;
        public int Score { get => score; set => score = value; }

        private float factor;
        public float Factor { get => factor; set => factor = value; }

        private bool straight;
        public bool Straight { get => straight; set => straight = value; }

        private bool u01;
        public bool U01 { get => u01; set => u01 = value; }

        private bool u02;
        public bool U02 { get => u02; set => u02 = value; }

        private bool u03;
        public bool U03 { get => u03; set => u03 = value; }

        private int combo;
        public int Combo { get => combo; set => combo = value; }

        private int totalScore;
        public int TotalScore { get => totalScore; set => totalScore = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v == 0)
            {
                rw.TimeSingle(ref time);
            }
            rw.EnumInt32<EStuntFigure>(ref figure);
            rw.Int32(ref angle);
            rw.Int32(ref score);
            rw.Single(ref factor);
            rw.Boolean(ref straight);
            rw.Boolean(ref u01);
            rw.Boolean(ref u02);
            rw.Boolean(ref u03);
            rw.Int32(ref combo);
            rw.Int32(ref totalScore);
        }
    }

    public sealed partial class EndOfLap : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private int u03;
        public int U03 { get => u03; set => u03 = value; }

        private int u04;
        public int U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            if (U01>=2)
            {
                return;
            }
            rw.TimeSingle(ref time);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            if (v == 1)
            {
                rw.Int32(ref u04);
            }
        }
    }

    public sealed partial class Event : IKey, IReadableWritable
    {
    }

    public sealed partial class EndOfRace : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private TimeSingle time;
        public TimeSingle Time { get => time; set => time = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            if (U01!=0)
            {
                return;
            }
            rw.TimeSingle(ref time);
            rw.Int32(ref u02);
        }
    }


    public enum EventType
    {
        Stunt,
        Checkpoint,
        EndOfLap,
        EndOfRace,
    }

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
        Reset,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2407F000 => new Chunk2407F000(),
        0x2407F003 => new Chunk2407F003(),
        _ => base.NewChunk(chunkId),
    };
}
