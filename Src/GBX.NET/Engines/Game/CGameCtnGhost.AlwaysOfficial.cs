using System.Globalization;

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
            HasGhostEnd = 1 << 3,
            HasOfficialStart = 1 << 4,
            HasOfficialEnd = 1 << 5,
        }

        private const Flags KnownFlags = Flags.CarriedAccelerate
            | Flags.CarriedBrake
            | Flags.HasRevision
            | Flags.HasGhostEnd
            | Flags.HasOfficialStart
            | Flags.HasOfficialEnd;

        private const int GitCommitSize = 20;

        /// <summary>
        /// Version of the mod that recorded the ghost. The revision is undefined when it was zero.
        /// </summary>
        public Version ModVersion { get; private set; } = new(0, 0, 0);

        /// <summary>
        /// Lowercase SHA-1 Git commit hash of the mod build, or null when the build didn't include one.
        /// </summary>
        public string? GitCommit { get; private set; }

        /// <summary>
        /// Accelerate was carried over from the previous run instead of being pressed again.
        /// </summary>
        public bool CarriedAccelerate { get; private set; }

        /// <summary>
        /// Brake was carried over from the previous run instead of being pressed again.
        /// </summary>
        public bool CarriedBrake { get; private set; }

        /// <summary>
        /// Real time when the ghost recording started.
        /// </summary>
        public DateTimeOffset GhostStartTimestamp { get; private set; }

        /// <summary>
        /// Real time when the run ended, if the ghost was saved after the end of the run.
        /// </summary>
        public DateTimeOffset? GhostEndTimestamp { get; private set; }

        /// <summary>
        /// Real time when official mode started (<c>StartOfficialRecord</c>).
        /// </summary>
        public DateTimeOffset? OfficialStartTimestamp { get; private set; }

        /// <summary>
        /// Real time when official mode stopped (<c>StopOfficialRecord</c>).
        /// </summary>
        public DateTimeOffset? OfficialEndTimestamp { get; private set; }

        /// <summary>
        /// Duration between <see cref="GhostStartTimestamp"/> and <see cref="GhostEndTimestamp"/>.
        /// </summary>
        public TimeSpan? GhostDuration => GhostEndTimestamp - GhostStartTimestamp;

        /// <summary>
        /// Duration between <see cref="OfficialStartTimestamp"/> and <see cref="OfficialEndTimestamp"/>.
        /// </summary>
        public TimeSpan? OfficialDuration => OfficialEndTimestamp - OfficialStartTimestamp;

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

            var gitCommit = r.ReadBytes(GitCommitSize);

            return new AlwaysOfficial
            {
                ModVersion = modVersion,
                GitCommit = gitCommit.All(static x => x == 0) ? null : ToHex(gitCommit),
                CarriedAccelerate = (flags & Flags.CarriedAccelerate) != 0,
                CarriedBrake = (flags & Flags.CarriedBrake) != 0,
                GhostStartTimestamp = ReadTimestamp(r),
                GhostEndTimestamp = (flags & Flags.HasGhostEnd) != 0 ? ReadTimestamp(r) : null,
                OfficialStartTimestamp = (flags & Flags.HasOfficialStart) != 0 ? ReadTimestamp(r) : null,
                OfficialEndTimestamp = (flags & Flags.HasOfficialEnd) != 0 ? ReadTimestamp(r) : null,
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
            if (GhostEndTimestamp.HasValue) flags |= Flags.HasGhostEnd;
            if (OfficialStartTimestamp.HasValue) flags |= Flags.HasOfficialStart;
            if (OfficialEndTimestamp.HasValue) flags |= Flags.HasOfficialEnd;

            w.Write((byte)flags);

            WriteVersionComponent(w, ModVersion.Major);
            WriteVersionComponent(w, ModVersion.Minor);
            WriteVersionComponent(w, Math.Max(ModVersion.Build, 0));

            if (revision != 0)
            {
                WriteVersionComponent(w, revision);
            }

            w.Write(GitCommit is null ? new byte[GitCommitSize] : FromHex(GitCommit));

            WriteTimestamp(w, GhostStartTimestamp);

            if (GhostEndTimestamp.HasValue)
            {
                WriteTimestamp(w, GhostEndTimestamp.Value);
            }

            if (OfficialStartTimestamp.HasValue)
            {
                WriteTimestamp(w, OfficialStartTimestamp.Value);
            }

            if (OfficialEndTimestamp.HasValue)
            {
                WriteTimestamp(w, OfficialEndTimestamp.Value);
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
            var value = ReadUnsigned(r);

            if (value > ushort.MaxValue)
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

            WriteUnsigned(w, (ulong)value);
        }

        private static DateTimeOffset ReadTimestamp(GbxReader r)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(checked((long)ReadUnsigned(r)));
        }

        private static void WriteTimestamp(GbxWriter w, DateTimeOffset value)
        {
            var milliseconds = value.ToUnixTimeMilliseconds();

            if (milliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Always Official timestamps cannot precede the Unix epoch.");
            }

            WriteUnsigned(w, (ulong)milliseconds);
        }

        // Unsigned LEB128, the same encoding as BinaryWriter.Write7BitEncodedInt64
        private static ulong ReadUnsigned(GbxReader r)
        {
            var value = 0UL;

            for (var shift = 0; shift < 64; shift += 7)
            {
                var b = r.ReadByte();
                value |= (ulong)(b & 0x7F) << shift;

                if ((b & 0x80) == 0)
                {
                    return value;
                }
            }

            throw new InvalidDataException("Always Official variable-length integer is too long.");
        }

        private static void WriteUnsigned(GbxWriter w, ulong value)
        {
            do
            {
                var b = (byte)(value & 0x7F);
                value >>= 7;
                w.Write((byte)(b | (value != 0 ? 0x80 : 0)));
            } while (value != 0);
        }

        private static string ToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(static x => x.ToString("x2")));
        }

        private static byte[] FromHex(string hex)
        {
            var bytes = new byte[GitCommitSize];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return bytes;
        }
    }
}
