using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Exceptions;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugFileGenLayoutTests
{
    private const int Sentinel = 0x12345678;

    [Test]
    public async Task ReadableWritable_ArchiveInterfaces_IncludesVersionPrefix()
    {
        var original = new CPlugFileGen
        {
            Version = 6,
            GenKind = (int)CPlugFileGen.EGenKind.Pixels,
            IntParams = [2, 2, 4, 0, 1],
            PixelData = Enumerable.Range(0, 16).Select(i => (byte)i).ToArray()
        };
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        using (var rw = new GbxReaderWriter(writer))
        {
            rw.ReadableWritable(original);
            writer.Write(Sentinel);
        }

        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var restored = reader.ReadReadable<CPlugFileGen>();
        await Assert.That(restored.Version).IsEqualTo(6);
        await Assert.That(restored.PixelData).IsEquivalentTo(original.PixelData, CollectionOrdering.Matching);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        using var secondSave = new MemoryStream();
        using (var writer = new GbxWriter(secondSave))
        {
            restored.Write(writer);
            writer.Write(Sentinel);
        }
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
    public async Task ReadWrite_VersionedArchive_ReadsScriptInlineCurvesAndImageReferences(int version)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteParameters(writer, version, 38, [16, 32, 4]);
            writer.Write(Sentinel);
        }

        var node = await Read(payload);
        await Assert.That(node.Version).IsEqualTo(version);
        await Assert.That(node.GenKind).IsEqualTo((int)CPlugFileGen.EGenKind.Script);
        await Assert.That(node.ColorParams!.Single()).IsEqualTo(new Vec4(0.1f, 0.2f, 0.3f, 0.4f));
        await Assert.That(node.RealParams).IsEquivalentTo(new[] { 0.25f, 0.75f }, CollectionOrdering.Matching);
        if (version >= 3) await Assert.That(node.Script).IsEqualTo("return Color;");
        else await Assert.That(node.Script).IsNull();
        if (version >= 4)
        {
            var curve = node.Functions!.Single();
            await Assert.That(curve.Xs).IsEquivalentTo(new[] { 0f, 1f }, CollectionOrdering.Matching);
            await Assert.That(curve.Ys).IsEquivalentTo(new[] { 0.25f, 0.75f }, CollectionOrdering.Matching);
            await Assert.That(curve.RealInterp).IsEqualTo(CFuncKeysReal.ERealInterp.Linear);
        }
        else await Assert.That(node.Functions).IsNull();
        if (version >= 6) await Assert.That(node.InputImages!.Single()).IsNull();
        else await Assert.That(node.InputImages).IsNull();

        await RoundTrip(node);
    }

    [Test]
    [Arguments(-1, 5, 0, 3)]
    [Arguments(-1, 5, 1, 4)]
    [Arguments(-1, 6, 0, 3)]
    [Arguments(-1, 6, 1, 4)]
    [Arguments(0, 5, 0, 3)]
    [Arguments(0, 6, 1, 4)]
    public async Task ReadWrite_LegacyRenderParameters_AreConvertedOnceAndSaveWithVersionMarker(int version, int kind, int flag, int expected)
    {
        var parameters = new[] { 16, 32, 0 };
        var index = kind == 5 ? 2 : 1;
        parameters[index] = flag;
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteParameters(writer, version, kind, parameters);
            writer.Write(Sentinel);
        }

        var node = await Read(payload);
        await Assert.That(node.Version).IsEqualTo(0);
        await Assert.That(node.IntParams![index]).IsEqualTo(expected);
        await RoundTrip(node);
        await Assert.That(node.IntParams[index]).IsEqualTo(expected);
        await Assert.That(node.Version).IsEqualTo(2);
    }

    public static IEnumerable<(int Version, Func<int[]> Parameters, int ByteCount)> PixelPayloads()
    {
        yield return (-1, () => [2, 2, 4], 16);
        yield return (0, () => [2, 2, 4], 16);
        yield return (1, () => [2, 2, 4, 0, 0], 16); // Version 1 inverts the mip-count flag.
        yield return (1, () => [2, 2, 4, 0, 1], 20);
        yield return (2, () => [2, 2, 4, 0, 1], 16);
        yield return (5, () => [4, 2, 3, 0, 0], 33); // Full mip chain: 8 + 2 + 1 RGB pixels.
        yield return (5, () => [4, 4, 4, 1, 1], 8); // One DXT1 block.
        yield return (5, () => [5, 3, 4, 2, 1], 32); // Two DXT3 blocks with padded edges.
        yield return (5, () => [4, 4, 4, 1, 0], 24); // DXT1 retains one block per mip level.
        yield return (5, () => [2, 2, 2, 6, 1], 16); // Two 16-bit components per pixel.
        yield return (5, () => [2, 2, 4, 8, 1], 64); // Four 32-bit components per pixel.
        yield return (5, () => [2, 2, 3, 15, 1], 16); // Packed pixels have a fixed byte width.
        yield return (5, () => [2, 2, 2, 4, 0, 1], 32); // 3D RGBA image.
        yield return (6, () => [2, 2, 4, 0, 1], 16);
    }

    [Test]
    [MethodDataSource(nameof(PixelPayloads))]
    public async Task ReadWrite_PixelArchive_ConsumesRawDataWithoutLengthPrefix(int version, Func<int[]> createParameters, int byteCount)
    {
        var parameters = createParameters();
        var pixels = Enumerable.Range(0, byteCount).Select(i => (byte)i).ToArray();
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteParameters(writer, version, 9, parameters);
            writer.Write(pixels);
            writer.Write(Sentinel);
        }

        var node = await Read(payload);
        await Assert.That(node.PixelData).IsEquivalentTo(pixels, CollectionOrdering.Matching);
        if (version == 1) await Assert.That(node.IntParams![4]).IsEqualTo(parameters[4] == 0 ? 1 : 0);
        await RoundTrip(node);
    }

    [Test]
    public async Task ReadWrite_UnsupportedVersion_IsRejectedBeforeReadingParameters()
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload)) writer.Write(int.MinValue | 7);
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        using var rw = new GbxReaderWriter(reader);
        Assert.Throws<VersionNotSupportedException>(() => new CPlugFileGen().ReadWrite(rw));
        await Assert.That(payload.Position).IsEqualTo(4L);
    }

    private static void WriteParameters(GbxWriter writer, int version, int kind, int[] parameters)
    {
        writer.Write(version < 0 ? kind : int.MinValue | version);
        if (version >= 0) writer.Write(kind);
        writer.Write(parameters.Length);
        foreach (var value in parameters) writer.Write(value);
        writer.Write(1);
        writer.Write(new Vec4(0.1f, 0.2f, 0.3f, 0.4f));
        if (version >= 0)
        {
            writer.Write(2);
            writer.Write(0.25f);
            writer.Write(0.75f);
        }
        if (version >= 3) writer.Write("return Color;");
        if (version >= 4)
        {
            writer.Write(1); // Curve count, followed directly by CFuncKeysReal chunks.
            writer.Write(0x05002001u);
            writer.Write(2);
            writer.Write(0f);
            writer.Write(1f);
            writer.Write(0x0501A001u);
            writer.Write(2);
            writer.Write(0.25f);
            writer.Write(0.75f);
            writer.Write(1); // Linear interpolation.
            writer.Write(0xFACADE01u);
        }
        if (version >= 6)
        {
            writer.Write(1); // Image count, followed by node reference indices.
            writer.Write(-1);
        }
    }

    private static async Task<CPlugFileGen> Read(MemoryStream payload)
    {
        payload.Position = 0;
        using var reader = new GbxReader(payload, new GbxReadSettings { SafeSkippableChunks = false });
        using var rw = new GbxReaderWriter(reader);
        var node = new CPlugFileGen();
        node.ReadWrite(rw);
        await Assert.That(reader.ReadInt32()).IsEqualTo(Sentinel);
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        return node;
    }

    private static async Task RoundTrip(CPlugFileGen node)
    {
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) node.ReadWrite(rw);
        var bytes = saved.ToArray();
        saved.Position = 0;
        using var reader = new GbxReader(saved, new GbxReadSettings { SafeSkippableChunks = false });
        await Assert.That(reader.ReadInt32()).IsEqualTo(int.MinValue | node.Version);
        saved.Position = 0;
        var restored = reader.ReadNode<CPlugFileGen>()!;
        await Assert.That(saved.Position).IsEqualTo(saved.Length);
        await Assert.That(restored.Version).IsEqualTo(node.Version);
        await Assert.That(restored.GenKind).IsEqualTo(node.GenKind);
        await Assert.That(restored.IntParams).IsEquivalentTo(node.IntParams!, CollectionOrdering.Matching);
        if (node.PixelData is not null)
        {
            await Assert.That(restored.PixelData).IsEquivalentTo(node.PixelData, CollectionOrdering.Matching);
        }
        else await Assert.That(restored.PixelData).IsNull();
        using var secondSave = new MemoryStream();
        using (var writer = new GbxWriter(secondSave)) writer.WriteNode(restored);
        await Assert.That(secondSave.ToArray()).IsEquivalentTo(bytes, CollectionOrdering.Matching);
    }
}
