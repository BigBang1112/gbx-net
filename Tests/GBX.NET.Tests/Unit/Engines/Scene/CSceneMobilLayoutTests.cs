using GBX.NET.Engines.Control;
using GBX.NET.Engines.Scene;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Engines.Scene;

[Category("Unit")]
public class CSceneMobilLayoutTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task ReadWrite_Item_ControlsUsePresenceFlagBeforeInlineArchive(bool isControl, bool hasItem)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (isControl) writer.Write(hasItem);
            if (!isControl || hasItem)
            {
                writer.Write(0x06003011U);
                writer.Write(0UL);
                writer.Write((ushort)1);
                writer.Write(0xFACADE01U);
            }
            writer.Write(0x11223344);
        }
        payload.Position = 0;
        CSceneMobil node = isControl ? new CControlFrame() : new CSceneMobil();
        var initializer = new RecordingInitializer();
        var chunk = new CSceneMobil.Chunk0A011005();
        using (var reader = new GbxReader(payload, new GbxReadSettings { EncryptionInitializer = initializer }))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        await Assert.That(initializer.ClassIds.Count).IsEqualTo(!isControl || hasItem ? 1 : 0);
        if (node is CControlBase control)
        {
            await Assert.That(control.HasHmsItem).IsEqualTo(hasItem);
        }
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray()[..^4], CollectionOrdering.Matching);
    }

    private sealed class RecordingInitializer : IEncryptionInitializer
    {
        public List<uint> ClassIds { get; } = [];

        public void Initialize(byte[] data, uint offset, uint count)
        {
            ClassIds.Add(BitConverter.ToUInt32(data, (int)offset));
        }
    }
}
