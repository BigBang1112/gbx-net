using GBX.NET.Components;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class NullableNodeRefTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadNodeRef_NullReference_LeavesAbsentStorageNull(bool encapsulated)
    {
        using var stream = new MemoryStream(BitConverter.GetBytes(-1));
        using var reader = new GbxReader(stream);
        using var encapsulation = encapsulated ? new Encapsulation(reader) : null;
        NodeRef<CMwNod>? reference = null;

        ((IGbxReader)reader).ReadNodeRef(ref reference);

        await Assert.That(reference).IsNull();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task ReadNodeRef_NullReference_ClearsAnExistingHandleWithoutReplacingIt()
    {
        using var stream = new MemoryStream(BitConverter.GetBytes(-1));
        using var reader = new GbxReader(stream);
        NodeRef<CMwNod>? reference = new();
        reference.SetEmbedded(new CPlugCrystal());
        var captured = reference;

        reader.ReadNodeRef(ref reference);

        await Assert.That(reference).IsSameReferenceAs(captured);
        await Assert.That(captured.Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(captured.CachedTarget).IsNull();
    }

    [Test]
    public async Task ReadNodeRef_ExternalReference_AllocatesStorageAndPreservesTheDescriptor()
    {
        var file = new GbxRefTable().CreateFile("unloaded.Gbx");
        using var stream = new MemoryStream(BitConverter.GetBytes(1));
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
        NodeRef<CMwNod>? reference = null;

        reader.ReadNodeRef(ref reference);

        await Assert.That(reference!.Descriptor).IsSameReferenceAs(file);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
    }

    [Test]
    public async Task ReadWriteNodeRef_AbsentReference_PreservesNullAndWritesTheNullMarker()
    {
        using var input = new MemoryStream(BitConverter.GetBytes(-1));
        using var output = new MemoryStream();
        using var reader = new GbxReader(input);
        using var writer = new GbxWriter(output);
        using IGbxReaderWriter rw = new GbxReaderWriter(reader, writer);
        NodeRef<CMwNod>? reference = null;

        rw.NodeRef(ref reference);

        await Assert.That(reference).IsNull();
        await Assert.That(output.ToArray()).IsEquivalentTo(BitConverter.GetBytes(-1), CollectionOrdering.Matching);
    }

    [Test]
    public async Task NodeProperties_EmptyGettersAndSetters_DoNotAllocateReferenceHandles()
    {
        var node = new CGameItemModel();
        node.EntityModelEdition = null;
        _ = node.EntityModelEdition;
        var before = GC.GetAllocatedBytesForCurrentThread();
        node.EntityModelEdition = null;
        var target = node.EntityModelEdition;
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        await Assert.That(target).IsNull();
        await Assert.That(allocated).IsEqualTo(0);
    }

    [Test]
    public async Task NodeMetadata_ConcurrentFirstAccess_ReturnsOneStableHandle()
    {
        var node = new CGameItemModel();
        var handles = await Task.WhenAll(Enumerable.Range(0, 32).Select(_ => Task.Run(() => node.References.EntityModelEdition)));

        foreach (var handle in handles)
            await Assert.That(handle).IsSameReferenceAs(handles[0]);
        var target = new CPlugCrystal();
        node.EntityModelEdition = target;
        await Assert.That(handles[0].CachedTarget).IsSameReferenceAs(target);
        node.EntityModelEdition = null;
        await Assert.That(node.References.EntityModelEdition).IsSameReferenceAs(handles[0]);
        await Assert.That(handles[0].Kind).IsEqualTo(NodeRefKind.None);
    }
}
