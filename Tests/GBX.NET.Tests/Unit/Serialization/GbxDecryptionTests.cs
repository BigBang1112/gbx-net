using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Hms;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class GbxDecryptionTests
{
    [Test]
    public async Task Initialize_UsesNativeParentIds()
    {
        var initializer = new RecordingInitializer();
        using var stream = new MemoryStream();
        using var reader = new GbxReader(stream, new GbxReadSettings { EncryptionInitializer = initializer });

        reader.TryInitializeDecryption(new CGameItemModel());
        reader.TryInitializeDecryption(new CHmsLight());

        await Assert.That(initializer.ClassIds).IsEquivalentTo(new uint[] { 0x2400A000, 0x06007000 }, CollectionOrdering.Matching);
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
