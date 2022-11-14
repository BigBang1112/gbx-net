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
    private Event[] events;

    [NodeMember]
    [AppliedWithChunk<Chunk2407F000>]
    public TimeSingle Start { get => start; set => start = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk2407F000>]
    public TimeSingle End { get => end; set => end = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk2407F000>]
    public Stunt[] Stunts { get => stunts; set => stunts = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk2407F003>]
    public Event[] Events { get => events; set => events = value; }

    internal CCtnMediaBlockEventTrackMania()
    {
        stunts = Array.Empty<Stunt>();
        events = Array.Empty<Event>();
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
            
            rw.ArrayArchive<Stunt>(ref n.stunts!);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CCtnMediaBlockEventTrackMania 0x003 chunk
    /// </summary>
    [Chunk(0x2407F003)]
    public class Chunk2407F003 : Chunk<CCtnMediaBlockEventTrackMania>
    {
        public bool U01;
        
        public override void ReadWrite(CCtnMediaBlockEventTrackMania n, GameBoxReaderWriter rw)
        {
            rw.TimeSingle(ref n.start);
            rw.TimeSingle(ref n.end);
            rw.Boolean(ref U01);
            
            rw.ArrayArchive<Event>(ref n.events!);
        }
    }

    #endregion

    #endregion

    #region Other classes

    public class Stunt : IReadableWritable
    {
        private TimeSingle time;
        private EStuntFigure figure;
        private int angle;
        private int score;
        private float factor;
        private bool straight;
        private bool u02;
        private bool u03;
        private bool u04;
        private int combo;
        private int totalScore;

        public TimeSingle Time { get => time; set => time = value; }
        public EStuntFigure Figure { get => figure; set => figure = value; }
        public int Angle { get => angle; set => angle = value; }
        public int Score { get => score; set => score = value; }
        public float Factor { get => factor; set => factor = value; }
        public bool Straight { get => straight; set => straight = value; }
        public bool U02 { get => u02; set => u02 = value; }
        public bool U03 { get => u03; set => u03 = value; }
        public bool U04 { get => u04; set => u04 = value; }
        public int Combo { get => combo; set => combo = value; }
        public int TotalScore { get => totalScore; set => totalScore = value; }

        public override string ToString()
        {
            return $"[{Time}] {(Combo > 1 ? Combo + "x " : "")}{(Combo > 0 ? "Chained " : "")}{Figure} " +
                $"{(Angle > 0 ? Angle + "° " : "")}({(Figure == EStuntFigure.TimePenalty ? "-" : "+")}{Score} = {TotalScore})";
        }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            if (version == 0) // More of a hack
            {
                rw.TimeSingle(ref time);
            }
            
            rw.EnumInt32<EStuntFigure>(ref figure);
            rw.Int32(ref angle);
            rw.Int32(ref score);
            rw.Single(ref factor);
            rw.Boolean(ref straight);
            rw.Boolean(ref u02);
            rw.Boolean(ref u03);
            rw.Boolean(ref u04);
            rw.Int32(ref combo);
            rw.Int32(ref totalScore);
        }
    }

    public class Checkpoint : IReadableWritable
    {
        private int u01;
        private TimeInt32 time;
        private int u02;
        private int u03;
        private int u04;

        public int U01 { get => u01; set => u01 = value; }
        public TimeInt32 Time { get => time; set => time = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u01; set => u01 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);

            if (u01 != 0)
            {
                return;
            }
            
            rw.TimeInt32(ref time);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
        }

        public override string ToString()
        {
            return $"[{time}] Checkpoint";
        }
    }

    public class EndOfLap : IReadableWritable
    {
        private int u01;
        private TimeInt32 time;
        private int u02;
        private int u03;
        private int u04;

        public int U01 { get => u01; set => u01 = value; }
        public TimeInt32 Time { get => time; set => time = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);

            if (u01 >= 2)
            {
                return;
            }

            rw.TimeInt32(ref time);
            rw.Int32(ref u02);
            rw.Int32(ref u03);

            if (version == 1)
            {
                rw.Int32(ref u04);
            }
        }

        public override string ToString()
        {
            return $"[{time}] End of lap";
        }
    }

    public class EndOfRace : IReadableWritable
    {
        private int u01;
        private TimeInt32 time;
        private int u02;

        public int U01 { get => u01; set => u01 = value; }
        public TimeInt32 Time { get => time; set => time = value; }
        public int U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);

            if (u01 != 0)
            {
                return;
            }

            rw.TimeInt32(ref time);
            rw.Int32(ref u02);
        }

        public override string ToString()
        {
            return $"[{time}] End of race";
        }
    }

    public enum EventType
    {
        Stunt,
        Checkpoint,
        EndOfLap,
        EndOfRace
    }

    public class Event : IReadableWritable
    {
        private byte version;
        private int? u01;
        private byte? u02;
        private EventType type;
        private TimeSingle time;
        private Stunt? stunt;
        private Checkpoint? checkpoint;
        private EndOfLap? endOfLap;
        private EndOfRace? endOfRace;

        public TimeSingle Time { get => time; set => time = value; }
        public byte Version { get => version; set => version = value; }
        public int? U01 { get => u01; set => u01 = value; }
        public byte? U02 { get => u02; set => u02 = value; }
        public EventType Type { get => type; set => type = value; }
        public Stunt? Stunt { get => stunt; set => stunt = value; }
        public Checkpoint? Checkpoint { get => checkpoint; set => checkpoint = value; }
        public EndOfLap? EndOfLap { get => endOfLap; set => endOfLap = value; }
        public EndOfRace? EndOfRace { get => endOfRace; set => endOfRace = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.TimeSingle(ref time);
            rw.Byte(ref version);

            if (version == 0)
            {
                throw new VersionNotSupportedException(version);
            }
            
            if (version < 3)
            {
                rw.Int32(ref u01);
            }
            else
            {
                rw.Byte(ref u02);
            }

            rw.EnumInt32<EventType>(ref type);

            switch (type)
            {
                case EventType.Stunt:
                    rw.Archive<Stunt>(ref stunt, version: 1);

                    if (rw.Reader is not null && stunt is not null) // ???
                    {
                        stunt.Time = time;
                    }

                    break;
                case EventType.Checkpoint:
                    rw.Archive<Checkpoint>(ref checkpoint);
                    break;
                case EventType.EndOfLap:
                    rw.Archive<EndOfLap>(ref endOfLap, version: version == 1 ? 0 : 1);
                    break;
                case EventType.EndOfRace:
                    rw.Archive<EndOfRace>(ref endOfRace);
                    break;
            }
        }

        public override string ToString() => type switch
        {
            EventType.Stunt => stunt?.ToString() ?? "",
            EventType.Checkpoint => checkpoint?.ToString() ?? "",
            EventType.EndOfLap => endOfLap?.ToString() ?? "",
            EventType.EndOfRace => endOfRace?.ToString() ?? "",
            _ => "Unknown event",
        };
    }

    #endregion
}
