using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit;

public class CSceneVehicleCarSampleTests
{
    [Test]
    [Arguments(17, 74)]
    [Arguments(18, 75)]
    [Arguments(19, 76)]
    [Arguments(20, 77)]
    public async Task Write_StateFirstVersions_HasExpectedFixedSize(int version, int expectedSize)
    {
        var sample = new CSceneVehicleCar.Sample(new TimeInt32(0), []);

        var data = Write(sample, version);

        await Assert.That(data.Length).IsEqualTo(expectedSize);
    }

    [Test]
    public async Task Version20_ReadAndWrite_UsesStateFirstLayout()
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

        await Assert.That(data[0]).IsEqualTo((byte)0x22);
        await Assert.That(data[1]).IsEqualTo((byte)0x11);
        await Assert.That(data[47..51]).IsEquivalentTo(BitConverter.GetBytes(1f), CollectionOrdering.Matching);
        await Assert.That(data[65..69]).IsEquivalentTo(BitConverter.GetBytes(0xA1B2C3D4u), CollectionOrdering.Matching);
        await Assert.That(data[73]).IsEqualTo((byte)0xA9);
        await Assert.That(data[74]).IsEqualTo((byte)0xB8);
        await Assert.That(data[75]).IsEqualTo((byte)0xC7);
        await Assert.That(data[76]).IsEqualTo((byte)0xD6);

        var restored = new CSceneVehicleCar.Sample(new TimeInt32(0), []);
        using var input = new MemoryStream(data);
        using var reader = new GbxReader(input);
        restored.Read(reader, version: 20);

        var restoredRaw = (CSceneVehicleCar.ISampleRawData)restored;
        await Assert.That(restoredRaw.SpeedForward).IsEqualTo(raw.SpeedForward);
        await Assert.That(restoredRaw.Velocity).IsEqualTo(raw.Velocity);
        await Assert.That(restoredRaw.AngularVelocity).IsEqualTo(raw.AngularVelocity);
        await Assert.That(restored.U35_2).IsEqualTo(sample.U35_2);
        await Assert.That(restored.U41).IsEqualTo(sample.U41);
        await Assert.That(restored.U42).IsEqualTo(sample.U42);
        await Assert.That(restored.U44).IsEqualTo(sample.U44);
        await Assert.That(restored.U45).IsEqualTo(sample.U45);
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
