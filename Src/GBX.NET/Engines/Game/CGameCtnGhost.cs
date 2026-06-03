using GBX.NET.Inputs;
using System.Buffers.Binary;
using System.Collections.Immutable;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    [SupportsFormatting]
    [AppliedWithChunk<Chunk03092000>]
    [AppliedWithChunk<Chunk03092003>]
    [AppliedWithChunk<Chunk03092006>]
    [AppliedWithChunk<Chunk0309200D>]
    [AppliedWithChunk<Chunk03092015>]
    [AppliedWithChunk<Chunk03092017>]
    public string? GhostNickname { get; set; }

    [SupportsFormatting]
    [AppliedWithChunk<Chunk03092000>(sinceVersion: 8)]
    public string? GhostClubTag { get; set; }

    [AppliedWithChunk<Chunk0309200E>]
    public Id? GhostUid { get; set; }

    private TimeInt32 eventsDuration;
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public TimeInt32 EventsDuration { get => eventsDuration; set => eventsDuration = value; }

    private string? validate_ExeVersion;
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public string? Validate_ExeVersion { get => validate_ExeVersion; set => validate_ExeVersion = value; }

    private uint validate_ExeChecksum;
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public uint Validate_ExeChecksum { get => validate_ExeChecksum; set => validate_ExeChecksum = value; }

    private int validate_OsKind;
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public int Validate_OsKind { get => validate_OsKind; set => validate_OsKind = value; }

    private int validate_CpuKind;
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public int Validate_CpuKind { get => validate_CpuKind; set => validate_CpuKind = value; }

    private string? validate_RaceSettings;
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public string? Validate_RaceSettings { get => validate_RaceSettings; set => validate_RaceSettings = value; }

    private ImmutableArray<IInput> inputs = [];
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public ImmutableArray<IInput> Inputs { get => inputs; set => inputs = value; }

    private bool steeringWheelSensitivity;
    [AppliedWithChunk<Chunk03092025>]
    public bool SteeringWheelSensitivity { get => steeringWheelSensitivity; set => steeringWheelSensitivity = value; }

    private string? validate_TitleId;
    [AppliedWithChunk<Chunk03092028>]
    public string? Validate_TitleId { get => validate_TitleId; set => validate_TitleId = value; }

    private Checksum256? validate_TitleChecksum;
    [AppliedWithChunk<Chunk03092028>]
    public Checksum256? Validate_TitleChecksum { get => validate_TitleChecksum; set => validate_TitleChecksum = value; }

    private int? validate_ValidationSeed;
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public int? Validate_ValidationSeed { get => validate_ValidationSeed; set => validate_ValidationSeed = value; }

    public string GhostVersionString
    {
        get
        {
            if (ghostVersion >= 1 && ghostVersion <= 9998)
            {
                return $"TMr.{ghostVersion}";
            }

            if (ghostVersion >= 10000)
            {
                return $"VSKr.{ghostVersion - 9999}";
            }

            return "Unknown";
        }
    }

    /// <summary>
    /// Retrieves inputs from <see cref="Inputs"/> that are displayable. Currently it only makes a difference with Competition Patch 2 features.
    /// </summary>
    /// <returns>Displayable inputs as IEnumerable.</returns>
    public IEnumerable<IInput> GetDisplayableInputs()
    {
        uint version = 0;

        foreach (var input in inputs)
        {
            if (input is FakeIsRaceRunning fakeIsRaceRunning)
            {
                version = fakeIsRaceRunning.Data;

                // TM2 and TMT has _FakeIsRaceRunning 128 instead of 1
                if (version == 128)
                {
                    version = 1;
                }

                if (version > 3)
                {
                    throw new VersionNotSupportedException((int)version);
                }
            }

            if (version < 2)
            {
                yield return input;
                continue;
            }

            if (input is FakeFinishLine)
            {
                if (input.Time.TotalMilliseconds % 10 == 0)
                {
                    yield return input;
                    break;
                }
                continue;
            }

            if (input is SteerLeft or SteerRight)
            {
                if (input.Time.TotalMilliseconds % 10 is 1 or 2 or 3)
                {
                    var ceilingTime = TimeInt32.FromMilliseconds((input.Time.TotalMilliseconds + 9) / 10 * 10);

                    yield return input switch
                    {
                        SteerLeft steerLeft => steerLeft with { Time = ceilingTime },
                        SteerRight steerRight => steerRight with { Time = ceilingTime },
                        _ => throw new InvalidOperationException()
                    };
                }
                continue;
            }
            
            if (input is Steer steer)
            {
                if (input.Time.TotalMilliseconds % 10 is 2 or 3 or 9)
                {
                    yield return steer with { Time = new((input.Time.TotalMilliseconds + 9) / 10 * 10) };
                }
                continue;
            }

            yield return input;
        }
    }

    public partial class Chunk0309200E
    {
        public override void Read(CGameCtnGhost n, GbxReader r)
        {
            n.GhostUid = r.ReadId();
        }

        public override void Write(CGameCtnGhost n, GbxWriter w)
        {
            w.Write(n.GhostUid.GetValueOrDefault());
        }
    }

    public partial class Chunk03092011
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.TimeInt32(ref n.eventsDuration);

            if (n.eventsDuration != TimeInt32.Zero)
            {
                ReadWriteInputs(n, rw);
            }
        }

        internal void ReadWriteInputs(CGameCtnGhost n, GbxReaderWriter rw)
        {
            // CInputEventsStore::Archive
            rw.Int32(ref U01); // always 0 now

            if (rw.Reader is not null)
            {
                ReadInputs(n, rw.Reader);
            }

            if (rw.Writer is not null)
            {
                WriteInputs(n, rw.Writer);
            }
            //

            // SGameGhostValidationData
            rw.String(ref n.validate_ExeVersion);
            rw.UInt32(ref n.validate_ExeChecksum);
            rw.Int32(ref n.validate_OsKind);
            rw.Int32(ref n.validate_CpuKind);
            rw.String(ref n.validate_RaceSettings);
            //
        }

        private void ReadInputs(CGameCtnGhost n, GbxReader r)
        {
            Span<string> inputNames = r.ReadArrayId();

            var numInputs = r.ReadInt32();
            U02 = r.ReadInt32(); // CountLimit?

            if (numInputs == 0)
            {
                return;
            }

            Span<IInput> inputs = new IInput[numInputs];

            // 9 bytes per entry: 4 for time, 1 for name index, 4 for data
            var inputDataLength = numInputs * 9;

#if NET5_0_OR_GREATER
            Span<byte> inputData = inputDataLength > 8192 ? new byte[inputDataLength] : stackalloc byte[inputDataLength];
            r.BaseStream.ReadExactly(inputData);
#else
            Span<byte> inputData = r.ReadBytes(inputDataLength);
#endif

            for (var i = 0; i < numInputs; i++)
            {
                var time = TimeInt32.FromMilliseconds(BinaryPrimitives.ReadInt32LittleEndian(inputData.Slice(i * 9, 4)) - 100000);
                var inputNameIndex = inputData[i * 9 + 4];
                var data = BinaryPrimitives.ReadUInt32LittleEndian(inputData.Slice(i * 9 + 5, 4));

                var name = inputNames[inputNameIndex];

                inputs[i] = NET.Inputs.Input.Parse(time, name, data);
            }

            n.inputs = inputs.ToImmutableArray();
        }

        private void WriteInputs(CGameCtnGhost n, GbxWriter w)
        {
            var inputNames = n.inputs
                .Select(NET.Inputs.Input.GetName)
                .Distinct()
                .ToImmutableList() ?? ImmutableList<string>.Empty;

            w.WriteListId(inputNames);

            w.Write(n.inputs.Length);
            w.Write(U02);

            foreach (var input in n.inputs)
            {
                w.Write(input.Time.TotalMilliseconds + 100000);
                w.Write((byte)inputNames.IndexOf(NET.Inputs.Input.GetName(input)));
                w.Write(NET.Inputs.Input.GetData(input));
            }
        }
    }

    public partial class Chunk03092019
    {
        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            if (n.eventsDuration != TimeInt32.Zero)
            {
                rw.Int32(ref n.validate_ValidationSeed);
            }
        }
    }

    public partial class Chunk0309201A
    {
        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(n.checkpoints?.Length ?? 0);
        }
    }

    public partial class Chunk03092025 : IVersionable
    {
        public int Version { get; set; }

        private readonly Chunk03092019 chunk019 = new();

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version == 0)
            {
                rw.Chunk(n, chunk019);

                if (n.eventsDuration != TimeInt32.Zero)
                {
                    rw.Boolean(ref n.steeringWheelSensitivity);
                }
            }
            else
            {
                rw.TimeInt32(ref n.eventsDuration);

                chunk019.ReadWriteInputs(n, rw);

                rw.Int32(ref n.validate_ValidationSeed);
                rw.Boolean(ref n.steeringWheelSensitivity);
            }
        }
    }

    public partial class Chunk03092028
    {
        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            if (n.EventsDuration == TimeInt32.Zero)
            {
                return;
            }

            rw.String(ref n.validate_TitleId);
            rw.Checksum256(ref n.validate_TitleChecksum);
        }
    }

    public partial class Chunk0309202D
    {
        private readonly Chunk03092019 chunk019 = new();

        public int U01;
        public int U02; // same as 02A
        public int U03; // same as 02A
        public int U04;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);

            if (U01 >= 1)
            {
                throw new Exception("Inputs stored separately");
                //chunk019.ReadWriteInputs(n, rw);
            }

            rw.String(ref n.validate_ExeVersion);
            rw.UInt32(ref n.validate_ExeChecksum);
            rw.Int32(ref n.validate_OsKind);
            rw.Int32(ref n.validate_CpuKind);
            rw.UnixTime(ref n.walltimeStartTimestamp);
            rw.UnixTime(ref n.walltimeEndTimestamp);
            rw.String(ref n.validate_TitleId);
            rw.Checksum256(ref n.validate_TitleChecksum);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref n.validate_ValidationSeed);
            rw.Int32(ref U04);
            rw.String(ref n.validate_RaceSettings);
        }
    }

    public partial class Checkpoint
    {
        public override string ToString()
        {
            return $"{Time.ToTmString()} ({(Speed.HasValue ? $"{Speed}km/h, " : "")}{StuntsScore} pts.)";
        }
    }

    public override bool IsGameVersion(GameVersion version, bool strict = false)
    {
        if (!base.IsGameVersion(version, strict))
        {
            return false;
        }

        if (version == (GameVersion.MP4 | GameVersion.TM2020))
        {
            return Chunks.Any(static x => x is Chunk03092029);
        }

        return true;
    }
}
