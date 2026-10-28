using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class TM2020PakLayoutTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    public async Task AnimationFile_BeforeVersion20UsesGraphVersionForVariables()
    {
        var payload = Payload(w =>
        {
            w.Write(19);
            w.Write(63);
            // Seven resource arrays, spot version and array, three strings, graphs and class ids.
            for (var i = 0; i < 14; i++) w.Write(0);
        });
        var node = new CPlugAnimFile();
        var chunk = new CPlugAnimFile.Chunk090B0003();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.Version).IsEqualTo(19);
        await Assert.That(node.VariableVersion).IsEqualTo(63);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task AnimationGraphArrays_UseNodeVariableVersionAndPreserveBoundary(int framing)
    {
        var payload = Payload(w =>
        {
            if (framing == 0) w.Write(2);
            if (framing == 1) w.Write((byte)2);
            for (var i = 0; i < 2; i++)
            {
                w.Write(16); // Base-only graph node.
                if (i == 0) w.Write(3); // Shared id format version.
                w.Write(-1);
                w.Write(1f); w.Write(2f);
                w.Write(3);
                w.Write((byte)4); w.Write((byte)5);
                w.Write(6 + i);
            }
        });
        var node = new CPlugAnimFile { VariableVersion = 55 };
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        var values = framing == 2
            ? reader.ReadArrayReadable<CPlugAnimFile.GraphNode, CPlugAnimFile>(node, length: 2, version: 51)
            : reader.ReadArrayReadable<CPlugAnimFile.GraphNode, CPlugAnimFile>(node, byteLengthPrefix: framing == 1, version: 51);
        await Assert.That(values.Length).IsEqualTo(2);
        await Assert.That(values[1].Base!.U06).IsEqualTo(7);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        if (framing == 2) writer.WriteArrayWritable(values, node, length: 2, version: 51);
        else writer.WriteArrayWritable(values, node, byteLengthPrefix: framing == 1, version: 51);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    public async Task MaterialBuffer_PreservesAbsentDataSizeAndBoundary(int version, bool absent)
    {
        var payload = Payload(w =>
        {
            w.Write(7);
            w.Write(3);
            if (version >= 1) w.Write(absent ? 1 : 0);
            if (version == 0 || !absent) w.Write(new byte[] { 4, 5, 6 });
        });
        var buffer = new CPlugMaterialCustom.CBuffer { IsDataAbsent = absent };

        await Verify(payload, rw => buffer.ReadWrite(rw, version));
        await Assert.That(buffer.DataSize).IsEqualTo(3);
        await Assert.That(buffer.IsDataAbsent).IsEqualTo(absent);
        await Assert.That(buffer.U02?.Length ?? 0).IsEqualTo(version >= 1 && absent ? 0 : 3);
    }

    [Test]
    public async Task MaterialColorTable_ReadsVersionedColorPairsWithoutChunkFraming()
    {
        var payload = Payload(w =>
        {
            w.Write(3);
            w.Write(1);
            w.Write(2);
            w.Write(0x11223344);
            w.Write(0x55667788);
            w.Write(1);
            w.Write(0x12345678);
        });
        var table = new CPlugMaterialColorTargetTable();

        await Verify(payload, rw => table.ReadWrite(rw));
        await Assert.That(table.Colors!.Length).IsEqualTo(1);
        await Assert.That(table.Colors[0].Colors1![1]).IsEqualTo(0x55667788);
        await Assert.That(table.Colors[0].Colors2![0]).IsEqualTo(0x12345678);
    }

    [Test]
    public async Task PreLightGen_UvRecordsOccupyTwentyBytes()
    {
        var payload = Payload(w =>
        {
            w.Write(1);
            w.Write(2);
            w.Write(3f);
            w.Write(1);
            for (var i = 0; i < 8; i++) w.Write(i + 0.5f);
            w.Write(9);
            w.Write(10);
            w.Write(0);
            w.Write(1);
            for (var i = 1; i <= 5; i++) w.Write(i);
        });
        var data = new CPlugSolid2Model.PreLightGen();

        await Verify(payload, rw => data.ReadWrite(rw));
        await Assert.That(data.UvGroups![0].U01).IsEqualTo(5);
    }

    [Test]
    public async Task Solid2Model_FiveIntegerRecordsExcludeTuplePadding()
    {
        var payload = Payload(w =>
        {
            w.Write(34);
            w.Write(3); w.Write(-1); // Id version and empty id.
            w.Write(0); // Shaded geometries.
            w.Write(10); w.Write(0); // Deprecated visual list.
            w.Write(0); // Material ids.
            w.Write(0); // Custom material count.
            w.Write(10); w.Write(0); // Deprecated material list.
            w.Write(-1); // Skeleton.
            w.Write(0); // Lod distances.
            w.Write(0); // Visual type.
            w.Write(0); // No prelight generator.
            w.Write(0L); // File time.
            for (var i = 0; i < 6; i++) w.Write(0); // Three strings and three light arrays.
            w.Write(0); // Damage zone.
            w.Write(0); // Flags.
            w.Write(1); // Legacy occlusion flag.
            w.Write(0); // Legacy string.
            w.Write(-1); // Material setting.
            w.Write(0); // Joint ids.
            w.Write(0); // U10.
            w.Write(0); // U11.
            w.Write(0); // U12.
            w.Write(0); // U13.
            w.Write(-1); // U14.
            w.Write(1f); w.Write(1f);
            w.Write(-1); // U17 id.
            w.Write(0); // U18.
            w.Write(1); // Five-integer record count.
            for (var i = 1; i <= 5; i++) w.Write(i);
        });
        var node = new CPlugSolid2Model();
        var chunk = new CPlugSolid2Model.Chunk090BB000();

        await Verify(payload, rw => chunk.ReadWrite(node, rw));
        await Assert.That(chunk.U19![0]).IsEqualTo((1, 2, 3, 4, 5));
    }

    [Test]
    public async Task EffectVariable_Version55UsesSingleByteBoolean()
    {
        var payload = Payload(w =>
        {
            w.Write(3); w.Write(-1); // Id version and empty id.
            w.Write((byte)0);
            w.Write((byte)1);
            w.Write((byte)8);
        });
        var variable = new CPlugFxSystem.Variable();

        await Verify(payload, rw => variable.ReadWrite(rw, 55));
        await Assert.That(variable.U01).IsTrue();
        await Assert.That(variable.U08).IsEqualTo((byte)8);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        write(writer);
        writer.Write(Sentinel);
        return stream.ToArray();
    }

    private static async Task Verify(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using (var rw = new GbxReaderWriter(reader)) serialize(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        writer.Write(Sentinel);
        await Assert.That(output.ToArray().SequenceEqual(payload)).IsTrue();
    }
}
