using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugMaterialColorTargetTableLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task LegacyVersions_PreserveClassicAndStuntPalettes(int version)
    {
        var palettes = Enumerable.Range(0, 4).Select(i => Palette(i)).ToArray();
        var payload = Payload(w =>
        {
            w.Write(version);
            WritePalette(w, palettes[0]);
            if (version >= 1) WritePalette(w, palettes[1]);
            if (version >= 2)
            {
                WritePalette(w, palettes[2]);
                WritePalette(w, palettes[3]);
            }
        });
        var node = new CPlugMaterialColorTargetTable();

        await RoundTrip(payload, node);
        await Assert.That(node.LegacyClassicColors).IsEquivalentTo(palettes[0], CollectionOrdering.Matching);
        if (version >= 1)
            await Assert.That(node.LegacyClassicColorBlindColors).IsEquivalentTo(palettes[1], CollectionOrdering.Matching);
        if (version >= 2)
        {
            await Assert.That(node.LegacyStuntColors).IsEquivalentTo(palettes[2], CollectionOrdering.Matching);
            await Assert.That(node.LegacyStuntColorBlindColors).IsEquivalentTo(palettes[3], CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(3, 0)]
    [Arguments(3, 1)]
    [Arguments(3, 13)]
    [Arguments(4, 13)]
    public async Task ModernVersions_InterleaveStandardAndColorBlindPalettes(int version, int count)
    {
        var payload = Payload(w =>
        {
            w.Write(version);
            w.Write(count);
            for (var i = 0; i < count; i++)
            {
                WritePalette(w, Palette(i * 2));
                WritePalette(w, Palette(i * 2 + 1));
            }
        });
        var node = new CPlugMaterialColorTargetTable();

        await RoundTrip(payload, node);
        await Assert.That(node.ColorTargets!.Length).IsEqualTo(count);
        for (var i = 0; i < count; i++)
        {
            await Assert.That(node.ColorTargets[i].Colors).IsEquivalentTo(Palette(i * 2), CollectionOrdering.Matching);
            await Assert.That(node.ColorTargets[i].ColorBlindColors).IsEquivalentTo(Palette(i * 2 + 1), CollectionOrdering.Matching);
        }
    }

    [Test]
    public async Task Defaults_WriteThirteenEmptyPalettePairs()
    {
        var node = new CPlugMaterialColorTargetTable();
        var expected = Payload(w =>
        {
            w.Write(3);
            w.Write(13);
            for (var i = 0; i < 26; i++) w.Write(0);
        });
        var actual = Payload(w =>
        {
            using var rw = new GbxReaderWriter(w);
            node.ReadWrite(rw);
        });

        await Assert.That(actual).IsEquivalentTo(expected, CollectionOrdering.Matching);
        await Assert.That(Enum.GetNames<CPlugMaterialColorTargetTable.EColorTarget>()).IsEquivalentTo(
            new[] { "Classic", "Stunt", "Red", "Orange", "Yellow", "Lime", "Green", "Cyan", "Blue", "Purple", "Pink", "White", "Black" },
            CollectionOrdering.Matching);
        await Assert.That(ReferenceEquals(node.ColorTargets![0], node.ColorTargets[1])).IsFalse();
    }

    [Test]
    [Arguments(-1)]
    [Arguments(14)]
    public void Read_RejectsTargetCountsOutsideNativeCapacity(int count)
    {
        using var stream = new MemoryStream(Payload(w => { w.Write(3); w.Write(count); }));
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);

        Assert.Throws<InvalidDataException>(() => new CPlugMaterialColorTargetTable().ReadWrite(rw));
    }

    [Test]
    [Arguments(2, 0)]
    [Arguments(2, 1)]
    [Arguments(2, 2)]
    [Arguments(2, 3)]
    [Arguments(3, 0)]
    [Arguments(3, 1)]
    public void Read_RejectsOversizedPalettesInEveryPosition(int version, int paletteIndex)
    {
        using var stream = new MemoryStream(Payload(w =>
        {
            w.Write(version);
            if (version >= 3) w.Write(1);
            for (var i = 0; i < paletteIndex; i++) w.Write(0);
            w.Write(9);
        }));
        using var reader = new GbxReader(stream);
        using var rw = new GbxReaderWriter(reader);

        Assert.Throws<InvalidDataException>(() => new CPlugMaterialColorTargetTable().ReadWrite(rw));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public void Write_RejectsOversizedPalettes(bool colorBlind)
    {
        var pair = new CPlugMaterialColorTargetTable.ColorPair();
        if (colorBlind) pair.ColorBlindColors = new int[9];
        else pair.Colors = new int[9];
        var node = new CPlugMaterialColorTargetTable { ColorTargets = [pair] };
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var rw = new GbxReaderWriter(writer);

        Assert.Throws<InvalidDataException>(() => node.ReadWrite(rw));
    }

    private static int[] Palette(int seed) => Enumerable.Range(0, 8)
        .Select(i => unchecked((int)(0xFF000000u | (uint)(seed * 16 + i))))
        .ToArray();

    private static void WritePalette(GbxWriter writer, int[] colors)
    {
        writer.Write(colors.Length);
        foreach (var color in colors) writer.Write(color);
    }

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, CPlugMaterialColorTargetTable node)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var writer = new GbxWriter(input)) writer.Write(0x11223344);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            node.ReadWrite(rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        var output = Payload(w =>
        {
            using var rw = new GbxReaderWriter(w);
            node.ReadWrite(rw);
        });
        await Assert.That(output).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
