using GBX.NET.Engines.Hms;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Hms;

[Category("Unit")]
public class CHmsZoneLayoutTests
{
    [Test]
    [Arguments(0u)]
    [Arguments(6u)]
    [Arguments(10u)]
    [Arguments(15u)]
    [Arguments(uint.MaxValue)]
    public async Task GlobalFogHas28BytesAndMasksNonpersistentFlags(uint flags)
    {
        var payload = Payload(flags);
        var node = new CHmsZone();
        var chunk = new CHmsZone.Chunk06004002();
        using var input = new MemoryStream();
        input.Write(payload);
        using (var suffix = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true)) suffix.Write(0xDEADBEEFu);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(input.Position).IsEqualTo(8L + 28);
            await Assert.That(reader.ReadUInt32()).IsEqualTo(0xDEADBEEFu);
        }

        await Assert.That(node.FogRGB).IsEqualTo(new Vec3(0.25f, 0.5f, 0.75f));
        await Assert.That(node.FogLinearStart).IsEqualTo(12.5f);
        await Assert.That(node.FogLinearEnd).IsEqualTo(500f);
        await Assert.That(node.FogExpDensity).IsEqualTo(0.125f);
        await Assert.That(node.FogFlags).IsEqualTo(flags & 15);
        await Assert.That(node.FogByVertex).IsEqualTo((flags & 1) != 0);
        await Assert.That(node.FogFormula).IsEqualTo((CHmsZone.EGxFogFormula)((flags >> 1) & 3));
        await Assert.That(node.FogSpace).IsEqualTo((CHmsZone.EGxFogSpace)((flags >> 3) & 1));

        // Native ArchiveFog also clears transient bits before writing.
        node.FogFlags = flags;
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(Payload(flags & 15), CollectionOrdering.Matching);
        await Assert.That(node.FogFlags).IsEqualTo(flags & 15);
    }

    [Test]
    public async Task FogPropertiesPreserveAdjacentPackedSettings()
    {
        var node = new CHmsZone();
        await Assert.That(node.FogRGB).IsEqualTo(new Vec3(1, 1, 1));
        await Assert.That(node.FogFormula).IsEqualTo(CHmsZone.EGxFogFormula.Linear);
        await Assert.That(node.FogSpace).IsEqualTo(CHmsZone.EGxFogSpace.CameraFarZ);

        node.FogByVertex = true;
        node.FogFormula = CHmsZone.EGxFogFormula.Exp2;
        node.FogSpace = CHmsZone.EGxFogSpace.World;
        await Assert.That(node.FogFlags).IsEqualTo(13u);
        node.FogByVertex = false;
        await Assert.That(node.FogFlags).IsEqualTo(12u);
        node.FogFormula = CHmsZone.EGxFogFormula.None;
        await Assert.That(node.FogFlags).IsEqualTo(8u);
        node.FogSpace = CHmsZone.EGxFogSpace.CameraFarZ;
        await Assert.That(node.FogFlags).IsEqualTo(0u);
    }

    private static byte[] Payload(uint flags)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(10); // Deprecated node-array version.
        writer.Write(0); // Fog plane count.
        foreach (var value in new[] { 0.25f, 0.5f, 0.75f, 12.5f, 500f, 0.125f }) writer.Write(value);
        writer.Write(flags);
        return stream.ToArray();
    }
}
