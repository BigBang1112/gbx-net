using TmEssentials;

namespace GBX.NET.Engines.Game;

public partial class CGameUserFileList
{
    public partial class FileInfo
    {
        [Obsolete("Use FileWriteTime instead.")]
        public ulong U02
        {
            get => (ulong)(FileWriteTime?.ToFileTimeUtc() ?? 0);
            set => FileWriteTime = value == 0 ? null : DateTime.FromFileTime((long)value);
        }

        [Obsolete("Use FileSize instead.")]
        public ulong U03 { get => FileSize; set => FileSize = value; }

        [Obsolete("Use RaceTime instead.")]
        public int? U06
        {
            get => RaceTime?.TotalMilliseconds;
            set => RaceTime = value is null ? null : new TimeInt32(value.Value);
        }

        [Obsolete("Use RecordingContext instead.")]
        public string? GhostKind { get => RecordingContext; set => RecordingContext = value; }

        public override string ToString()
        {
            return Name ?? "[unknown file]";
        }
    }
}
