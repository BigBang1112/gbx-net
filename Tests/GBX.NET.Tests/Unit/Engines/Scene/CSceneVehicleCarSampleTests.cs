using GBX.NET.Engines.Plug;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;
using TmEssentials;

namespace GBX.NET.Tests.Unit.Engines.Scene;

[Category("Unit")]
public class CSceneVehicleCarSampleTests
{
    [Test]
    [Arguments(0x70, 46)]
    [Arguments(0x30, 42)]
    [Arguments(0x10, 38)]
    [Arguments(0x20, 50)]
    [Arguments(0x60, 62)]
    [Arguments(0x71, 42)]
    [Arguments(0x61, 50)]
    [Arguments(0xE1, 62)]
    public async Task Version2_ReadAndWrite_UsesInlineDynaFlags(int flags, int expectedSize)
    {
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, [(byte)flags])
        {
            Position = new Vec3(1, 2, 3),
            Rotation = new Quat(0, 0, 0, 1),
            Velocity = new Vec3(1, 2, 3),
            AngularVelocity = new Vec3(0.1f, 0.2f, 0.3f),
            FLWheelRotation = (float)Math.PI * 2,
            FRWheelRotation = (float)Math.PI,
            TurboStrength = 0.5f,
            U25 = 0xF8,
            U26 = 0xC0,
            U27 = 0x3F
        };
        var raw = (CSceneVehicleCar.ISampleRawData)sample;
        raw.SpeedForward = 0x1234;
        raw.SpeedSideward = 0x5678;
        raw.RPM = 0x9ABC;
        var bytes = Write(sample, version: 2);
        await Assert.That(bytes.Length).IsEqualTo(expectedSize);
        await Assert.That(bytes[0]).IsEqualTo((byte)flags);

        var restored = new CSceneVehicleCar.Sample(TimeInt32.Zero, bytes);
        using var input = new MemoryStream(bytes);
        using var reader = new GbxReader(input);
        restored.Read(reader, version: 2);

