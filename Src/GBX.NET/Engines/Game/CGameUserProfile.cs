namespace GBX.NET.Engines.Game;

public partial class CGameUserProfile
{
    public partial class Chunk031CC009 : IVersionable
    {
        public int Version { get; set; } = 1;

        public Id U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
        }
    }

    public partial class Sticker
    {
        [Obsolete("Use StickerName instead.")]
        public string? U01 { get => StickerName; set => StickerName = value; }

        [Obsolete("Use SlotName instead.")]
        public string? U02 { get => SlotName; set => SlotName = value; }
    }

    public partial class ContextTime
    {
        [Obsolete("Use Context instead.")]
        public string? U01 { get => Context; set => Context = value; }

        [Obsolete("Use GameModeTimeSeconds instead.")]
        public uint U02 { get => GameModeTimeSeconds; set => GameModeTimeSeconds = value; }

        [Obsolete("Use PlayTimeSeconds instead.")]
        public uint U03 { get => PlayTimeSeconds; set => PlayTimeSeconds = value; }
    }

    public partial class ContextMapRecordForProfile
    {
        [Obsolete("Use Context instead.")]
        public string? U03 { get => Context; set => Context = value; }
    }

    public partial class Unknown
    {
        [Obsolete("Use Vehicle instead, which also contains the collection and author.")]
        public string? U01
        {
            get => Vehicle.Id;
            set => Vehicle = Vehicle with { Id = value ?? string.Empty };
        }

        [Obsolete("Use AnalogSensitivity instead.")]
        public float U02 { get => AnalogSensitivity; set => AnalogSensitivity = value; }

        [Obsolete("Use AnalogDeadZone instead.")]
        public float U03 { get => AnalogDeadZone; set => AnalogDeadZone = value; }

        [Obsolete("Use AnalogSteerV2 instead.")]
        public bool U04 { get => AnalogSteerV2; set => AnalogSteerV2 = value; }
    }

    public partial class Unknown3
    {
        [Obsolete("Use MapUid instead.")]
        public string? U01 { get => MapUid; set => MapUid = value; }

        [Obsolete("Use Medal instead.")]
        public int U06 { get => Medal; set => Medal = value; }
    }

    public partial class Unknown4
    {
        [Obsolete("Use Version instead.")]
        public int U01 { get => Version; set => Version = value; }
    }

    public partial class Chunk031CC01B
    {
        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            foreach (var deviceSettings in n.vehicleSettings ?? [])
            {
                deviceSettings.AnalogSensitivity = rw.Single(deviceSettings.AnalogSensitivity);
                deviceSettings.AnalogDeadZone = rw.Single(deviceSettings.AnalogDeadZone);
                deviceSettings.InvertSteeringAxis = rw.Boolean(deviceSettings.InvertSteeringAxis);
                deviceSettings.AccelerateUseToggleMode = rw.Boolean(deviceSettings.AccelerateUseToggleMode);
                deviceSettings.BrakeUseToggleMode = rw.Boolean(deviceSettings.BrakeUseToggleMode);
                deviceSettings.VibrationIntensity = rw.Single(deviceSettings.VibrationIntensity);
                deviceSettings.CenterSpringIntensity = rw.Single(deviceSettings.CenterSpringIntensity);
                deviceSettings.U06 = rw.Int32(deviceSettings.U06);
            }
        }
    }

    public partial class Chunk031CC021
    {
        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            foreach (var deviceSettings in n.vehicleSettings ?? [])
            {
                deviceSettings.U07 = rw.Int32(deviceSettings.U07);
            }
        }
    }

    public partial class DeviceSettings
    {
        public bool InvertSteeringAxis { get; set; }
        public bool AccelerateUseToggleMode { get; set; }
        public bool BrakeUseToggleMode { get; set; }
        public int U06 { get; set; }
        public int U07 { get; set; } = 1;

        public override string ToString()
        {
            return Vehicle.ToString();
        }
    }
}
