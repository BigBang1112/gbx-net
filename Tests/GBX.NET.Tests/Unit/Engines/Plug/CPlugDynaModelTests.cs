using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDynaModelTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task OlderArchivesResetSettingsAbsentFromTheirPayload(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            for (var i = 0; i < 15; i++) writer.Write(0f);
            if (version >= 1) writer.Write(false);
            if (version >= 2) writer.Write((byte)2);
        }

        var node = new CPlugDynaModel
        {
            UseTMSimulation = false,
            SleepingMethod = CPlugDynaModel.ESleepingMethod.None,
            EnableSubStepping = true
        };
        payload.Position = 0;
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) node.ReadWrite(rw);

        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.UseTMSimulation).IsEqualTo(version == 0);
        await Assert.That(node.SleepingMethod).IsEqualTo(version >= 2
            ? CPlugDynaModel.ESleepingMethod.LowLinearVel
            : CPlugDynaModel.ESleepingMethod.LowLinearVel_AngularVel);
        await Assert.That(node.EnableSubStepping).IsFalse();
    }

    [Test]
    public async Task DefaultArchivePreservesNativePhysicalParameters()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) new CPlugDynaModel().ReadWrite(rw);

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        await Assert.That(reader.ReadInt32()).IsEqualTo(4);
        await Assert.That(reader.ReadSingle()).IsEqualTo(1f);
        await Assert.That(reader.ReadSingle()).IsEqualTo(0.3f);
        await Assert.That(reader.ReadVec3()).IsEqualTo(Vec3.Zero);
        var inverseInertia = reader.ReadMat3();
        var diagonal = 3f / 12.566371f; // Native unit-sphere initialization.
        await Assert.That(inverseInertia).IsEqualTo(new Mat3(diagonal, 0, 0, 0, diagonal, 0, 0, 0, diagonal));
        await Assert.That(reader.ReadSingle()).IsEqualTo(10000f);
        await Assert.That(reader.ReadBoolean()).IsTrue();
        await Assert.That(reader.ReadByte()).IsEqualTo((byte)1);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0); // Four-byte substepping flag.
        await Assert.That(reader.ReadInt32()).IsEqualTo(-1); // No water model.
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }

    [Test]
    public async Task ObsoleteComponentsUpdateTheNamedArchiveMembers()
    {
#pragma warning disable CS0618
        var node = new CPlugDynaModel
        {
            U01 = 1.5f,
            U02 = 2.5f,
            U03 = 3.5f,
            U04 = 4.5f,
            U05 = 5.5f,
            U06 = 6.5f,
            U07 = 7.5f,
            U08 = 8.5f,
            U09 = 9.5f,
            U10 = 10.5f,
            U11 = 11.5f,
            U12 = 12.5f,
            U13 = 13.5f,
            U14 = 14.5f,
            U15 = 15.5f,
            U16 = false,
            U17 = 2,
            U18 = 1
        };
#pragma warning restore CS0618

        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer)) node.ReadWrite(rw);

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        await Assert.That(reader.ReadInt32()).IsEqualTo(4);
        for (var i = 1; i <= 15; i++) await Assert.That(reader.ReadSingle()).IsEqualTo(i + 0.5f);
        await Assert.That(reader.ReadBoolean()).IsFalse();
        await Assert.That(reader.ReadByte()).IsEqualTo((byte)2);
        await Assert.That(reader.ReadInt32()).IsEqualTo(1);
        await Assert.That(reader.ReadInt32()).IsEqualTo(-1);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }
}