        var restoredRaw = (CSceneVehicleCar.ISampleRawData)restored;
        await Assert.That(input.Position).IsEqualTo(input.Length);
        await Assert.That(restoredRaw.SpeedForward).IsEqualTo(raw.SpeedForward);
        await Assert.That(restoredRaw.SpeedSideward).IsEqualTo(raw.SpeedSideward);
        await Assert.That(restoredRaw.RPM).IsEqualTo(raw.RPM);
        await Assert.That(restoredRaw.TurboStrength).IsEqualTo(raw.TurboStrength);
        await Assert.That(restored.Horn).IsEqualTo((byte)3);
        await Assert.That(restored.U25_3).IsEqualTo((byte)3);
        await Assert.That(restored.U25_4).IsTrue();
        await Assert.That(restored.U26).IsEqualTo((byte)0xC0);
        await Assert.That(restored.U27).IsEqualTo((byte)0x3F);
        await Assert.That(restored.FLWheelRotation).IsBetween(6.27f, 6.30f);
        if ((flags & 0x20) == 0 || ((flags & 0x80) == 0 && (flags & 0xF) != 0))
        {
            await Assert.That(restored.Velocity).IsEqualTo(new Vec3(0, 0, 0));
        }
        if ((flags & 0x20) == 0 || (flags & 0x40) == 0)
        {
            await Assert.That(restored.AngularVelocity).IsEqualTo(new Vec3(0, 0, 0));
        }
        await Assert.That(Write(restored, version: 2)).IsEquivalentTo(bytes, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Version2_ReadAndWrite_UsesSignedQuaternionAxisBytes()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write((byte)0x10);
            writer.Write(new Vec3(1, 2, 3));
            writer.Write((byte)85);
            writer.Write((sbyte)-63);
            writer.Write((sbyte)-32);
            writer.Write(new byte[22]);
        }
        var bytes = stream.ToArray();
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, bytes);
        stream.Position = 0;
        using var reader = new GbxReader(stream);

        sample.Read(reader, version: 2);

        await Assert.That(sample.Rotation.W).IsBetween(0.499f, 0.501f);
        await Assert.That(sample.Rotation.Y).IsLessThan(0f);
        await Assert.That(sample.Rotation.Z).IsLessThan(0f);
        await Assert.That(Write(sample, version: 2)).IsEquivalentTo(bytes, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(255)]
    public async Task Version2_ReadAndWrite_PreservesUndefinedQuaternionAxis(int angle)
    {
        var bytes = new byte[38];
        bytes[0] = 0x10;
        bytes[13] = (byte)angle;
        bytes[14] = 126;
        bytes[15] = 37;
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, bytes);
        using var input = new MemoryStream(bytes);
        using var reader = new GbxReader(input);

        sample.Read(reader, version: 2);

        await Assert.That(Write(sample, version: 2)).IsEquivalentTo(bytes, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(4)]
    [Arguments(8)]
    [Arguments(0x81A5)]
    [Arguments(0xFFFF)]
    public async Task Version6_ReadAndWrite_PreservesNetworkState(int netState)
    {
        // Native layout: Vec3Pos_9, Quat_6 (identity), SVehicleSimpleNetState.
        byte[] bytes = [0x80, 0xF4, 1, 0x80, 0xE8, 3, 0x80, 0xDC, 5,
            0, 0, 0, 0, 0, 0, (byte)netState, (byte)(netState >> 8)];
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, bytes);
        using var input = new MemoryStream(bytes);
        using var reader = new GbxReader(input);

        sample.Read(reader, version: 6);

        await Assert.That(input.Position).IsEqualTo(17L);
        await Assert.That(sample.Position.X).IsBetween(0.999f, 1.001f);
        await Assert.That(sample.Position.Y).IsBetween(1.999f, 2.001f);
        await Assert.That(sample.Position.Z).IsBetween(2.999f, 3.001f);
        await Assert.That(sample.Rotation).IsEqualTo(new Quat(0, 0, 0, 1));
        await Assert.That(sample.FLIsSliding).IsEqualTo((netState & 1) != 0);
        await Assert.That(sample.FLOnGround).IsEqualTo(sample.FLIsSliding);
        await Assert.That(sample.FRIsSliding).IsEqualTo((netState & 2) != 0);
        await Assert.That(sample.FROnGround).IsEqualTo(sample.FRIsSliding);
        await Assert.That(sample.RRIsSliding).IsEqualTo((netState & 4) != 0);
        await Assert.That(sample.RROnGround).IsEqualTo(sample.RRIsSliding);
        await Assert.That(sample.RLIsSliding).IsEqualTo((netState & 8) != 0);
        await Assert.That(sample.RLOnGround).IsEqualTo(sample.RLIsSliding);
        await Assert.That(sample.Horn).IsEqualTo((byte)(netState >> 4 & 3));
        await Assert.That(sample.FLGroundContactMaterial).IsEqualTo(CPlugSurface.MaterialId.Asphalt);
        await Assert.That(Write(sample, version: 6)).IsEquivalentTo(bytes, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Version6_EditingFlagsAndRPM_PreservesSplashEvents()
    {
        var data = CreateNetworkStateData(0x81C0);
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, data);
        using var input = new MemoryStream(data);
        using var reader = new GbxReader(input);
        sample.Read(reader, version: 6);

        await Assert.That(sample.RPM).IsBetween(8362f, 8363f);

        sample.FRIsSliding = true;
        sample.RLIsSliding = true;
        sample.U25 = 3 << 3;
        sample.RPM = 11000;

        var bytes = Write(sample, version: 6);

        await Assert.That(bytes.Length).IsEqualTo(17);
        await Assert.That(bytes[15]).IsEqualTo((byte)0xFA);
        await Assert.That(bytes[16]).IsEqualTo((byte)0xFF);
    }

    [Test]
    [Arguments(6)]
    [Arguments(13)]
    public void NetworkVersions_Read_RejectsTrailingBytes(int version)
    {
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, []);
        using var input = new MemoryStream(new byte[18]);
        using var reader = new GbxReader(input);

        Assert.Throws<InvalidDataException>(() => sample.Read(reader, version));
    }

    [Test]
    [Arguments(0, false, false, false)]
    [Arguments(1, true, false, false)]
    [Arguments(2, false, true, false)]
    [Arguments(4, false, false, true)]
    [Arguments(7, true, true, true)]
    public async Task Version13_ReadAndWrite_SeparatesSlidingBrakeAndGroundContact(int flags, bool sliding, bool brake, bool ground)
    {
        var data = CreateNetworkStateData(0x81F8 | flags);
        var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, data);
        using var input = new MemoryStream(data);
        using var reader = new GbxReader(input);

        sample.Read(reader, version: 13);

        await Assert.That(sample.FLIsSliding).IsEqualTo(sliding);
        await Assert.That(sample.FRIsSliding).IsEqualTo(sliding);
        await Assert.That(sample.RRIsSliding).IsEqualTo(sliding);
        await Assert.That(sample.RLIsSliding).IsEqualTo(sliding);
        await Assert.That(sample.FLOnGround).IsEqualTo(ground);
        await Assert.That(sample.FROnGround).IsEqualTo(ground);
        await Assert.That(sample.RROnGround).IsEqualTo(ground);
        await Assert.That(sample.RLOnGround).IsEqualTo(ground);
        await Assert.That(sample.Brake).IsEqualTo(brake ? 1f : 0f);
        await Assert.That(sample.Horn).IsEqualTo((byte)3);
        await Assert.That(sample.RPM).IsBetween(8362f, 8363f);
        await Assert.That(Write(sample, version: 13)).IsEquivalentTo(data, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(6)]
    [Arguments(13)]
    public async Task NetworkVersions_ReadAndWrite_PreservesAllRPMLevels(int version)
    {
        for (var level = 0; level < 128; level++)
        {
            var data = CreateNetworkStateData(level << 9);
            var sample = new CSceneVehicleCar.Sample(TimeInt32.Zero, data);
            using var input = new MemoryStream(data);
            using var reader = new GbxReader(input);

            sample.Read(reader, version);

            if (level == 0)
            {
                await Assert.That(sample.RPM).IsBetween(version == 6 ? 0f : 99f, version == 6 ? 0f : 101f);
            }
            else if (level == 127)
            {
                await Assert.That(sample.RPM).IsBetween(10999f, 11001f);
            }
            await Assert.That(Write(sample, version)).IsEquivalentTo(data, CollectionOrdering.Matching);
        }
    }

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

    private static byte[] CreateNetworkStateData(int netData)
        => [0x80, 0, 0, 0x80, 0, 0, 0x80, 0, 0, 0, 0, 0, 0, 0, 0, (byte)netData, (byte)(netData >> 8)];

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
