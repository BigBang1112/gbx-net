using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CSceneVehicleCarSampleTests
{
    [Theory]
    [InlineData(17, 74)]
    [InlineData(18, 75)]
    [InlineData(19, 76)]
    [InlineData(20, 77)]
    public void Write_StateFirstVersions_HasExpectedFixedSize(int version, int expectedSize)
    {
        var sample = new CSceneVehicleCar.Sample(new TimeInt32(0), []);

        var data = Write(sample, version);

        Assert.Equal(expectedSize, data.Length);
    }

    [Fact]
    public void Version20_ReadAndWrite_UsesStateFirstLayout()
    {
        var sample = new CSceneVehicleCar.Sample(new TimeInt32(0), [])
        {
            Position = new Vec3(1, 2, 3),
            Rotation = new Quat(0, 0, 0, 1),
            U35_2 = 1,
            U41 = 0xA9,
            U42 = 0xB8,
            U44 = 0xC7,
            U45 = 0xD6
        };
        var raw = (CSceneVehicleCar.ISampleRawData)sample;
        raw.SpeedForward = 0x1122;
        raw.Velocity = 0xA1B2C3D4;
        raw.AngularVelocity = 0xE5F60718;

        var data = Write(sample, version: 20);

        Assert.Equal((byte)0x22, data[0]);
        Assert.Equal((byte)0x11, data[1]);
        Assert.Equal(BitConverter.GetBytes(1f), data[47..51]);
        Assert.Equal(BitConverter.GetBytes(0xA1B2C3D4u), data[65..69]);
        Assert.Equal((byte)0xA9, data[73]);
        Assert.Equal((byte)0xB8, data[74]);
        Assert.Equal((byte)0xC7, data[75]);
        Assert.Equal((byte)0xD6, data[76]);

        var restored = new CSceneVehicleCar.Sample(new TimeInt32(0), []);
        using var input = new MemoryStream(data);
        using var reader = new GbxReader(input);
        restored.Read(reader, version: 20);

        var restoredRaw = (CSceneVehicleCar.ISampleRawData)restored;
        Assert.Equal(raw.SpeedForward, restoredRaw.SpeedForward);
        Assert.Equal(raw.Velocity, restoredRaw.Velocity);
        Assert.Equal(raw.AngularVelocity, restoredRaw.AngularVelocity);
        Assert.Equal(sample.U35_2, restored.U35_2);
        Assert.Equal(sample.U41, restored.U41);
        Assert.Equal(sample.U42, restored.U42);
        Assert.Equal(sample.U44, restored.U44);
        Assert.Equal(sample.U45, restored.U45);
    }

    private static byte[] Write(CSceneVehicleCar.Sample sample, int version)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            sample.Write(writer, version);
        }

        return stream.ToArray();
    }
}
