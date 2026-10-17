using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugDynaPointModelTests
{
    [Test]
    public async Task ReadWrite_Archive_ReadsNativeFloatingPointFieldsAndPreservesBytes()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0);
            writer.Write(new Vec3(1.5f, -2.5f, 3.5f));
            writer.Write(4.5f);
            writer.Write(0.2f);
            writer.Write(0.9f);
            writer.Write(-1.5f);
            writer.Write(0x12345678);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadReadable<CPlugDynaPointModel>();
        await Assert.That(node.Version).IsEqualTo(0);
        await Assert.That(node.Center).IsEqualTo(new Vec3(1.5f, -2.5f, 3.5f));
        await Assert.That(node.Radius).IsEqualTo(4.5f);
        await Assert.That(node.Friction).IsEqualTo(0.2f);
        await Assert.That(node.Restitution).IsEqualTo(0.9f);
        await Assert.That(node.GravityCoef).IsEqualTo(-1.5f);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);

        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        {
            node.Write(writer);
            writer.Write(0x12345678);
        }
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWrite_DefaultArchive_UsesNativePhysicalParameters()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.ReadableWritable(new CPlugDynaPointModel());
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0);
        await Assert.That(reader.ReadVec3()).IsEqualTo(Vec3.Zero);
        await Assert.That(reader.ReadSingle()).IsEqualTo(0.5f);
        await Assert.That(reader.ReadSingle()).IsEqualTo(0.8f);
        await Assert.That(reader.ReadSingle()).IsEqualTo(0.5f);
        await Assert.That(reader.ReadSingle()).IsEqualTo(1f);
        await Assert.That(payload.Length).IsEqualTo(32L);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }

    [Test]
    public async Task ObsoleteIntegerProperties_WhenChanged_PreserveFloatBits()
    {
        int[] bits = [0, unchecked((int)0x80000000), 0x7FC12345, 0x3FC00000, 0x40000000, 0x3E4CCCCD, 0x3F666666, unchecked((int)0xBF800000)];
#pragma warning disable CS0618
        var node = new CPlugDynaPointModel
        {
            U01 = bits[0],
            U02 = bits[1],
            U03 = bits[2],
            U04 = bits[3],
            U05 = bits[4],
            U06 = bits[5],
            U07 = bits[6],
            U08 = bits[7]
        };
        await Assert.That(new[] { node.U01, node.U02, node.U03, node.U04, node.U05, node.U06, node.U07, node.U08 })
            .IsEquivalentTo(bits, CollectionOrdering.Matching);
#pragma warning restore CS0618

        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload)) node.Write(writer);
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        foreach (var value in bits) await Assert.That(reader.ReadInt32()).IsEqualTo(value);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
    }
}
