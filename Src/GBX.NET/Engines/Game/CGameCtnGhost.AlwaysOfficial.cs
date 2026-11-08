namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    /// <summary>
    /// Data written by the Always Official mod for TrackMania United Forever. Null when the ghost wasn't recorded with the mod.
    /// </summary>
    /// <remarks>Applied with: 0xBB111201 body chunk</remarks>
    [AppliedWithChunk<ChunkBB111201>]
    public AlwaysOfficial? AlwaysOfficialData { get; private set; }

    public partial class ChunkBB111201 : IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnGhost n, GbxReader r)
        {
            Version = r.ReadByte();

            if (Version != 0)
            {
                throw new VersionNotSupportedException(Version);
            }

            n.AlwaysOfficialData = AlwaysOfficial.Read(r);
        }

        public override void Write(CGameCtnGhost n, GbxWriter w)
        {
            var data = n.AlwaysOfficialData ?? throw new InvalidOperationException(
                $"{nameof(AlwaysOfficialData)} is null. Remove the 0xBB111201 chunk to write the ghost without Always Official data.");

            w.Write((byte)Version);
            data.Write(w);
        }
    }

    /// <summary>
    /// Ghost metadata stored by the Always Official mod.
    /// </summary>
    public sealed class AlwaysOfficial : IDeepCloneable
    {
        [Flags]
        private enum Flags : byte
        {
            CarriedAccelerate = 1 << 0,
            CarriedBrake = 1 << 1,
            HasRevision = 1 << 2,
            HasRaceFinish = 1 << 3,
            IsOfficial = 1 << 4,
        }

        private const Flags KnownFlags = Flags.CarriedAccelerate
            | Flags.CarriedBrake
            | Flags.HasRevision
            | Flags.HasRaceFinish
            | Flags.IsOfficial;

        private const int GitCommitSize = 20;

        /// <summary>
        /// Version of the mod that recorded the ghost. The revision is undefined when it was zero.
        /// </summary>
        public Version ModVersion { get; private set; } = new(0, 0, 0);

        /// <summary>
        /// Lowercase SHA-1 Git commit hash of the mod build, or null when the build didn't include one.
        /// </summary>
        public string? GitCommit => gitCommit.All(static x => x == 0) ? null : ToHex(gitCommit);

        // The raw bytes are written back as read
        private byte[] gitCommit = new byte[GitCommitSize];

        /// <summary>
        /// Accelerate was carried over from the previous run instead of being pressed again.
        /// </summary>
        public bool CarriedAccelerate { get; private set; }

        /// <summary>
        /// Brake was carried over from the previous run instead of being pressed again.
        /// </summary>
        public bool CarriedBrake { get; private set; }

        /// <summary>
        /// The run entered official mode (<c>StartOfficialRecord</c>).
        /// </summary>
        public bool IsOfficial { get; private set; }

        /// <summary>
        /// Real time when the local player's race started (the Go transition), or null when the race didn't start.
        /// </summary>
        public DateTimeOffset? RaceStartTimestamp { get; private set; }

        /// <summary>
        /// Real time when the local player's final finish was accepted, or null when the run didn't finish.
        /// </summary>
        public DateTimeOffset? RaceFinishTimestamp { get; private set; }

        /// <summary>
        /// Real time between <see cref="RaceStartTimestamp"/> and <see cref="RaceFinishTimestamp"/>.
        /// </summary>
        public TimeSpan? RaceDuration => RaceFinishTimestamp - RaceStartTimestamp;

        private AlwaysOfficial() { }

        internal static AlwaysOfficial Read(GbxReader r)
        {
            var flags = (Flags)r.ReadByte();

            if ((flags & ~KnownFlags) != 0)
            {
                throw new NotSupportedException($"Unknown Always Official flags: 0x{(byte)flags:X2}.");
            }

            var major = ReadVersionComponent(r);
            var minor = ReadVersionComponent(r);
            var build = ReadVersionComponent(r);

            var modVersion = (flags & Flags.HasRevision) != 0
                ? new Version(major, minor, build, ReadVersionComponent(r))
                : new Version(major, minor, build);

            return new AlwaysOfficial
            {
                ModVersion = modVersion,
                gitCommit = r.ReadBytes(GitCommitSize),
                CarriedAccelerate = (flags & Flags.CarriedAccelerate) != 0,
                CarriedBrake = (flags & Flags.CarriedBrake) != 0,
                IsOfficial = (flags & Flags.IsOfficial) != 0,
                RaceStartTimestamp = ReadTimestamp(r),
                RaceFinishTimestamp = (flags & Flags.HasRaceFinish) != 0 ? ReadTimestamp(r) : null,
            };
        }

        internal void Write(GbxWriter w)
        {
            // The mod writes the revision only when it is nonzero
            var revision = Math.Max(ModVersion.Revision, 0);

            var flags = default(Flags);
            if (CarriedAccelerate) flags |= Flags.CarriedAccelerate;
            if (CarriedBrake) flags |= Flags.CarriedBrake;
            if (revision != 0) flags |= Flags.HasRevision;
            if (RaceFinishTimestamp.HasValue) flags |= Flags.HasRaceFinish;
            if (IsOfficial) flags |= Flags.IsOfficial;

            w.Write((byte)flags);

            WriteVersionComponent(w, ModVersion.Major);
            WriteVersionComponent(w, ModVersion.Minor);
            WriteVersionComponent(w, Math.Max(ModVersion.Build, 0));

            if (revision != 0)
            {
                WriteVersionComponent(w, revision);
            }

            w.Write(gitCommit);

            WriteTimestamp(w, RaceStartTimestamp);

            if (RaceFinishTimestamp.HasValue)
            {
                WriteTimestamp(w, RaceFinishTimestamp);
            }
        }

        // Version and string are immutable, so a shallow copy is a deep copy
        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = (AlwaysOfficial)MemberwiseClone();
            context.Register(this, clone);
            return clone;
        }

        private static int ReadVersionComponent(GbxReader r)
        {
            var value = r.Read7BitEncodedInt64();

            if (value is < 0 or > ushort.MaxValue)
            {
                throw new InvalidDataException($"Always Official version component {value} is out of range.");
            }

            return (int)value;
        }

        private static void WriteVersionComponent(GbxWriter w, int value)
        {
            if ((uint)value > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Always Official version components must fit in 16 bits.");
            }

            w.Write7BitEncodedInt64(value);
        }

        // Zero stands for a missing timestamp
        private static DateTimeOffset? ReadTimestamp(GbxReader r)
        {
            var milliseconds = r.Read7BitEncodedInt64();
            return milliseconds == 0 ? null : DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
        }

        private static void WriteTimestamp(GbxWriter w, DateTimeOffset? value)
        {
            var milliseconds = value?.ToUnixTimeMilliseconds() ?? 0;

            if (milliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Always Official timestamps cannot precede the Unix epoch.");
            }

            w.Write7BitEncodedInt64(milliseconds);
        }

        private static string ToHex(byte[] bytes)
        {
#if NET9_0_OR_GREATER
            return Convert.ToHexStringLower(bytes);
#elif NET5_0_OR_GREATER
            return Convert.ToHexString(bytes).ToLowerInvariant();
#else
            return string.Concat(bytes.Select(static x => x.ToString("x2")));
#endif
        }
    }
}
