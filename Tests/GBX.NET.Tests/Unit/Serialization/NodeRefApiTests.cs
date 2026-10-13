using GBX.NET.Components;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class NodeRefApiTests
{
    [Test]
    public async Task Resolve_ReentrantAccessWhileLoading_ThrowsAndCachesTheError()
    {
        var table = new GbxRefTable();
        var reference = new NodeRef<CPlugCrystal> { Descriptor = table.CreateFile("recursive.Gbx") };
        var calls = 0;
        table.ExternalNodes["recursive.Gbx"] = () => { calls++; return reference.Node!.ToGbx(); };

        await Assert.That(() => reference.Resolve()).Throws<InvalidOperationException>();
        await Assert.That(() => reference.Node).Throws<InvalidOperationException>();
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Failed);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PropertyAssignments_ReplaceStorageWithoutResolvingAndKeepHandleIdentity(bool untyped)
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var file = table.CreateFile("unloaded.Gbx");
        var resource = table.CreateResource(42);
        var typed = new NodeRef<CPlugCrystal>();
        NodeRef reference = typed;
        var node = new CPlugCrystal();

        reference.Descriptor = file;
        await Assert.That(reference.File).IsSameReferenceAs(file);
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.ExternalFile);

        if (untyped) reference.Node = node;
        else typed.Node = node;

        await Assert.That(reference.Descriptor).IsNull();
        await Assert.That(reference.Node).IsSameReferenceAs(node);
        await Assert.That(typed.Node).IsSameReferenceAs(node);
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.Embedded);

        typed.Descriptor = resource;
        await Assert.That(reference.Resource).IsSameReferenceAs(resource);
        await Assert.That(reference.CachedNode).IsNull();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.ExternalResource);

        reference.Descriptor = null;
        await Assert.That(reference.Node).IsNull();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task UntypedNodeAssignment_IncompatibleType_LeavesStoredReferenceIntact()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["crystal.Gbx"] = () => { calls++; return node.ToGbx(); };
        var descriptor = table.CreateFile("crystal.Gbx");
        var typed = new NodeRef<CPlugCrystal> { Descriptor = descriptor };
        NodeRef reference = typed;

        await Assert.That(reference.Node).IsSameReferenceAs(node);
        await Assert.That(() => reference.Node = new CPlugMaterial()).Throws<InvalidCastException>();
        await Assert.That(reference.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(reference.Node).IsSameReferenceAs(node);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolved);
        await Assert.That(reference.Error).IsNull();
        await Assert.That(calls).IsEqualTo(1);

        reference.Node = null;
        await Assert.That(reference.Node).IsNull();
        await Assert.That(reference.Descriptor).IsNull();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.None);
    }

    [Test]
    public async Task UntypedReference_Resolution_UsesTheTypedHandlesSharedCache()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["shared.Gbx"] = () => { calls++; return node.ToGbx(); };
        var typed = new NodeRef<CPlugCrystal>();
        GbxRefTableNode descriptor = table.CreateFile("shared.Gbx");
        NodeRef reference = typed;
        reference.Descriptor = descriptor;

        await Assert.That(reference.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(reference.Descriptor!.RefTable).IsSameReferenceAs(table);
        await Assert.That(reference.Node).IsSameReferenceAs(node);
        CPlugCrystal? typedNode = typed.Node;
        await Assert.That(typedNode).IsSameReferenceAs(node);
        await Assert.That(reference.Resolve()).IsSameReferenceAs(node);
        CMwNod? resolvedAsync = await reference.ResolveAsync();
        await Assert.That(resolvedAsync).IsSameReferenceAs(node);
        await Assert.That(reference.TryResolve(out var resolved)).IsTrue();
        await Assert.That(resolved).IsSameReferenceAs(node);
        await Assert.That(typed.CachedNode).IsSameReferenceAs(node);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task NodeAccess_AfterPathChange_UsesTheNewSharedResult(bool untyped)
    {
        var table = new GbxRefTable();
        var original = new CPlugCrystal();
        var replacement = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["original.Gbx"] = () => { calls++; return original.ToGbx(); };
        table.ExternalNodes["replacement.Gbx"] = () => { calls++; return replacement.ToGbx(); };
        var descriptor = table.CreateFile("original.Gbx");
        var reference = new NodeRef<CPlugCrystal>();
        reference.Descriptor = descriptor;
        Func<CMwNod?> access = untyped ? () => ((NodeRef)reference).Node : () => reference.Node;

        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(original);
        await Assert.That(access()).IsSameReferenceAs(original);
        await Assert.That(calls).IsEqualTo(1);

        descriptor.FilePath = "replacement.Gbx";

        await Assert.That(access()).IsSameReferenceAs(replacement);
        await Assert.That(reference.Resolve()).IsSameReferenceAs(replacement);
        await Assert.That(calls).IsEqualTo(2);
        await Assert.That(reference.Descriptor).IsSameReferenceAs(descriptor);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task UntypedReference_IncompatibleNode_PreservesTypeValidation(bool asynchronous)
    {
        var table = new GbxRefTable();
        table.ExternalNodes["crystal.Gbx"] = () => new CPlugCrystal().ToGbx();
        var typed = new NodeRef<CPlugMaterial>();
        typed.Descriptor = table.CreateFile("crystal.Gbx");
        NodeRef reference = typed;

        if (asynchronous)
            await Assert.That(async () => await reference.ResolveAsync()).Throws<InvalidCastException>();
        else
            await Assert.That(() => reference.TryResolve(out _)).Throws<InvalidCastException>();

        await Assert.That(reference.State).IsEqualTo(NodeRefState.Failed);
        await Assert.That(reference.CachedNode).IsNull();
        await Assert.That(reference.Error).IsTypeOf<InvalidCastException>();
        await Assert.That(() => reference.Node).Throws<InvalidCastException>();
        await Assert.That(() => typed.Node).Throws<InvalidCastException>();
    }

    [Test]
    public async Task UntypedReference_ResetAndClear_InvalidateSharedCacheAndKeepHandleOwnership()
    {
        var table = new GbxRefTable();
        var original = new CPlugCrystal();
        var replacement = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["shared.Gbx"] = () => { calls++; return original.ToGbx(); };
        var file = table.CreateFile("shared.Gbx");
        var first = new NodeRef<CPlugCrystal>();
        var second = new NodeRef<CPlugCrystal>();
        first.Descriptor = file;
        second.Descriptor = file;
        first.Resolve();
        second.Resolve();
        NodeRef reference = first;

        reference.Reset();

        await Assert.That(reference.Descriptor).IsSameReferenceAs(file);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(reference.CachedNode).IsNull();
        await Assert.That(second.CachedNode).IsNull();
        await Assert.That(calls).IsEqualTo(1);
        table.ExternalNodes[file.FilePath] = () => { calls++; return replacement.ToGbx(); };
        await Assert.That(await reference.RetryAsync()).IsSameReferenceAs(replacement);
        await Assert.That(second.Resolve()).IsSameReferenceAs(replacement);
        await Assert.That(calls).IsEqualTo(2);
        reference.Clear();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(reference.Descriptor).IsNull();
        await Assert.That(reference.Node).IsNull();
        await Assert.That(reference.Resolve()).IsNull();
        await Assert.That(second.CachedNode).IsSameReferenceAs(replacement);
        await Assert.That(second.File).IsSameReferenceAs(file);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ResolveAsync_AbsentOrMissingNode_ReturnsNull(bool external)
    {
        var reference = new NodeRef<CPlugCrystal>();
        if (external) reference.Descriptor = new GbxRefTable().CreateFile("missing.Gbx");

        CPlugCrystal? result = await reference.ResolveAsync();

        await Assert.That(result).IsNull();
        await Assert.That(reference.Node).IsNull();
        await Assert.That(reference.State).IsEqualTo(external ? NodeRefState.Missing : NodeRefState.Unresolved);
    }

    [Test]
    public async Task ResolveAsync_ResolvedNode_ReturnsTheTypedNode()
    {
        var reference = new NodeRef<CPlugCrystal>();
        var node = new CPlugCrystal();
        reference.Node = node;

        CPlugCrystal? result = await reference.ResolveAsync();

        await Assert.That(result).IsSameReferenceAs(node);
        await Assert.That(reference.Node).IsSameReferenceAs(node);
    }

    [Test]
    public async Task ResolveAsync_ResolverFailure_PropagatesAndCachesTheError()
    {
        var table = new GbxRefTable();
        var failure = new InvalidDataException("Malformed dependency");
        var calls = 0;
        table.ExternalNodes["bad.Gbx"] = () => { calls++; throw failure; };
        var reference = new NodeRef<CPlugCrystal>();
        reference.Descriptor = table.CreateFile("bad.Gbx");

        await Assert.That(async () => await reference.ResolveAsync()).Throws<InvalidDataException>();
        await Assert.That(async () => await reference.ResolveAsync()).Throws<InvalidDataException>();
        await Assert.That(() => reference.Node).Throws<InvalidDataException>();
        await Assert.That(reference.Error).IsSameReferenceAs(failure);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task UntypedReference_CanceledRetry_PreservesTheExistingCache()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        table.ExternalNodes["node.Gbx"] = () => node.ToGbx();
        var typed = new NodeRef<CPlugCrystal>();
        typed.Descriptor = table.CreateFile("node.Gbx");
        typed.Resolve();
        NodeRef reference = typed;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.That(async () => await reference.RetryAsync(cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(async () => await reference.ResolveAsync(cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(reference.CachedNode).IsSameReferenceAs(node);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolved);
    }

    [Test]
    public async Task TypedReference_CanceledRetry_PreservesTheExistingCache()
    {
        var table = new GbxRefTable();
        var node = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["node.Gbx"] = () => { calls++; return node.ToGbx(); };
        var reference = new NodeRef<CPlugCrystal> { Descriptor = table.CreateFile("node.Gbx") };
        await Assert.That(reference.Node).IsSameReferenceAs(node);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.That(async () => await reference.RetryAsync(cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolved);
        await Assert.That(reference.Node).IsSameReferenceAs(node);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task ReferenceCollection_EnumeratingAndInsertingMetadata_DoesNotResolveNodes()
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var file = table.CreateFile("unloaded.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.Descriptor = file;
        var list = new NodeRefList<CMwNod> { null };

        list.Insert(0, reference);
        var stored = list.EnumerateReferences().ToArray();
        reference.Clear();

        await Assert.That(list.Count).IsEqualTo(2);
        await Assert.That(stored[0]).IsSameReferenceAs(list.Get(0));
        await Assert.That(stored[0]).IsNotSameReferenceAs(reference);
        await Assert.That(stored[0].Descriptor).IsSameReferenceAs(file);
        await Assert.That(stored[1].Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task ReferenceCollection_AddAndInsert_SelectNodesOrHandlesWithoutResolving()
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var descriptor = table.CreateFile("unloaded.Gbx");
        var reference = new NodeRef<CPlugCrystal> { Descriptor = descriptor };
        var node = new CPlugCrystal();
        var list = new NodeRefList<CPlugCrystal>();

        list.Add(null);
        list.Insert(0, null);
        list.Add(node);
        list.Insert(0, node);
        list.Add(reference);
        list.Insert(0, reference);

        await Assert.That(list.Count).IsEqualTo(6);
        await Assert.That(list.Get(0).Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(list.Get(5).Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(list.Get(0)).IsNotSameReferenceAs(reference);
        await Assert.That(list.Get(5)).IsNotSameReferenceAs(reference);
        await Assert.That(list.Get(0)).IsNotSameReferenceAs(list.Get(5));
        await Assert.That(list.Get(1).Node).IsSameReferenceAs(node);
        await Assert.That(list.Get(4).Node).IsSameReferenceAs(node);
        await Assert.That(list.Get(2).Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(list.Get(3).Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(calls).IsEqualTo(0);
        await Assert.That(() => list.Add((NodeRef<CPlugCrystal>)null!)).Throws<ArgumentNullException>();
        await Assert.That(() => list.Insert(0, (NodeRef<CPlugCrystal>)null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ReferenceCollection_RemoveAll_PreservesPredicateOrderAndSurvivingHandles()
    {
        var first = new CPlugCrystal();
        var material = new CPlugMaterial();
        var last = new CPlugCrystal();
        CMwNod?[] nodes = [first, material, null, last];
        var references = new NodeRefList<CMwNod>(nodes);
        var handles = references.EnumerateReferences().ToArray();
        var visited = new List<CMwNod?>();

        var removed = references.RemoveAll(node =>
        {
            visited.Add(node);
            return node is null or CPlugMaterial;
        });

        await Assert.That(removed).IsEqualTo(2);
        await Assert.That(visited).IsEquivalentTo(nodes, CollectionOrdering.Matching);
        await Assert.That(references.Count).IsEqualTo(2);
        await Assert.That(references.Get(0)).IsSameReferenceAs(handles[0]);
        await Assert.That(references.Get(1)).IsSameReferenceAs(handles[3]);
        await Assert.That(handles[1].Node).IsSameReferenceAs(material);
    }

    [Test]
    public async Task ReferenceTable_FactoriesAndPolymorphicImport_PreserveDescriptorMetadata()
    {
        var source = new GbxRefTable();
        var destination = new GbxRefTable();
        var file = source.CreateFile("dependency.Gbx");
        var resource = source.CreateResource(42);

        var importedFile = (GbxRefTableFile)destination.Import((GbxRefTableNode)file);
        var importedResource = (GbxRefTableResource)destination.Import((GbxRefTableNode)resource);

        await Assert.That(file.Flags).IsEqualTo(0);
        await Assert.That(resource.Flags).IsEqualTo(4);
        await Assert.That(file.UseFile).IsTrue();
        await Assert.That(resource.UseFile).IsTrue();
        await Assert.That(importedFile.FilePath).IsEqualTo(file.FilePath);
        await Assert.That(importedResource.ResourceIndex).IsEqualTo(resource.ResourceIndex);
        await Assert.That(importedFile.RefTable).IsSameReferenceAs(destination);
        await Assert.That(importedResource.RefTable).IsSameReferenceAs(destination);
        await Assert.That(file.RefTable).IsSameReferenceAs(source);
        await Assert.That(resource.RefTable).IsSameReferenceAs(source);
        await Assert.That(source.Files.Count).IsEqualTo(0);
        await Assert.That(source.Resources.Count).IsEqualTo(0);
    }

    [Test]
    public async Task ReferenceTable_InvalidFactoryArguments_AreRejected()
    {
        var table = new GbxRefTable();

        await Assert.That(() => table.CreateFile(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => table.CreateFile("file.Gbx", flags: 4)).Throws<ArgumentException>();
        await Assert.That(() => table.CreateResource(42, flags: 0)).Throws<ArgumentException>();
        await Assert.That(() => table.Import((GbxRefTableNode)null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ReferenceTable_CustomResolverMaps_ResolveAndRejectNullReplacements()
    {
        var node = new CPlugCrystal();
        var table = new GbxRefTable
        {
            ExternalNodes = new SortedDictionary<string, Func<Gbx>>(StringComparer.OrdinalIgnoreCase)
            {
                ["NODE.GBX"] = () => node.ToGbx()
            }
        };
        var reference = new NodeRef<CPlugCrystal>();
        reference.Descriptor = table.CreateFile("node.Gbx");

        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(node);
        await Assert.That(() => table.ExternalNodes = null!).Throws<ArgumentNullException>();
        await Assert.That(() => table.ExternalNodesAsync = null!).Throws<ArgumentNullException>();
        await Assert.That(reference.CachedNode).IsSameReferenceAs(node);
    }
}
