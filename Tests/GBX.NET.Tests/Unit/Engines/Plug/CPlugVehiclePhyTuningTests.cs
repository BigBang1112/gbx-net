using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using System.Text;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugVehiclePhyTuningTests
{
    [Test]
    [Arguments("", 0)]
    [Arguments("A", 0)]
    [Arguments("ABC", 0)]
    [Arguments("ABCD", 1)]
    [Arguments("Stadium", 1)]
    public async Task ReadWrite_Chunk090EB000_InitializesEncryptionOnlyWithFourCharacterPrefix(string name, int expectedCalls)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        using (var rw = new GbxReaderWriter(writer))
        {
            new CPlugVehiclePhyTuning.Chunk090EB000 { U02 = 12.5f }.ReadWrite(new() { Name = name }, rw);
        }

        stream.Position = 0;
        var initializer = new RecordingEncryptionInitializer();
        using var reader = new GbxReader(stream, new() { EncryptionInitializer = initializer });
        using var readerWriter = new GbxReaderWriter(reader);
        var node = new CPlugVehiclePhyTuning();
        var chunk = new CPlugVehiclePhyTuning.Chunk090EB000();
        chunk.ReadWrite(node, readerWriter);

        await Assert.That(node.Name).IsEqualTo(name);
        await Assert.That(chunk.U02).IsEqualTo(12.5f);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
        await Assert.That(initializer.Calls).IsEqualTo(expectedCalls);

        if (expectedCalls != 0)
        {
            await Assert.That(initializer.Data).IsEquivalentTo(Encoding.ASCII.GetBytes(name.Substring(0, 4)), CollectionOrdering.Matching);
        }
    }

    private sealed class RecordingEncryptionInitializer : IEncryptionInitializer
    {
        public int Calls { get; private set; }
        public byte[] Data { get; private set; } = [];

        public void Initialize(byte[] data, uint offset, uint count)
        {
            Calls++;
            Data = data.AsSpan((int)offset, (int)count).ToArray();
        }
    }
}
