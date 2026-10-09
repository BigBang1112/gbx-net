using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Game;

[Category("Unit")]
public class CGameCtnSolidDecalsLayoutTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyParametersKeepDiscardedValuesAndFourByteBooleans(bool enabled)
    {
        var payload = Payload(w =>
        {
            w.Write(0x03121000u);
            w.Write(0xFEDCBA98u);
            w.Write(0x87654321u);
            w.Write(enabled ? 1 : 0);
            w.Write(3); // Id table version.
            w.Write(uint.MaxValue); // Empty TypeId.
            w.Write(0xF1234567u); // Unsigned TypeIntensity.
            w.Write(0xFACADE01u);
        });
        var node = new CGameCtnSolidDecals();

        await RoundTrip(payload, node);
        await Assert.That(node.TypeId).IsEqualTo("");
        await Assert.That(node.TypeIntensity).IsEqualTo(0xF1234567u);
        await Assert.That(node.DecalFrequency).IsEqualTo(1u);
        await Assert.That(node.Name).IsEqualTo("Unnamed");
        var chunk = (CGameCtnSolidDecals.Chunk03121000)node.Chunks.Single();
        await Assert.That(chunk.U01).IsEqualTo(0xFEDCBA98u);
        await Assert.That(chunk.U02).IsEqualTo(0x87654321u);
        await Assert.That(chunk.U03).IsEqualTo(enabled);
        await Assert.That(chunk.GameVersion).IsEqualTo(GameVersion.Unspecified);
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    [Arguments(3, true)]
    public async Task ModernChunksKeepOpaqueSceneDataAndUnsignedParameters(int version, bool hasData)
    {
        byte[] sceneDecals = hasData ? [2, 0, 0, 0, 0xFF, 0x80, 0, 0x7F, 0xFE] : [];
        var payload = Payload(w =>
        {
            w.Write(0x03121001u);
            w.Write(version);
            w.Write(sceneDecals.Length);
            w.Write(sceneDecals);
            w.Write(0x03121002u);
            String(w, "Decals é");
            w.Write(0x03121003u);
            w.Write(3); // Id table version.
            w.Write(0x40000000u); // New TypeId string.
            String(w, "Damage");
            w.Write(uint.MaxValue);
            w.Write(0x03121004u);
            w.Write(0x80000001u);
            w.Write(0xFACADE01u);
        });
        var node = new CGameCtnSolidDecals();
        await Assert.That(node.SceneDecals).IsEmpty();
        await Assert.That(node.TypeId).IsEqualTo("");
        await Assert.That(node.TypeIntensity).IsEqualTo(1u);
        await Assert.That(node.DecalFrequency).IsEqualTo(1u);
        await Assert.That(new CGameCtnSolidDecals.Chunk03121001(GameVersion.TM2020).Version).IsEqualTo(2);

        await RoundTrip(payload, node);
        await Assert.That(node.SceneDecals).IsEquivalentTo(sceneDecals, CollectionOrdering.Matching);
        await Assert.That(node.Name).IsEqualTo("Decals é");
        await Assert.That(node.TypeId).IsEqualTo("Damage");
        await Assert.That(node.TypeIntensity).IsEqualTo(uint.MaxValue);
        await Assert.That(node.DecalFrequency).IsEqualTo(0x80000001u);
        await Assert.That(((CGameCtnSolidDecals.Chunk03121001)node.Chunks.First()).Version).IsEqualTo(version);
        await Assert.That(node.Chunks).All(x => x.GameVersion == (GameVersion.MP3 | GameVersion.TMT | GameVersion.MP4 | GameVersion.TM2020));

        if (hasData)
        {
#pragma warning disable GBXNET10001
            var clone = (CGameCtnSolidDecals)node.DeepClone();
#pragma warning restore GBXNET10001
            await Assert.That(clone.SceneDecals).IsNotSameReferenceAs(node.SceneDecals);
            clone.SceneDecals[0] = 0xFF;
            await Assert.That(node.SceneDecals[0]).IsEqualTo((byte)2);
        }
    }

    private static void String(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    private static byte[] Payload(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, CGameCtnSolidDecals node)
    {
        const uint followingWord = 0xDEADBEEF;
        using var input = new MemoryStream();
        input.Write(payload);
        using var suffixWriter = new BinaryWriter(input, Encoding.UTF8, leaveOpen: true);
        suffixWriter.Write(followingWord);
        input.Position = 0;
        using var reader = new GbxReader(input);
        using var readWrite = new GbxReaderWriter(reader);
        node.Read(readWrite);
        await Assert.That(reader.ReadUInt32()).IsEqualTo(followingWord);
        await Assert.That(input.Position).IsEqualTo(input.Length);

        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using var writeRead = new GbxReaderWriter(writer);
        node.Write(writeRead);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
