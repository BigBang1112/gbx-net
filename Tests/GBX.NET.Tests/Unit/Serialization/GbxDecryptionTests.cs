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
        reader.TryInitializeDecryption(new CHmsItem());
        reader.TryInitializeDecryption(new CHmsZoneDynamic());

        await Assert.That(initializer.ClassIds).IsEquivalentTo(new uint[] { 0x2400A000, 0x06007000, 0x01001000, 0x06004000 }, CollectionOrdering.Matching);
    }

    [Test]
    public async Task Initialize_ClassAwareInitializer_ReceivesClassAndNativeParent()
    {
        var initializer = new ClassRecordingInitializer();
        using var stream = new MemoryStream();
        using var reader = new GbxReader(stream, new GbxReadSettings { EncryptionInitializer = initializer });

        reader.TryInitializeDecryption(new CGameItemModel());

        await Assert.That(initializer.ClassId).IsEqualTo(0x2E002000U);
        await Assert.That(initializer.ParentClassId).IsEqualTo(0x2400A000U);
    }

    private sealed class ClassRecordingInitializer : IClassEncryptionInitializer
    {
        public uint ClassId { get; private set; }
        public uint ParentClassId { get; private set; }

        public void InitializeClass(uint classId, uint parentClassId)
        {
            ClassId = classId;
            ParentClassId = parentClassId;
        }

        public void Initialize(byte[] data, uint offset, uint count) => throw new InvalidOperationException("Class context was not provided.");
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
