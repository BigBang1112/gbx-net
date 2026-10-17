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
        public TimeSingle Time { get => RaceTime; set => RaceTime = value; }

        public override string ToString()
        {
            return $"[{Time}] Checkpoint";
        }
    }

    public partial class EndOfLap
    {
        public TimeSingle Time { get => RaceTime; set => RaceTime = value; }

        public override string ToString()
        {
            return $"[{Time}] End of lap";
        }
    }

    public partial class EndOfRace
    {
        public TimeSingle Time { get => RaceTime; set => RaceTime = value; }

        public override string ToString()
        {
            return $"[{Time}] End of race";
        }
    }

    public partial class Event
    {
        private Stunt? eventStunt;

        public byte Version { get; set; }

        int IVersionable.Version
        {
            get => Version;
            set => Version = (byte)value;
        }

        public Stunt? Stunt
        {
            get => eventStunt;
            set
            {
                eventStunt = value;
                if (value is not null)
                {
                    value.Time = Time;
                }
            }
        }

        public override string ToString() => Type switch
        {
            EventType.Stunt => Stunt?.ToString() ?? "",
            EventType.Checkpoint => Checkpoint?.ToString() ?? "",
            EventType.EndOfLap => EndOfLap?.ToString() ?? "",
            EventType.EndOfRace => EndOfRace?.ToString() ?? "",
            _ => "Unknown event",
        };
    }
}
