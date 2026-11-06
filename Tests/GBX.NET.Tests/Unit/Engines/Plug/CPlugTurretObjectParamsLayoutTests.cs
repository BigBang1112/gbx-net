using GBX.NET.Components;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugTurretObjectParamsLayoutTests
{
    [Test]
    public async Task Parse_TurboTurret_UsesLegacyClassForReusedClassId()
    {
        var node = new CPlugTurretObjectParams { IdName = "Turret", ShootPeriod = new(1500) };
        node.CreateChunk<CPlugTurretObjectParams.Chunk090D6000>().Version = 3;
        using var stream = new MemoryStream();
        node.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);
        stream.Position = 0;

        var restored = Gbx.ParseNode<CPlugTurretObjectParams>(stream, new GbxReadSettings
        {
            GameVersion = GameVersion.TMT,
            IgnoreExceptionsInBody = false
        });

        await Assert.That(restored.IdName).IsEqualTo("Turret");
        await Assert.That(restored.ShootPeriod.TotalMilliseconds).IsEqualTo(1500);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_Turret_PreservesNativeWidthsAndVersionBoundaries(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write((byte)7);
            writer.Write(1500);
            writer.Write(2f);
            writer.Write(40f);
            writer.Write(Iso4.Identity);
            if (version >= 1) writer.WriteIdAsString("Turret");
            if (version >= 2)
            {
                writer.Write(3f);
                writer.Write(4f);
                writer.Write(50f);
                writer.Write(60f);
            }
            if (version >= 3) writer.Write(90f);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugTurretObjectParams();
        var chunk = new CPlugTurretObjectParams.Chunk090D6000();
        await Assert.That(node.ShootPeriod.TotalMilliseconds).IsEqualTo(1000);
        await Assert.That(node.BulletOffset).IsEqualTo(Iso4.Identity);
        await Assert.That(node.HorizontalFOV).IsEqualTo(120f);

        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.ShootPeriod.TotalMilliseconds).IsEqualTo(1500);
        await Assert.That(node.MinShootDistance).IsEqualTo(2f);
        await Assert.That(node.MaxShootDistance).IsEqualTo(40f);
        if (version >= 1) await Assert.That(node.IdName).IsEqualTo("Turret");
        if (version >= 2) await Assert.That(node.YawSpeed).IsEqualTo(60f);
        if (version >= 3) await Assert.That(node.HorizontalFOV).IsEqualTo(90f);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }
}
