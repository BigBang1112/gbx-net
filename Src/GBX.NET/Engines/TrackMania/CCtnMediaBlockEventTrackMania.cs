namespace GBX.NET.Engines.TrackMania;

public partial class CCtnMediaBlockEventTrackMania
{
    public partial class Stunt
    {
        public override string ToString()
        {
            return $"[{Time}] {(Combo > 1 ? Combo + "x " : "")}{(Combo > 0 ? "Chained " : "")}{Figure} " +
                $"{(Angle > 0 ? Angle + "° " : "")}({(Figure == EStuntFigure.TimePenalty ? "-" : "+")}{Score} = {TotalScore})";
        }
    }

    public partial class Checkpoint
    {
        public override string ToString()
        {
            return $"[{time}] Checkpoint";
        }
    }

    public partial class EndOfLap
    {
        public override string ToString()
        {
            return $"[{time}] End of lap";
        }
    }

    public partial class EndOfRace
    {
        public override string ToString()
        {
            return $"[{time}] End of race";
        }
    }

    public partial class Event : IReadableWritable
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

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
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
                    rw.ReadableWritable<Stunt>(ref stunt, version: 1);

                    if (rw.Reader is not null && stunt is not null) // ???
                    {
                        stunt.Time = time;
                    }

                    break;
                case EventType.Checkpoint:
                    rw.ReadableWritable<Checkpoint>(ref checkpoint);
                    break;
                case EventType.EndOfLap:
                    rw.ReadableWritable<EndOfLap>(ref endOfLap, version: version == 1 ? 0 : 1);
                    break;
                case EventType.EndOfRace:
                    rw.ReadableWritable<EndOfRace>(ref endOfRace);
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
}
