using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugShaderLayoutTests
{
    [Test]
    [Arguments(0x000)]
    [Arguments(0x005)]
    [Arguments(0x007)]
    [Arguments(0x008)]
    [Arguments(0x009)]
    [Arguments(0x00A)]
    [Arguments(0x00B)]
    [Arguments(0x00C)]
    [Arguments(0x00D)]
    [Arguments(0x00E)]
    [Arguments(0x00F)]
    [Arguments(0x010)]
    [Arguments(0x011)]
    [Arguments(0x012)]
    [Arguments(0x013)]
    [Arguments(0x014)]
    [Arguments(0x015)]
    [Arguments(0x016)]
    [Arguments(0x018)]
    [Arguments(0x019)]
    [Arguments(0x01A)]
    [Arguments(0x01B)]
    [Arguments(0x01C)]
    [Arguments(0x01D)]
    [Arguments(0x01E)]
    [Arguments(0x01F)]
    [Arguments(0x020)]
    public async Task ReadWrite_NativePayloads_PreserveFieldOrderAndValues(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WritePayload(writer, offset);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugShader();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
        if (offset is >= 0x010 and <= 0x016 or 0x020)
            await Assert.That(node.VisibleIdFlags).IsEqualTo((ushort)0xFEDC);
        if (offset is 0x01A or 0x01B)
            await Assert.That(node.VisibleMaxDist).IsEqualTo(250.5f);
        if (offset == 0x01B)
            await Assert.That(node.VisibleMinScreenHeight).IsEqualTo(0.25f);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_RequirementsVersionBoundaries_PreserveUnknownHighBits(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(version);
            writer.Write(0xFEDCBA9876543210UL);
            if (version >= 3) writer.Write(0xF1234567u);
            writer.Write(1.25f);
            writer.Write(-1);
            writer.Write((ushort)0xFEDC);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugShader();
        var chunk = new CPlugShader.Chunk09002020();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(node.RequirementFlags).IsEqualTo(0xFEDCBA9876543210UL);
        if (version >= 3) await Assert.That(chunk.U01).IsEqualTo(0xF1234567u);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task ReadWrite_LegacySorting_PreservesUpperWordOriginalEncodingAndWriterState(int mode)
    {
        var flags = 0xFEDCBA9800040000UL | ((ulong)mode << 26);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(flags);
            writer.Write(1.0f);
            writer.Write(-1);
            writer.Write((ushort)1);
        }
        payload.Position = 0;
        var node = new CPlugShader();
        var chunk = new CPlugShader.Chunk09002014();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) chunk.ReadWrite(node, rw);
        await Assert.That(node.RequirementFlags >> 32).IsEqualTo(0xFEDCBA98UL);
        await Assert.That((int)node.SortPosition).IsEqualTo(mode == 0 ? 0 : mode == 1 ? 2 : 3);
        var before = node.RequirementFlags;
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(node.RequirementFlags).IsEqualTo(before);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(-1, false)]
    [Arguments(0, false)]
    [Arguments(0, true)]
    public async Task ReadWrite_CustomShaderEntries_PreservePresenceAndOldEncoding(int version, bool hasShader)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(-1);
            if (version >= 0) writer.Write(hasShader);
            if (version < 0 || hasShader) writer.Write(-1);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var entry = new CPlugShader.ShaderCustom();
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            entry.ReadWrite(rw, version);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(entry.HasShader).IsEqualTo(hasShader);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) entry.ReadWrite(rw, version);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Read_LegacyFlags_ReplacesValuesWithoutWriterConversion()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(0xFEDCBA9800040000UL);
            writer.Write(1.0f);
            writer.Write(-1);
            writer.Write((ushort)1);
        }
        payload.Position = 0;
        var node = new CPlugShader { SortCustom = true, SortPosition = CPlugShader.ESortPosition.BeforeAlpha01 };
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader)) new CPlugShader.Chunk09002014().ReadWrite(node, rw);
        await Assert.That(node.RequirementFlags).IsEqualTo(0xFEDCBA9800040000UL);
    }

    [Test]
    public async Task ReadWrite_SignedSortingIndexAndGeometryColor_UseIndependentFlagBits()
    {
        var node = new CPlugShader { RequirementFlags = 0xFEDCBA9800000000UL, RequirementFlags2 = 0 };
        node.SortIndexValue = -63;
        node.GReqColor0 = true;
        await Assert.That(node.SortIndexValue).IsEqualTo(-63);
        await Assert.That(node.RequirementFlags >> 32).IsEqualTo(0xFEDCBA98UL);
        await Assert.That(node.RequirementFlags2).IsEqualTo(0x200u);
    }

    private static void WritePayload(GbxWriter writer, int offset)
    {
        if (offset == 7 || offset == 14)
        {
            writer.Write(-1);
            writer.Write(0);
            writer.Write(-1);
            if (offset == 14) writer.Write(0);
            return;
        }
        if (offset is 0 or 8 or 9 or 10 or 12 or 24 or 29) writer.Write(0xF1234567u);
        else if (offset is 5 or 11 or 13 or 15 or >= 16 and <= 22) writer.Write(0xFEDCBA9800040000UL);
        if (offset is 0 or 11 or 12 or 13 or 15 or >= 16 and <= 22) writer.Write(1.25f);
        if (offset is 12 or 13 or 15 or >= 16 and <= 22) writer.Write(-1);
        if (offset is >= 16 and <= 22) writer.Write((ushort)0xFEDC);
        if (offset == 25) writer.Write(0);
        if (offset == 26) writer.Write(250.5f);
        if (offset == 27)
        {
            writer.Write(0);
            writer.Write(250.5f);
            writer.Write(0.25f);
        }
        if (offset == 28)
        {
            writer.Write(1);
            writer.Write(-1);
            writer.Write(-1);
        }
        if (offset == 30)
        {
            writer.Write(1);
            writer.Write(0xF1234567u);
        }
        if (offset == 31)
        {
            writer.Write(0);
            writer.Write(0);
        }
        if (offset == 32)
        {
            writer.Write(3);
            writer.Write(0xFEDCBA9876543210UL);
            writer.Write(0xF1234567u);
            writer.Write(1.25f);
            writer.Write(-1);
            writer.Write((ushort)0xFEDC);
        }
    }

    private static Chunk<CPlugShader> CreateChunk(int offset) => (Chunk<CPlugShader>)Activator.CreateInstance(
        typeof(CPlugShader).GetNestedType($"Chunk09002{offset:X3}")!)!;
}
