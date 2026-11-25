namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_ManiaPlanetStations
{
    public partial class OldGameStationDesc
    {
        [Obsolete("Use MasterServerStationId instead.")]
        public int U01 { get => MasterServerStationId; set => MasterServerStationId = value; }

        [Obsolete("Use LegacySizeCode instead.")]
        public int U02 { get => LegacySizeCode; set => LegacySizeCode = value; }

        [Obsolete("Use TitleId instead.")]
        public string? U03 { get => TitleId; set => TitleId = value; }

        [Obsolete("Use X instead.")]
        public int U05 { get => X; set => X = value; }

        [Obsolete("Use Y instead.")]
        public int U06 { get => Y; set => Y = value; }

        [Obsolete("Use Size instead.")]
        public int U07 { get => Size; set => Size = value; }
    }

    public sealed partial class GameStationDesc
    {
        [Obsolete("Use MasterServerStationId instead.")]
        public int U01 { get => MasterServerStationId; set => MasterServerStationId = value; }

        [Obsolete("Use StationType instead.")]
        public int U02 { get => StationType; set => StationType = value; }

        [Obsolete("Use PackChecksum instead.")]
        public byte[]? Checksum { get => PackChecksum; set => PackChecksum = value; }

        [Obsolete("Use X instead.")]
        public int U04 { get => X; set => X = value; }

        [Obsolete("Use Y instead.")]
        public int U05 { get => Y; set => Y = value; }

        [Obsolete("Use Size instead.")]
        public int U06 { get => Size; set => Size = value; }

        [Obsolete("Use AutoLayoutTimestamp instead.")]
        public int? U07 { get => AutoLayoutTimestamp; set => AutoLayoutTimestamp = value; }

        [Obsolete("Use Order instead.")]
        public int? U08 { get => Order; set => Order = value; }

        [Obsolete("Use LoadWithParentalLock instead.")]
        public bool? U09 { get => LoadWithParentalLock; set => LoadWithParentalLock = value; }
    }

    public partial class DisplayInfo
    {
        [Obsolete("Use MasterServerStationId instead.")]
        public int U01 { get => MasterServerStationId; set => MasterServerStationId = value; }

        [Obsolete("Use X instead.")]
        public int U02 { get => X; set => X = value; }

        [Obsolete("Use Y instead.")]
        public int U03 { get => Y; set => Y = value; }

        [Obsolete("Use Size instead.")]
        public int U04 { get => Size; set => Size = value; }
    }
}
