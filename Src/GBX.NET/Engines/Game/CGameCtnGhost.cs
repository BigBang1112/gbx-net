
using GBX.NET.Inputs;
using System.Collections.Immutable;
using System.Numerics;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnGhost
{
    public Id? GhostUid { get; set; }

    private TimeInt32 eventsDuration;
    public TimeInt32 EventsDuration { get => eventsDuration; set => eventsDuration = value; }

    private string? validate_ExeVersion;
    public string? Validate_ExeVersion { get => validate_ExeVersion; set => validate_ExeVersion = value; }

    private uint validate_ExeChecksum;
    public uint Validate_ExeChecksum { get => validate_ExeChecksum; set => validate_ExeChecksum = value; }

    private int validate_OsKind;
    public int Validate_OsKind { get => validate_OsKind; set => validate_OsKind = value; }

    private int validate_CpuKind;
    public int Validate_CpuKind { get => validate_CpuKind; set => validate_CpuKind = value; }

    private string? validate_RaceSettings;
    public string? Validate_RaceSettings { get => validate_RaceSettings; set => validate_RaceSettings = value; }

    private ImmutableList<IInput>? inputs;
    public ImmutableList<IInput>? Inputs { get => inputs; set => inputs = value; }

    private bool steeringWheelSensitivity;
    public bool SteeringWheelSensitivity { get => steeringWheelSensitivity; set => steeringWheelSensitivity = value; }

    private string? validate_TitleId;
    public string? Validate_TitleId { get => validate_TitleId; set => validate_TitleId = value; }

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
            var inputNames = r.ReadArrayId();

            var numEntries = r.ReadInt32();
            U02 = r.ReadInt32(); // CountLimit?

            var inputs = ImmutableList.CreateBuilder<IInput>();

            for (var i = 0; i < numEntries; i++)
            {
                var time = TimeInt32.FromMilliseconds(r.ReadInt32() - 100000);
                var inputNameIndex = r.ReadByte();
                var data = r.ReadUInt32();

                var name = inputNames[inputNameIndex];

                inputs.Add(NET.Inputs.Input.Parse(time, name, data));
            }

            n.inputs = inputs.ToImmutable();
        }

        private void WriteInputs(CGameCtnGhost n, GbxWriter w)
        {
            var inputNames = n.inputs?
                .Select(NET.Inputs.Input.GetName)
                .Distinct()
                .ToImmutableList() ?? ImmutableList<string>.Empty;

            w.WriteListId(inputNames);

            w.Write(n.inputs?.Count ?? 0);
            w.Write(U02);

            if (n.inputs is null)
            {
                return;
            }

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
        public int U03;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            if (n.eventsDuration != TimeInt32.Zero)
            {
                rw.Int32(ref U03);
            }
        }
    }

    public partial class Chunk03092025 : IVersionable
    {
        public int Version { get; set; }

        public Chunk03092019 Chunk019 { get; set; } = new();

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version == 0)
            {
                rw.Chunk(n, Chunk019);

                if (n.eventsDuration != TimeInt32.Zero)
                {
                    rw.Boolean(ref n.steeringWheelSensitivity);
                }
            }
            else
            {
                rw.TimeInt32(ref n.eventsDuration);

                Chunk019 ??= new();
                Chunk019.ReadWriteInputs(n, rw);

                rw.Int32(ref Chunk019.U03);
                rw.Boolean(ref n.steeringWheelSensitivity);
            }
        }
    }

    public partial class Chunk03092028
    {
        public UInt256? U01;

        public override void ReadWrite(CGameCtnGhost n, GbxReaderWriter rw)
        {
            if (n.EventsDuration == TimeInt32.Zero)
            {
                return;
            }

            rw.String(ref n.validate_TitleId);
            rw.UInt256(ref U01);
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
