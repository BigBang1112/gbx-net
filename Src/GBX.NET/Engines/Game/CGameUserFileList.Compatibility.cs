namespace GBX.NET.Engines.Game;

public partial class CGameUserFileList
{
    public partial class FileInfo
    {
        [Obsolete("Use RaceTime instead.")]
        public int? U04 { get => RaceTime?.TotalMilliseconds; set => RaceTime = value is null ? null : new TimeInt32(value.Value); }
    }
}
