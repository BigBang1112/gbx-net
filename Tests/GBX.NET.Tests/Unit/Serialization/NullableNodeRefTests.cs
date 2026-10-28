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
    public async Task Node_AbsentReference_PreservesNullStorageAndWritesTheNullMarker()
    {
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using IGbxReaderWriter rw = new GbxReaderWriter(writer);
        NodeRef<CPlugMaterialUserInst>? reference = null;

        rw.Node(ref reference);

        await Assert.That(reference).IsNull();
        await Assert.That(output.ToArray()).IsEquivalentTo(BitConverter.GetBytes(-1), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Node_ReadingDirectNode_PreservesHandleIdentityAndClearsExternalMetadata(bool existing)
    {
        using var input = new MemoryStream();
        using (var inputWriter = new GbxWriter(input)) inputWriter.WriteNode(new CPlugMaterialUserInst());
        input.Position = 0;
        using var output = new MemoryStream();
        using var reader = new GbxReader(input);
        using var writer = new GbxWriter(output);
        using IGbxReaderWriter rw = new GbxReaderWriter(reader, writer);
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        NodeRef<CPlugMaterialUserInst>? reference = existing ? new() { Descriptor = table.CreateFile("unloaded.Gbx") } : null;
        var captured = reference;

        rw.Node(ref reference);

        await Assert.That(reference).IsNotNull();
        if (existing) await Assert.That(reference).IsSameReferenceAs(captured);
        await Assert.That(reference!.Descriptor).IsNull();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.Embedded);
        await Assert.That(reference.Node).IsNotNull();
        await Assert.That(calls).IsEqualTo(0);
        await Assert.That(input.Position).IsEqualTo(input.Length);
        await Assert.That(output.ToArray()).IsEquivalentTo(input.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Node_WritingExternalReference_UsesOnlyTheCachedNodeAndPreservesMetadata(bool resolved)
    {
        var table = new GbxRefTable();
        var node = new CPlugMaterialUserInst();
        var calls = 0;
        table.ExternalNodes["node.Gbx"] = () => { calls++; return node.ToGbx(); };
        var descriptor = table.CreateFile("node.Gbx");
        NodeRef<CPlugMaterialUserInst>? reference = new() { Descriptor = descriptor };
        if (resolved) reference.Resolve();
        var captured = reference;
        using var output = new MemoryStream();
        using var writer = new GbxWriter(output);
        using IGbxReaderWriter rw = new GbxReaderWriter(writer);
        using var expected = new MemoryStream();
        using (var expectedWriter = new GbxWriter(expected)) expectedWriter.WriteNode(resolved ? node : null);

        rw.Node(ref reference);

        await Assert.That(reference).IsSameReferenceAs(captured);
        await Assert.That(reference!.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(reference.State).IsEqualTo(resolved ? NodeRefState.Resolved : NodeRefState.Unresolved);
        await Assert.That(calls).IsEqualTo(resolved ? 1 : 0);
        await Assert.That(output.ToArray()).IsEquivalentTo(expected.ToArray(), CollectionOrdering.Matching);
    }

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
        reference.Node = new CPlugCrystal();
        var captured = reference;

        reader.ReadNodeRef(ref reference);

        await Assert.That(reference).IsSameReferenceAs(captured);
        await Assert.That(captured.Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(captured.CachedNode).IsNull();
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
        var owner = new CGameItemModel();
        owner.EntityModelEdition = null;
        _ = owner.EntityModelEdition;
        var before = GC.GetAllocatedBytesForCurrentThread();
        owner.EntityModelEdition = null;
        var node = owner.EntityModelEdition;
        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        await Assert.That(node).IsNull();
        await Assert.That(allocated).IsEqualTo(0);
    }

    [Test]
    public async Task NodeMetadata_ConcurrentFirstAccess_ReturnsOneStableHandle()
    {
        var owner = new CGameItemModel();
        var handles = await Task.WhenAll(Enumerable.Range(0, 32).Select(_ => Task.Run(() => owner.References.EntityModelEdition)));

        foreach (var handle in handles)
            await Assert.That(handle).IsSameReferenceAs(handles[0]);
        var node = new CPlugCrystal();
        owner.EntityModelEdition = node;
        await Assert.That(handles[0].CachedNode).IsSameReferenceAs(node);
        owner.EntityModelEdition = null;
        await Assert.That(owner.References.EntityModelEdition).IsSameReferenceAs(handles[0]);
        await Assert.That(handles[0].Kind).IsEqualTo(NodeRefKind.None);
    }
}
