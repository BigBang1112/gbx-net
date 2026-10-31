using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class NodeRefTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task ReadWriteNodeRef_ScalarInterfaces_PreservesExternalDescriptors(bool resource, bool combined)
    {
        var table = new GbxRefTable();
        GbxRefTableNode descriptor = resource
            ? new GbxRefTableResource(table, 4, true, 42)
            : new GbxRefTableFile(table, 0, true, "unresolved.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = descriptor;
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        if (combined)
        {
            using IGbxReaderWriter rw = new GbxReaderWriter(writer);
            rw.NodeRef(reference);
        }
        else
        {
            IGbxWriter scalarWriter = writer;
            scalarWriter.WriteNodeRef(reference);
        }

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = descriptor });
        var restored = new NodeRef<CMwNod>();
        if (combined)
        {
            using IGbxReaderWriter rw = new GbxReaderWriter(reader);
            rw.NodeRef(restored);
        }
        else
        {
            IGbxReader scalarReader = reader;
            await Assert.That(scalarReader.ReadNodeRef(restored)).IsSameReferenceAs(restored);
        }
        await Assert.That(restored.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(restored.Kind).IsEqualTo(resource ? NodeRefKind.ExternalResource : NodeRefKind.ExternalFile);
        await Assert.That(restored.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(restored.CachedNode).IsNull();
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Resolve_UnsupportedResource_PreservesDescriptor(bool asynchronous)
    {
        var resource = new GbxRefTableResource(new GbxRefTable(), 4, true, 42);
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = resource;
        if (asynchronous)
            await Assert.That(async () => await reference.ResolveAsync()).Throws<NotSupportedException>();
        else
            await Assert.That(() => reference.Resolve()).Throws<NotSupportedException>();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.ExternalResource);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Failed);
        await Assert.That(reference.Resource).IsSameReferenceAs(resource);
        await Assert.That(reference.CachedNode).IsNull();
        await Assert.That(reference.Exception).IsTypeOf<NotSupportedException>();
    }

    [Test]
    [Arguments(GbxCompression.Uncompressed, false)]
    [Arguments(GbxCompression.Uncompressed, true)]
    [Arguments(GbxCompression.Compressed, false)]
    [Arguments(GbxCompression.Compressed, true)]
    public async Task ReadWriteNodeRef_RootCycles_PreservesIdentityAndNodeIndexes(GbxCompression compression, bool asynchronous)
    {
        var root = new CGameItemModel();
        var child = new CGameItemModel { EntityModelEdition = root };
        child.CreateChunk<CGameItemModel.Chunk2E002019>().Version = 15;
        root.NadeoSkinsFids = [root, child, child, null, null];
        root.CreateChunk<CGameItemModel.Chunk2E002008>();
        var table = new GbxRefTable();
        root.NadeoSkinsFids!.Get(3).Descriptor = new GbxRefTableFile(table, 0, true, "unresolved.Gbx");
        root.NadeoSkinsFids.Get(4).Descriptor = new GbxRefTableResource(table, 4, true, 42);

        using var stream = new MemoryStream();
        root.ToGbx(GbxHeaderBasic.Create(compressionOfBody: compression)).Save(stream);
        stream.Position = 0;
        var parsed = asynchronous ? await Gbx.ParseAsync(stream) : Gbx.Parse(stream);
        var restored = (CGameItemModel)parsed.Node!;
        await Assert.That(parsed.Header.NumNodes).IsEqualTo(4);
        await Assert.That(restored.NadeoSkinsFids![0]).IsSameReferenceAs(restored);
        await Assert.That(restored.NadeoSkinsFids[1]).IsSameReferenceAs(restored.NadeoSkinsFids[2]);
        var restoredChild = (CGameItemModel)restored.NadeoSkinsFids[1]!;
        await Assert.That(restoredChild.EntityModelEdition).IsSameReferenceAs(restored);
        await Assert.That(restored.NadeoSkinsFids.Get(3).File!.FilePath).IsEqualTo("unresolved.Gbx");
        await Assert.That(restored.NadeoSkinsFids.Get(4).Resource!.ResourceIndex).IsEqualTo(42);
        await Assert.That(restored.NadeoSkinsFids.Get(3).State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(restored.NadeoSkinsFids.Get(4).State).IsEqualTo(NodeRefState.Unresolved);

        using var rewritten = new MemoryStream();
        parsed.Save(rewritten);
        await Assert.That(rewritten.ToArray()).IsEquivalentTo(stream.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWriteNodeRef_Encapsulation_StoresRegisteredRootsAsInlineNodes()
    {
        var root = new CMwNod();
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.NodeDict[root] = 0;
        using (var encapsulation = new Encapsulation(writer)) writer.WriteNodeRef(root);
        writer.WriteNodeRef(root);
        await Assert.That(stream.Length).IsEqualTo(12L);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.NodeDict[0] = root;
        var inline = new NodeRef<CMwNod>();
        using (var encapsulation = new Encapsulation(reader)) reader.ReadNodeRef(inline);
        await Assert.That(inline.CachedNode).IsNotNull();
        await Assert.That(inline.CachedNode).IsNotSameReferenceAs(root);
        await Assert.That(reader.ReadNodeRef<CMwNod>(null).CachedNode).IsSameReferenceAs(root);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadListNodeRef_ArgumentlessCall_PreservesDescriptorsAndAllowsResizing(bool deprecated)
    {
        var table = new GbxRefTable();
        var file = new GbxRefTableFile(table, 0, true, "unresolved.Gbx");
        var resource = new GbxRefTableResource(table, 4, true, 31);
        var references = new NodeRefList<CMwNod?> { null, null, null };
        references.Get(0).Descriptor = file;
        references.Get(1).Descriptor = resource;
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        if (deprecated) writer.WriteListNodeRef_deprec(references);
        else writer.WriteListNodeRef(references);
        writer.Write(0x12345678);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(writer.NodeDict.ToDictionary(x => x.Value, x => (GbxRefTableNode)x.Key));
        IGbxReader typedReader = reader;
        var restored = deprecated ? typedReader.ReadListNodeRef_deprec<CMwNod?>() : typedReader.ReadListNodeRef<CMwNod?>();
        await Assert.That(restored.Count).IsEqualTo(3);
        await Assert.That(restored.Get(0).File).IsSameReferenceAs(file);
        await Assert.That(restored.Get(1).Resource).IsSameReferenceAs(resource);
        await Assert.That(restored.Get(0).State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(restored.Get(2).Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(reader.ReadInt32()).IsEqualTo(0x12345678);
        restored.Add(null);
        await Assert.That(restored.Count).IsEqualTo(4);
    }

    [Test]
    public async Task ReadListNodeRef_SharedAndNullNodes_KeepsIndependentHandles()
    {
        var node = new CMwNod();
        var references = new NodeRefList<CMwNod?> { node, node, null, null };
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.WriteListNodeRef(references);

        stream.Position = 0;
        using var reader = new GbxReader(stream);
        var restored = reader.ReadListNodeRef<CMwNod?>();

        await Assert.That(restored[0]).IsSameReferenceAs(restored[1]);
        await Assert.That(restored.Get(0)).IsNotSameReferenceAs(restored.Get(1));
        await Assert.That(restored.Get(2)).IsNotSameReferenceAs(restored.Get(3));

        restored.Get(0).Clear();
        restored.Get(2).Node = node;
        await Assert.That(restored[0]).IsNull();
        await Assert.That(restored[1]).IsNotNull();
        await Assert.That(restored[2]).IsSameReferenceAs(node);
        await Assert.That(restored[3]).IsNull();
    }

    [Test]
    public async Task ReadListNodeRef_ConcurrentFirstAccess_PreservesEmbeddedHandles()
    {
        var node = new CMwNod();
        var references = new NodeRefList<CMwNod> { node };
        var reference = references.Get(0);
        var start = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var readers = Enumerable.Range(0, 32).Select(async _ =>
        {
            await start.Task;
            return reference.Resolve();
        }).ToArray();

        start.SetResult(true);
        var results = await Task.WhenAll(readers);
        await Assert.That(results.All(result => ReferenceEquals(result, node))).IsTrue();
        reference.Clear();
        await Assert.That(reference.Node).IsNull();
    }

    private sealed class EqualMaterial : CPlugMaterial
    {
        public override bool Equals(object? obj) => obj is EqualMaterial;
        public override int GetHashCode() => 1;
    }

    [Test]
    public async Task WriteNodeRef_ValueEqualNodes_UsesObjectIdentity()
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        var first = new NodeRef<CPlugMaterial>();
        var second = new NodeRef<CPlugMaterial>();
        first.Node = new EqualMaterial();
        second.Node = new EqualMaterial();
        writer.WriteNodeRef(first);
        writer.WriteNodeRef(second);
        await Assert.That(writer.NodeDict.Count).IsEqualTo(2);
    }

    [Test]
    public async Task Resolve_MissingNode_ReturnsNullAndKeepsDescriptorUntilReplaced()
    {
        var table = new GbxRefTable();
        var file = new GbxRefTableFile(table, 0, true, "missing.Gbx");
        var owner = new CGameItemModel();
        var view = owner.References;
        var reference = view.EntityModelEdition;
        reference.Descriptor = file;

        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(owner.EntityModelEdition).IsNull();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.ExternalFile);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Missing);
        await Assert.That(reference.File).IsSameReferenceAs(file);
        await Assert.That(reference.TryResolve(out var missing)).IsFalse();
        await Assert.That(missing).IsNull();

        var embedded = new CPlugCrystal();
        owner.EntityModelEdition = embedded;
        await Assert.That(view.EntityModelEdition).IsSameReferenceAs(reference);
        await Assert.That(reference.File).IsNull();
        await Assert.That(reference.Resolve()).IsSameReferenceAs(embedded);
        owner.EntityModelEdition = null;
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.None);
    }

    [Test]
    public async Task Resolve_SharedEntries_LoadsOnceAndAllowsIndependentReplacement()
    {
        var calls = 0;
        var node = new CPlugCrystal();
        var table = new GbxRefTable();
        table.ExternalNodes["shared.Gbx"] = () => { calls++; return node.ToGbx(); };
        var file = new GbxRefTableFile(table, 0, true, "shared.Gbx");
        var first = new NodeRef<CMwNod>();
        var second = new NodeRef<CPlugCrystal>();
        first.Descriptor = file;
        second.Descriptor = file;
        await Assert.That(first.Resolve()).IsSameReferenceAs(node);
        await Assert.That(second.Resolve()).IsSameReferenceAs(node);
        await Assert.That(calls).IsEqualTo(1);
        first.Node = null;
        await Assert.That(second.File).IsSameReferenceAs(file);
        await Assert.That(second.Resolve()).IsSameReferenceAs(node);
    }

    [Test]
    public async Task Resolve_MissingAndFailedOutcomes_CachesUntilExplicitRetry()
    {
        var table = new GbxRefTable();
        var file = new GbxRefTableFile(table, 0, true, "dependency.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = file;
        await Assert.That(reference.Resolve()).IsNull();
        var node = new CPlugCrystal();
        table.ExternalNodes[file.FilePath] = () => node.ToGbx();
        await Assert.That(reference.Resolve()).IsNull();
        await Assert.That(reference.Retry()).IsSameReferenceAs(node);

        var calls = 0;
        var failure = new InvalidDataException("Malformed dependency");
        table.ExternalNodes["bad.Gbx"] = () => { calls++; throw failure; };
        reference.Descriptor = new GbxRefTableFile(table, 0, true, "bad.Gbx");
        await Assert.That(() => reference.Resolve()).Throws<InvalidDataException>();
        await Assert.That(() => reference.TryResolve(out _)).Throws<InvalidDataException>();
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Failed);
        await Assert.That(reference.Exception).IsSameReferenceAs(failure);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task Resolve_DeclaredTypes_ValidatesWithoutChangingSharedResolution()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        table.ExternalNodes["crystal.Gbx"] = () => node.ToGbx();
        var file = new GbxRefTableFile(table, 0, true, "crystal.Gbx");
        var incompatible = new NodeRef<CPlugMaterial>();
        var compatible = new NodeRef<CMwNod>();
        incompatible.Descriptor = file;
        compatible.Descriptor = file;
        await Assert.That(() => incompatible.Resolve()).Throws<InvalidCastException>();
        await Assert.That(incompatible.Exception!.Message).Contains("Expected CPlugMaterial,");
        await Assert.That(incompatible.State).IsEqualTo(NodeRefState.Failed);
        await Assert.That(incompatible.CachedNode).IsNull();
        await Assert.That(((NodeRef)incompatible).CachedNode).IsNull();
        await Assert.That(compatible.Resolve()).IsSameReferenceAs(node);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ResolveAsync_AsyncAndSyncResolvers_PrefersAsyncWithSyncFallback(bool hasAsyncResolver)
    {
        var table = new GbxRefTable();
        var syncNode = new CPlugCrystal();
        var asyncNode = new CPlugCrystal();
        var syncCalls = 0;
        var asyncCalls = 0;
        table.ExternalNodes["dependency.Gbx"] = () => { syncCalls++; return syncNode.ToGbx(); };
        if (hasAsyncResolver)
        {
            table.ExternalNodesAsync["dependency.Gbx"] = _ => { asyncCalls++; return Task.FromResult(asyncNode.ToGbx()); };
        }
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = new GbxRefTableFile(table, 0, true, "dependency.Gbx");
        var node = hasAsyncResolver ? asyncNode : syncNode;

        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(node);
        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(node);
        await Assert.That(reference.Resolve()).IsSameReferenceAs(node);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolved);
        await Assert.That(syncCalls).IsEqualTo(hasAsyncResolver ? 0 : 1);
        await Assert.That(asyncCalls).IsEqualTo(hasAsyncResolver ? 1 : 0);
    }

    [Test]
    public async Task ResolveAsync_ConcurrentCalls_SharesWorkAndCancelsOnlyOneWaiter()
    {
        var table = new GbxRefTable();
        var source = new TaskCompletionSource<Gbx>(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;
        table.ExternalNodesAsync["async.Gbx"] = _ => { calls++; return source.Task; };
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = new GbxRefTableFile(table, 0, true, "async.Gbx");
        using var cancellation = new CancellationTokenSource();
        var canceled = reference.ResolveAsync(cancellation.Token);
        var waiting = reference.ResolveAsync();
        cancellation.Cancel();
        await Assert.That(async () => await canceled).Throws<OperationCanceledException>();
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolving);
        var node = new CPlugCrystal();
        source.SetResult(node.ToGbx());
        await Assert.That(await waiting).IsSameReferenceAs(node);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task ResolveAsync_ReassignedReference_DoesNotPublishOldNode()
    {
        var source = new TaskCompletionSource<Gbx>(TaskCreationOptions.RunContinuationsAsynchronously);
        var table = new GbxRefTable();
        table.ExternalNodesAsync["old.Gbx"] = _ => source.Task;
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = new GbxRefTableFile(table, 0, true, "old.Gbx");
        var loading = reference.ResolveAsync();
        var replacement = new CPlugMaterial();
        reference.Node = replacement;
        source.SetResult(new CPlugCrystal().ToGbx());
        await loading;
        await Assert.That(reference.CachedNode).IsSameReferenceAs(replacement);
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.Embedded);
    }

    [Test]
    public async Task References_CollectionView_ExposesLiveReadOnlyHandlesWithoutResolving()
    {
        var owner = new CGameItemModel { NadeoSkinsFids = null };
        var view = owner.References;
        await Assert.That(view.NadeoSkinsFids).IsNull();
        await Assert.That(owner.NadeoSkinsFids).IsNull();

        var calls = 0;
        var table = new GbxRefTable();
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var file = table.CreateFile("unloaded.Gbx");
        var nodes = new NodeRefList<CMwNod?> { null, null };
        nodes.Get(0).Descriptor = file;
        owner.NadeoSkinsFids = nodes;
        var handles = view.NadeoSkinsFids!;

        await Assert.That(view.NadeoSkinsFids).IsSameReferenceAs(handles);
        await Assert.That(handles[0]).IsSameReferenceAs(nodes.Get(0));
        await Assert.That(handles[0].File).IsSameReferenceAs(file);
        var stored = handles.ToArray();
        await Assert.That(stored.Length).IsEqualTo(nodes.Count);
        for (var i = 0; i < stored.Length; i++)
        {
            await Assert.That(stored[i]).IsSameReferenceAs(nodes.Get(i));
        }
        var readOnly = (System.Collections.IList)handles;
        await Assert.That(readOnly.IsReadOnly).IsTrue();
        await Assert.That(() => readOnly.Add(new NodeRef<CMwNod>())).Throws<NotSupportedException>();
        await Assert.That(() => readOnly[0] = new NodeRef<CMwNod>()).Throws<NotSupportedException>();

        nodes.Add(null);
        await Assert.That(handles.Count).IsEqualTo(3);
        var retained = handles[0];
        nodes.RemoveAt(0);
        await Assert.That(handles.Count).IsEqualTo(2);
        await Assert.That(handles[0]).IsSameReferenceAs(nodes.Get(0));
        await Assert.That(retained.File).IsSameReferenceAs(file);
        handles[0].Descriptor = file;
        await Assert.That(nodes.Get(0).File).IsSameReferenceAs(file);

        owner.NadeoSkinsFids = new NodeRefList<CMwNod?> { null };
        await Assert.That(view.NadeoSkinsFids![0]).IsSameReferenceAs(owner.NadeoSkinsFids.Get(0));
        await Assert.That(view.NadeoSkinsFids).IsNotSameReferenceAs(handles);
        await Assert.That(handles.Count).IsEqualTo(2);
        owner.NadeoSkinsFids = null;
        await Assert.That(view.NadeoSkinsFids).IsNull();
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task References_JaggedCollectionView_ExposesCurrentRowsWithoutResolving()
    {
        var calls = 0;
        var table = new GbxRefTable();
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var file = table.CreateFile("unloaded.Gbx");
        var row = new NodeRefList<CGameCtnBlockInfoMobil> { new NodeRef<CGameCtnBlockInfoMobil>() };
        row.Get(0).Descriptor = file;
        var owner = new CGameCtnBlockInfoVariant { Mobils = [row, null!] };
        var view = owner.References;
        var handles = view.Mobils!;

        await Assert.That(handles.Count).IsEqualTo(2);
        await Assert.That(handles[0]![0]).IsSameReferenceAs(row.Get(0));
        await Assert.That(handles[0]![0].File).IsSameReferenceAs(file);
        await Assert.That(handles[1]).IsNull();
        row.Add(new NodeRef<CGameCtnBlockInfoMobil>());
        await Assert.That(handles[0]!.Count).IsEqualTo(2);
        owner.Mobils[1] = new NodeRefList<CGameCtnBlockInfoMobil> { new NodeRef<CGameCtnBlockInfoMobil>() };
        var current = view.Mobils!;
        await Assert.That(current[0]).IsSameReferenceAs(handles[0]);
        await Assert.That(current[1]![0]).IsSameReferenceAs(owner.Mobils[1].Get(0));
        await Assert.That(handles[1]).IsNull();
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task NodeAccess_CollectionWithMissingNodes_ContinuesAndKeepsEntryIdentity()
    {
        var list = new NodeRefList<CMwNod?> { null, new CPlugCrystal() };
        var table = new GbxRefTable();
        var file = new GbxRefTableFile(table, 0, true, "missing.Gbx");
        var first = list.Get(0);
        first.Descriptor = file;
        var snapshot = list.ToArray();
        await Assert.That(snapshot[0]).IsNull();
        await Assert.That(snapshot[1]).IsTypeOf<CPlugCrystal>();
        list.RemoveAt(0);
        await Assert.That(first.File).IsSameReferenceAs(file);
        await Assert.That(list.Get(0)).IsNotSameReferenceAs(first);

        var owner = new CGameItemModel { NadeoSkinsFids = list };
        var captured = owner.NadeoSkinsFids;
        owner.NadeoSkinsFids = new NodeRefList<CMwNod?> { null };
        await Assert.That(captured).IsSameReferenceAs(list);
        await Assert.That(owner.EnumerateReferences().Single(x => x.Key == "NadeoSkinsFids[0]").Value)
            .IsSameReferenceAs(owner.NadeoSkinsFids.Get(0));
    }

    [Test]
    public async Task WriteAndDeepClone_ResourceAndFileReferences_DoesNotResolveNodes()
    {
        var calls = 0;
        var table = new GbxRefTable();
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var file = new GbxRefTableFile(table, 32, true, "unloaded.Gbx");
        var resource = new GbxRefTableResource(table, 4, false, 42);
        var first = new NodeRef<CMwNod>();
        var second = new NodeRef<CMwNod>();
        first.Descriptor = file;
        second.Descriptor = resource;
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.WriteNodeRef(first);
            writer.WriteNodeRef(first);
            writer.WriteNodeRef(second);
            await Assert.That(writer.NodeDict.Count).IsEqualTo(2);
        }
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file, [2] = resource });
        var restored = reader.ReadNodeRef<CMwNod>(null);
        await Assert.That(restored.File).IsSameReferenceAs(file);
        reader.ReadNodeRef<CMwNod>(null);
        var restoredResource = reader.ReadNodeRef<CMwNod>(null);
        await Assert.That(restoredResource.Resource).IsSameReferenceAs(resource);
        await Assert.That(() => restoredResource.Resolve()).Throws<NotSupportedException>();
        var clone = new DeepCloneContext().Clone(first)!;
        await Assert.That(clone.File!.FilePath).IsEqualTo(file.FilePath);
        await Assert.That(clone.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task WriteNodeRef_EncapsulatedExternalReference_ThrowsEvenWithCachedNode()
    {
        var reference = new NodeRef<CMwNod>();
        var table = new GbxRefTable();
        table.ExternalNodes["loaded.Gbx"] = () => new CPlugCrystal().ToGbx();
        reference.Descriptor = new GbxRefTableFile(table, 0, true, "loaded.Gbx");
        reference.Resolve();
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        using var encapsulation = new Encapsulation(writer);
        await Assert.That(() => writer.WriteNodeRef(reference)).Throws<NotSupportedException>();
        await Assert.That(stream.Length).IsEqualTo(0);
    }

    [Test]
    public async Task Resolve_DistinctDescriptorsWithSameNode_DoesNotMergeDescriptors()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        table.ExternalNodes["same.Gbx"] = () => node.ToGbx();
        var first = new NodeRef<CMwNod>();
        var second = new NodeRef<CMwNod>();
        first.Descriptor = new GbxRefTableFile(table, 0, true, "same.Gbx");
        second.Descriptor = new GbxRefTableFile(table, 32, false, "same.Gbx");
        first.Resolve();
        second.Resolve();
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        writer.WriteNodeRef(first);
        writer.WriteNodeRef(second);
        writer.WriteNodeRef(node);
        await Assert.That(writer.NodeDict.Count).IsEqualTo(3);
        await Assert.That(first.Kind).IsEqualTo(NodeRefKind.ExternalFile);
    }

    [Test]
    public async Task FilePathSetter_ChangedPath_InvalidatesSharedNodesAndTypeErrors()
    {
        var table = new GbxRefTable();
        var original = new CPlugCrystal();
        var replacement = new CPlugMaterial();
        var calls = 0;
        table.ExternalNodes["old.Gbx"] = () => original.ToGbx();
        table.ExternalNodes["new.Gbx"] = () => { calls++; return replacement.ToGbx(); };
        var file = new GbxRefTableFile(table, 0, true, "old.Gbx");
        var general = new NodeRef<CMwNod>();
        var material = new NodeRef<CPlugMaterial>();
        general.Descriptor = file;
        material.Descriptor = file;
        await Assert.That(general.Resolve()).IsSameReferenceAs(original);
        await Assert.That(() => material.Resolve()).Throws<InvalidCastException>();

        file.FilePath = "new.Gbx";

        await Assert.That(general.CachedNode).IsNull();
        await Assert.That(material.Exception).IsNull();
        await Assert.That(general.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(material.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(await material.ResolveAsync()).IsSameReferenceAs(replacement);
        await Assert.That(general.Resolve()).IsSameReferenceAs(replacement);
        await Assert.That(material.Exception).IsNull();
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(general.File).IsSameReferenceAs(file);
        await Assert.That(material.File).IsSameReferenceAs(file);
    }

    [Test]
    public async Task FilePathSetter_ChangedPath_InvalidatesMissingResolution()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        table.ExternalNodes["new.Gbx"] = () => node.ToGbx();
        var file = new GbxRefTableFile(table, 0, true, "missing.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = file;
        await Assert.That(await reference.ResolveAsync()).IsNull();
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Missing);

        file.FilePath = "new.Gbx";

        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(reference.Resolve()).IsSameReferenceAs(node);
    }

    [Test]
    public async Task FilePathSetter_ChangedPath_InvalidatesFailedResolution()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        table.ExternalNodes["bad.Gbx"] = () => throw new InvalidDataException("Malformed dependency");
        table.ExternalNodes["new.Gbx"] = () => node.ToGbx();
        var file = new GbxRefTableFile(table, 0, true, "bad.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = file;
        await Assert.That(() => reference.Resolve()).Throws<InvalidDataException>();

        file.FilePath = "new.Gbx";

        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(reference.Exception).IsNull();
        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(node);
    }

    [Test]
    public async Task FilePathSetter_SameOrNullPath_PreservesCachedResolution()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["same.Gbx"] = () => { calls++; return node.ToGbx(); };
        var file = new GbxRefTableFile(table, 0, true, "same.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = file;
        await Assert.That(reference.Resolve()).IsSameReferenceAs(node);

        file.FilePath = new string("same.Gbx".ToCharArray());
        await Assert.That(() => file.FilePath = null!).Throws<ArgumentNullException>();

        await Assert.That(file.FilePath).IsEqualTo("same.Gbx");
        await Assert.That(reference.CachedNode).IsSameReferenceAs(node);
        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(node);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task FilePathSetter_Resolving_RejectsChangeWithoutAffectingLoad()
    {
        var table = new GbxRefTable();
        var source = new TaskCompletionSource<Gbx>(TaskCreationOptions.RunContinuationsAsynchronously);
        table.ExternalNodesAsync["old.Gbx"] = _ => source.Task;
        var file = new GbxRefTableFile(table, 0, true, "old.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = file;
        var loading = reference.ResolveAsync();

        file.FilePath = "old.Gbx";
        await Assert.That(() => file.FilePath = "new.Gbx").Throws<InvalidOperationException>();
        await Assert.That(() => ((NodeRef)reference).Reset()).Throws<InvalidOperationException>();
        await Assert.That(file.FilePath).IsEqualTo("old.Gbx");
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolving);
        var node = new CPlugCrystal();
        source.SetResult(node.ToGbx());
        await Assert.That(await loading).IsSameReferenceAs(node);
        await Assert.That(reference.CachedNode).IsSameReferenceAs(node);

        file.FilePath = "new.Gbx";
        await Assert.That(reference.CachedNode).IsNull();
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
    }

    [Test]
    public async Task FilePathSetter_ChangedPath_SavesTheNewPathWithoutResolving()
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 32, true, "old.Gbx");
        var owner = new CGameItemModel();
        owner.CreateChunk<CGameItemModel.Chunk2E002019>().Version = 15;
        owner.References.EntityModelEdition.Descriptor = file;
        file.FilePath = "assets/new.Gbx";
        using var stream = new MemoryStream();

        owner.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);

        stream.Position = 0;
        var parsed = Gbx.Parse(stream);
        var restored = (CGameItemModel)parsed.Node!;
        await Assert.That(restored.References.EntityModelEdition.File!.FilePath.Replace('\\', '/')).IsEqualTo("assets/new.Gbx");
        await Assert.That(owner.References.EntityModelEdition.State).IsEqualTo(NodeRefState.Unresolved);
    }

    [Test]
    public async Task External_ChangedFilePath_InvalidatesTheSeededNode()
    {
        var table = new GbxRefTable();
        var original = new CPlugCrystal();
        var replacement = new CPlugCrystal();
        table.ExternalNodes["new.Gbx"] = () => replacement.ToGbx();
        var file = new GbxRefTableFile(table, 0, true, "old.Gbx");
#pragma warning disable CS0618
        var external = new External<CPlugCrystal>(original, file);
#pragma warning restore CS0618
        await Assert.That(external.Node).IsSameReferenceAs(original);

        file.FilePath = "new.Gbx";

        await Assert.That(external.Node).IsSameReferenceAs(replacement);
        file.FilePath = "missing.Gbx";
        await Assert.That(external.Node).IsNull();
    }

    [Test]
    public async Task External_Resolution_UsesTheSameCacheAsNodeRef()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["shared.Gbx"] = () => { calls++; return node.ToGbx(); };
        var file = new GbxRefTableFile(table, 0, true, "shared.Gbx");
#pragma warning disable CS0618
        var external = new External<CPlugCrystal>(null, file);
#pragma warning restore CS0618
        var reference = new NodeRef<CPlugCrystal>();
        reference.Descriptor = file;

        await Assert.That(external.Node).IsSameReferenceAs(node);
        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(node);
        await Assert.That(external.Node).IsSameReferenceAs(node);
        await Assert.That(calls).IsEqualTo(1);
        var copy = external with { Node = null };
        await Assert.That(copy.File).IsNull();
        await Assert.That(copy.Node).IsNull();
        await Assert.That(external.File).IsSameReferenceAs(file);
        await Assert.That(external.Node).IsSameReferenceAs(node);
        await Assert.That(reference.CachedNode).IsSameReferenceAs(node);
    }

    [Test]
    public async Task DeepClone_ResolvedSharedDescriptor_PreservesTheClonedCacheAndIndependentPath()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        table.ExternalNodes["old.Gbx"] = () => node.ToGbx();
        var file = new GbxRefTableFile(table, 0, true, "old.Gbx");
        var first = new NodeRef<CMwNod>();
        var second = new NodeRef<CPlugCrystal>();
        first.Descriptor = file;
        second.Descriptor = file;
        first.Resolve();
        second.Resolve();
        var context = new DeepCloneContext();

        var clonedFirst = context.Clone(first)!;
        var clonedSecond = context.Clone(second)!;
        file.FilePath = "new.Gbx";

        await Assert.That(clonedFirst.State).IsEqualTo(NodeRefState.Resolved);
        await Assert.That(clonedFirst.CachedNode).IsNotSameReferenceAs(node);
        await Assert.That(clonedFirst.CachedNode).IsTypeOf<CPlugCrystal>();
        await Assert.That(clonedSecond.CachedNode).IsSameReferenceAs(clonedFirst.CachedNode);
        await Assert.That(clonedFirst.File).IsSameReferenceAs(clonedSecond.File);
        await Assert.That(clonedFirst.File!.FilePath).IsEqualTo("old.Gbx");
        await Assert.That(first.CachedNode).IsNull();
        clonedFirst.File.FilePath = "missing.Gbx";
        await Assert.That(clonedFirst.CachedNode).IsNull();
        await Assert.That(clonedSecond.CachedNode).IsNull();
    }

    [Test]
    public async Task Retry_SharedReference_InvalidatesMemberTypeErrorsAndCachedNodes()
    {
        var table = new GbxRefTable();
        table.ExternalNodes["changing.Gbx"] = () => new CPlugCrystal().ToGbx();
        var file = new GbxRefTableFile(table, 0, true, "changing.Gbx");
        var general = new NodeRef<CMwNod>();
        var material = new NodeRef<CPlugMaterial>();
        general.Descriptor = file;
        material.Descriptor = file;
        await Assert.That(() => material.Resolve()).Throws<InvalidCastException>();
        var replacement = new CPlugMaterial();
        table.ExternalNodes[file.FilePath] = () => replacement.ToGbx();
        await Assert.That(general.Retry()).IsSameReferenceAs(replacement);
        await Assert.That(material.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(material.Resolve()).IsSameReferenceAs(replacement);
        await Assert.That(material.Exception).IsNull();
    }

    [Test]
    public async Task ReadListNodeRef_ExplicitLength_AllowsStructuralChanges()
    {
        using var stream = new MemoryStream(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 });
        using var reader = new GbxReader(stream);
        var references = reader.ReadListNodeRef<CMwNod?>(length: 2);
        references[0] = new CPlugMaterial();
        var captured = references.Get(0);
        await Assert.That(references.Count).IsEqualTo(2);
        references.Add(null);
        references.Insert(0, new CPlugCrystal());
        references.RemoveAt(0);
        await Assert.That(references.Count).IsEqualTo(3);
        await Assert.That(references.Get(0)).IsSameReferenceAs(captured);
        references.Clear();
        await Assert.That(references.Count).IsEqualTo(0);
    }

    [Test]
    [Arguments(1)]
    [Arguments(3)]
    public async Task WriteListNodeRef_ExplicitWireLength_PadsOrTruncatesWithoutResizingList(int count)
    {
        var references = new NodeRefList<CMwNod?>(Enumerable.Repeat<CMwNod?>(null, count));
        var captured = references.Get(0);
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);

        writer.WriteListNodeRef(references, length: 2);

        await Assert.That(references.Count).IsEqualTo(count);
        await Assert.That(references.Get(0)).IsSameReferenceAs(captured);
        await Assert.That(stream.ToArray()).IsEquivalentTo(Enumerable.Repeat((byte)255, 8).ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadJaggedArrayNodeRef_ExplicitLength_KeepsRowsResizable(bool fixedLength)
    {
        var original = new[] { new NodeRefList<CMwNod?> { null, null }, new NodeRefList<CMwNod?> { null, null } };
        var innerLength = fixedLength ? 2 : (int?)null;
        using var input = new MemoryStream();
        using (var writer = new GbxWriter(input)) writer.WriteJaggedArrayNodeRef(original, innerLength, outerLength: 2);
        input.Position = 0;
        using var reader = new GbxReader(input);
        var rows = ((IGbxReader)reader).ReadJaggedListNodeRef<CMwNod?>(innerLength, outerLength: 2);
        var captured = rows[0].Get(0);

        rows[0].Add(null);
        rows[1].RemoveAt(0);
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output)) writer.WriteJaggedArrayNodeRef(rows, innerLength, outerLength: 2);

        await Assert.That(rows[0].Count).IsEqualTo(3);
        await Assert.That(rows[1].Count).IsEqualTo(1);
        await Assert.That(rows[0].Get(0)).IsSameReferenceAs(captured);
        output.Position = 0;
        using var outputReader = new GbxReader(output);
        var restored = outputReader.ReadJaggedListNodeRef<CMwNod?>(innerLength, outerLength: 2);
        await Assert.That(restored[0].Count).IsEqualTo(fixedLength ? 2 : 3);
        await Assert.That(restored[1].Count).IsEqualTo(fixedLength ? 2 : 1);
        await Assert.That(output.Position).IsEqualTo(output.Length);
    }

    [Test]
    public async Task ReadWrite_HandwrittenPresenceFlags_KeepsUnresolvedResourceReferences()
    {
        var resource = new GbxRefTableResource(new GbxRefTable(), 4, true, 42);
        var material = new CPlugSurface.SurfMaterial();
        material.References.Material.Descriptor = resource;
        using var stream = new MemoryStream();
        using (var rw = new GbxReaderWriter(new GbxWriter(stream))) material.ReadWrite(rw);
        await Assert.That(stream.Length).IsEqualTo(8);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = resource });
        using var readWrite = new GbxReaderWriter(reader);
        var restored = new CPlugSurface.SurfMaterial();
        restored.ReadWrite(readWrite);
        await Assert.That(restored.References.Material.Resource).IsSameReferenceAs(resource);
        await Assert.That(restored.References.Material.State).IsEqualTo(NodeRefState.Unresolved);
    }

    [Test]
    public async Task ReadWrite_GeneratedPresenceFlags_KeepsUnresolvedResourceReferences()
    {
        var resource = new GbxRefTableResource(new GbxRefTable(), 4, true, 42);
        var owner = new CGameItemModel();
        owner.References.IconFid.Descriptor = resource;
        var chunk = new CGameCtnCollector.Chunk2E001003();
        using var stream = new MemoryStream();
        using (var rw = new GbxReaderWriter(new GbxWriter(stream))) chunk.ReadWrite(owner, rw);
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = resource });
        using var readWrite = new GbxReaderWriter(reader);
        var restored = new CGameItemModel();
        new CGameCtnCollector.Chunk2E001003().ReadWrite(restored, readWrite);
        await Assert.That(restored.References.IconFid.Resource).IsSameReferenceAs(resource);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
    }

    [Test]
    public async Task Import_Descriptor_RebasesPathsWithoutChangingSource()
    {
        var root = Path.Combine(Path.GetTempPath(), "gbx-reference-rebase");
        var source = new GbxRefTable { FileSystemPath = Path.Combine(root, "source"), AncestorLevel = 1 };
        var destination = new GbxRefTable { FileSystemPath = Path.Combine(root, "output"), AncestorLevel = 0 };
        var file = new GbxRefTableFile(source, 32, true, "assets/dependency.Gbx");
        var imported = destination.Import(file);
        await Assert.That(imported.GetFullPath()).IsEqualTo(file.GetFullPath());
        await Assert.That(file.FilePath).IsEqualTo("assets/dependency.Gbx");
        await Assert.That(imported.Flags).IsEqualTo(32);
        await Assert.That(imported.UseFile).IsTrue();
    }

    [Test]
    public async Task Save_NewParent_PreservesFirstDescriptorAncestorLevel()
    {
        var table = new GbxRefTable { AncestorLevel = 2 };
        var file = new GbxRefTableFile(table, 32, true, "dependency.Gbx");
        var owner = new CGameItemModel();
        owner.CreateChunk<CGameItemModel.Chunk2E002019>().Version = 15;
        owner.References.EntityModelEdition.Descriptor = file;
        using var stream = new MemoryStream();
        owner.ToGbx(GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed)).Save(stream);
        stream.Position = 0;
        var parsed = Gbx.Parse(stream);
        await Assert.That(parsed.RefTable!.AncestorLevel).IsEqualTo(2);
        var restored = (CGameItemModel)parsed.Node!;
        await Assert.That(restored.References.EntityModelEdition.File!.FilePath).IsEqualTo(file.FilePath);
        await Assert.That(restored.References.EntityModelEdition.File!.Flags).IsEqualTo(file.Flags);
    }

    [Test]
    public async Task Resolve_FilesystemReferences_PreservesSettingsAndExternalCycles()
    {
        var directory = Path.Combine(Path.GetTempPath(), "gbx-reference-cycle-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var table = new GbxRefTable();
            var first = new CGameItemModel();
            var second = new CGameItemModel();
            first.CreateChunk<CGameItemModel.Chunk2E002019>().Version = 15;
            second.CreateChunk<CGameItemModel.Chunk2E002019>().Version = 15;
            first.References.EntityModelEdition.Descriptor = new GbxRefTableFile(table, 0, true, "second.Gbx");
            second.References.EntityModelEdition.Descriptor = new GbxRefTableFile(table, 0, true, "first.Gbx");
            var basic = GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed);
            var firstPath = Path.Combine(directory, "first.Gbx");
            first.ToGbx(basic).Save(firstPath);
            second.ToGbx(basic).Save(Path.Combine(directory, "second.Gbx"));
            var settings = new GbxReadSettings { MaxDataSize = 4096, GameVersion = GameVersion.TM2020 };
            var parsed = Gbx.Parse(firstPath, settings);
            var owner = (CGameItemModel)parsed.Node!;
            var dependency = (CGameItemModel)owner.EntityModelEdition!;
            await Assert.That(dependency.EntityModelEdition).IsSameReferenceAs(owner);
            await Assert.That(dependency.References.EntityModelEdition.File!.RefTable.ReadSettings).IsEqualTo(settings);
#pragma warning disable GBXNET10001
            var clone = (CGameItemModel)owner.DeepClone();
#pragma warning restore GBXNET10001
            var clonedDependency = (CGameItemModel)clone.EntityModelEdition!;
            await Assert.That(clonedDependency.EntityModelEdition).IsSameReferenceAs(clone);
            await Assert.That(clonedDependency).IsNotSameReferenceAs(dependency);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task Copy_LegacyReferences_PreservesResourceIdentityWithoutResolvingNodes()
    {
        var table = new GbxRefTable();
        var resource = new GbxRefTableResource(table, 4, true, 31);
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            writer.Write(0); // Legacy model prefix.
            writer.Write(1); // External particle emitter.
            writer.Write(0); // Wheel index.
            writer.Write(0); // Handler index.
            writer.Write(0); // Material group index.
            writer.Write(false);
            writer.Write(false);
            writer.Write(false);
            writer.Write(new Vec3());
            writer.Write(1f);
            writer.Write(new Vec3());
            writer.Write(0f);
            writer.Write(new Vec3());
            writer.Write(new Vec3());
            writer.Write(new Vec3());
        }
        stream.Position = 0;
        using var reader = new GbxReader(stream);
        reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = resource });
        using var rw = new GbxReaderWriter(reader);
        var owner = new CPlugVehicleVisEmitterModel();
        new CPlugVehicleVisEmitterModel.Chunk090E6000().ReadWrite(owner, rw);
        await Assert.That(owner.References.ParticleEmitterModel1.Resource).IsSameReferenceAs(resource);
        await Assert.That(owner.References.ParticleEmitterModel2.Resource).IsSameReferenceAs(resource);
        await Assert.That(owner.References.ParticleEmitterModel1.State).IsEqualTo(NodeRefState.Unresolved);
    }
}
