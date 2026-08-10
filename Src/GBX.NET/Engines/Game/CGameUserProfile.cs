namespace GBX.NET.Engines.Game;

public partial class CGameUserProfile
{
    public partial class Chunk031CC001 : IVersionable
    {
        public int Version { get; set; }

        public RawData U01 = new([], null);

        public override void Read(CGameUserProfile n, GbxReader r)
        {
            Version = r.ReadInt32();
            U01 = r.ReadEncapsulated();
        }

        public override void Write(CGameUserProfile n, GbxWriter w)
        {
            w.Write(Version);
            w.WriteEncapsulated(U01);
        }
    }

    public partial class Chunk031CC009 : IVersionable
    {
        public int Version { get; set; }

        public Id U01;

        public override void ReadWrite(CGameUserProfile n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
        }
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
        public int U07 { get; set; }

        public override string ToString()
        {
            return Vehicle.ToString();
        }
    }
}
