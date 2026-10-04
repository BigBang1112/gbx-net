using GBX.NET.Inputs;
using System.Buffers.Binary;
using System.Collections.Immutable;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    private Id? ghostUid;
    public partial Id? GhostUid { get => ghostUid; set => ghostUid = value; }

    private ImmutableArray<IInput> inputs = [];
    [AppliedWithChunk<Chunk03092011>]
    [AppliedWithChunk<Chunk03092019>]
    [AppliedWithChunk<Chunk03092025>]
    public ImmutableArray<IInput> Inputs { get => inputs; set => inputs = value; }

    [Obsolete("Use GhostCountryPath instead.")]
    public string? GhostZone { get => GhostCountryPath; set => GhostCountryPath = value; }

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
        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            if (rw.Reader is not null)
            {
                n.GhostUid = rw.Reader.ReadId();
            }

            rw.Writer?.Write(n.GhostUid.GetValueOrDefault());
        }
    }

    public partial class Chunk03092011
    {
        public int InputStoreVersion;
        public int InputCountLimit;

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
            rw.Int32(ref InputStoreVersion); // always 0 now

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
            InputCountLimit = r.ReadInt32();

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
            w.Write(InputCountLimit);

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

    public partial class Chunk0309201B : IVersionable
    {
        public int Version { get; set; } = 2;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version is < 0 or > 2)
            {
                throw new VersionNotSupportedException(Version);
            }

            var count = 0;
            if (Version == 2)
            {
                count = ReadWriteCheckpointCount(n, rw);
            }
            else
            {
                rw.TimeInt32Nullable(ref n.raceTime);
            }

            if (Version == 0)
            {
                rw.Int32(ref n.stuntScore);
                rw.Int32(ref n.respawns, defaultValue: -1);
            }
            else
            {
                // Earlier chunks can contain -1; do not convert their values while reading.
                var score = rw.Writer is null ? (ushort)0
                    : n.stuntScore == -1 ? ushort.MaxValue : checked((ushort)(n.stuntScore ?? 0));
                n.stuntScore = rw.UInt16(score);
                var storedRespawns = rw.Writer is null || n.respawns is null or -1
                    ? ushort.MaxValue : checked((ushort)n.respawns.Value);
                var respawns = rw.UInt16(storedRespawns);
                n.respawns = respawns == ushort.MaxValue ? -1 : respawns;
            }

            if (Version == 2)
            {
                rw.TimeInt32Nullable(ref n.raceTime);
            }
            else
            {
                count = ReadWriteCheckpointCount(n, rw);
            }

            if (rw.Reader is not null)
            {
                n.checkpoints = new Checkpoint[count];
            }

            // Version 1 stores differences backwards from the race time.
            var previousTime = Version == 1 ? n.raceTime?.TotalMilliseconds ?? -1 : 0;
            if (Version == 1 && previousTime == 0) previousTime = -1;

            for (var i = 0; i < count; i++)
            {
                var index = Version == 1 ? count - i - 1 : i;
                var checkpoint = n.checkpoints![index] ??= new Checkpoint();
                var time = checkpoint.Time?.TotalMilliseconds ?? -1;

                if (Version == 0)
                {
                    checkpoint.Time = rw.TimeInt32Nullable(checkpoint.Time);
                }
                else
                {
                    var delta = rw.Int32(unchecked(Version == 1 ? previousTime - time : time - previousTime));
                    previousTime = unchecked(Version == 1 ? previousTime - delta : previousTime + delta);
                    checkpoint.Time = previousTime == -1 ? null : TimeInt32.FromMilliseconds(previousTime);
                }
            }

            if (Version == 2)
            {
                rw.TimeInt32Nullable(ref n.raceStartTime);
            }
            else if (rw.Reader is not null)
            {
                n.raceStartTime = null;
            }
        }

        private int ReadWriteCheckpointCount(CGameCtnGhost n, GbxReaderWriter rw)
        {
            var count = rw.Writer is null ? 0 : n.checkpoints?.Length ?? 0;
            count = Version == 0 ? rw.Int32(count) : rw.UInt16(checked((ushort)count));

            if (count < 0 || (Version != 0 && count > 10000))
            {
                throw new LengthLimitException(count);
            }

            return count;
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

    public partial class Chunk0309202D
    {
        public int HasInputs;
        public int SimulationFlags; // bit 0 is SteeringWheelSensitivity

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.Int32(ref HasInputs);

            if (HasInputs >= 1)
            {
                throw new Exception("Inputs stored separately");
            }

            rw.String(ref n.validate_ExeVersion);
            rw.UInt32(ref n.validate_ExeChecksum);
            rw.Int32(ref n.validate_OsKind);
            rw.Int32(ref n.validate_CpuKind);
            rw.UnixTime(ref n.walltimeStartTimestamp);
            rw.UnixTime(ref n.walltimeEndTimestamp);
            rw.String(ref n.validate_TitleId);
            rw.Checksum256(ref n.validate_TitleChecksum);
            rw.Int32(ref n.validate_GameRules);
            rw.TimeInt32Nullable(ref n.validate_RaceStartTime);
            rw.Int32(ref n.validate_ValidationSeed);
            rw.Int32(ref SimulationFlags);
            rw.String(ref n.validate_RaceSettings);
        }
    }

    public partial class SettingsInfos
    {
        [Obsolete("Use MouseSensitivitiesEnableSpecific instead.")]
        public bool U06 { get => MouseSensitivitiesEnableSpecific; set => MouseSensitivitiesEnableSpecific = value; }

        [Obsolete("Use MouseScaleY instead.")]
        public float U07 { get => MouseScaleY; set => MouseScaleY = value; }

        [Obsolete("Use MouseScaleFreeLook instead.")]
        public float U11 { get => MouseScaleFreeLook; set => MouseScaleFreeLook = value; }
    }

    public partial class Checkpoint
    {
        public override string ToString()
        {
            var details = new List<string>(3);

            if (CheckpointId.HasValue)
                details.Add($"ID: {CheckpointId.Value}");

            if (Speed.HasValue)
                details.Add($"{Speed.Value}km/h");

            if (StuntsScore.HasValue)
                details.Add($"{StuntsScore.Value} pts.");

            return details.Count > 0
                ? $"{Time.ToTmString()} ({string.Join(", ", details)})"
                : Time.ToTmString();
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
