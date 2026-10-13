using GBX.NET.Components;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class NodeRefApiTests
{
    [Test]
    public async Task UntypedReference_Resolution_UsesTheTypedHandlesSharedCache()
    {
        var table = new GbxRefTable();
        var target = new CPlugCrystal();
        var calls = 0;
        table.ExternalNodes["shared.Gbx"] = () => { calls++; return target.ToGbx(); };
        var typed = new NodeRef<CPlugCrystal>();
        GbxRefTableNode descriptor = table.CreateFile("shared.Gbx");
        NodeRef reference = typed;
        reference.SetExternal(descriptor);

        await Assert.That(reference.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(reference.Descriptor!.RefTable).IsSameReferenceAs(table);
        await Assert.That(reference.Resolve()).IsSameReferenceAs(target);
        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(target);
        await Assert.That(reference.TryResolve(out var resolved)).IsTrue();
        await Assert.That(resolved).IsSameReferenceAs(target);
        var result = await reference.TryResolveAsync();
        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Target).IsSameReferenceAs(target);
        await Assert.That(typed.CachedTarget).IsSameReferenceAs(target);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task UntypedReference_IncompatibleTarget_PreservesTypeValidation(bool asynchronous)
    {
        var table = new GbxRefTable();
        table.ExternalNodes["crystal.Gbx"] = () => new CPlugCrystal().ToGbx();
        var typed = new NodeRef<CPlugMaterial>();
        typed.SetExternal(table.CreateFile("crystal.Gbx"));
        NodeRef reference = typed;

        if (asynchronous)
            await Assert.That(async () => await reference.TryResolveAsync()).Throws<InvalidCastException>();
        else
            await Assert.That(() => reference.TryResolve(out _)).Throws<InvalidCastException>();

        await Assert.That(reference.State).IsEqualTo(NodeRefState.Failed);
        await Assert.That(reference.CachedTarget).IsNull();
        await Assert.That(reference.Error).IsTypeOf<InvalidCastException>();
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
        first.SetExternal(file);
        second.SetExternal(file);
        first.Resolve();
        second.Resolve();
        NodeRef reference = first;

        reference.Reset();

        await Assert.That(reference.Descriptor).IsSameReferenceAs(file);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Unresolved);
        await Assert.That(reference.CachedTarget).IsNull();
        await Assert.That(second.CachedTarget).IsNull();
        await Assert.That(calls).IsEqualTo(1);
        table.ExternalNodes[file.FilePath] = () => { calls++; return replacement.ToGbx(); };
        await Assert.That(await reference.RetryAsync()).IsSameReferenceAs(replacement);
        await Assert.That(second.Resolve()).IsSameReferenceAs(replacement);
        await Assert.That(calls).IsEqualTo(2);
        reference.Clear();
        await Assert.That(reference.Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(reference.Descriptor).IsNull();
        await Assert.That(reference.Resolve()).IsNull();
        await Assert.That(second.CachedTarget).IsSameReferenceAs(replacement);
        await Assert.That(second.File).IsSameReferenceAs(file);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task TryResolveAsync_AbsentOrMissingTarget_ReturnsAnUnsuccessfulNamedResult(bool external)
    {
        var reference = new NodeRef<CPlugCrystal>();
        if (external) reference.SetExternal(new GbxRefTable().CreateFile("missing.Gbx"));

        NodeRefResolveResult<CPlugCrystal> result = await reference.TryResolveAsync();

        await Assert.That(result.Success).IsFalse();
        await Assert.That(result.Target).IsNull();
        await Assert.That(reference.State).IsEqualTo(external ? NodeRefState.Missing : NodeRefState.Unresolved);
        await Assert.That(default(NodeRefResolveResult<CPlugCrystal>).Success).IsFalse();
    }

    [Test]
    public async Task TryResolveAsync_ResolvedTarget_ReturnsTheTypedTarget()
    {
        var reference = new NodeRef<CPlugCrystal>();
        var target = new CPlugCrystal();
        reference.SetEmbedded(target);

        NodeRefResolveResult<CPlugCrystal> result = await reference.TryResolveAsync();

        await Assert.That(result.Success).IsTrue();
        await Assert.That(result.Target).IsSameReferenceAs(target);
    }

    [Test]
    public async Task TryResolveAsync_ResolverFailure_PropagatesAndCachesTheError()
    {
        var table = new GbxRefTable();
        var failure = new InvalidDataException("Malformed dependency");
        var calls = 0;
        table.ExternalNodes["bad.Gbx"] = () => { calls++; throw failure; };
        var reference = new NodeRef<CPlugCrystal>();
        reference.SetExternal(table.CreateFile("bad.Gbx"));

        await Assert.That(async () => await reference.TryResolveAsync()).Throws<InvalidDataException>();
        await Assert.That(async () => await reference.TryResolveAsync()).Throws<InvalidDataException>();
        await Assert.That(reference.Error).IsSameReferenceAs(failure);
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task UntypedReference_CanceledRetry_PreservesTheExistingCache()
    {
        var table = new GbxRefTable();
        var target = new CPlugCrystal();
        table.ExternalNodes["target.Gbx"] = () => target.ToGbx();
        var typed = new NodeRef<CPlugCrystal>();
        typed.SetExternal(table.CreateFile("target.Gbx"));
        typed.Resolve();
        NodeRef reference = typed;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.That(async () => await reference.RetryAsync(cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(async () => await reference.TryResolveAsync(cancellation.Token)).Throws<OperationCanceledException>();
        await Assert.That(reference.CachedTarget).IsSameReferenceAs(target);
        await Assert.That(reference.State).IsEqualTo(NodeRefState.Resolved);
    }

    [Test]
    public async Task ReferenceCollection_EnumeratingAndInsertingMetadata_DoesNotResolveTargets()
    {
        var table = new GbxRefTable();
        var calls = 0;
        table.ExternalNodes["unloaded.Gbx"] = () => { calls++; throw new Exception("Must not resolve"); };
        var file = table.CreateFile("unloaded.Gbx");
        var reference = new NodeRef<CMwNod>();
        reference.SetExternal(file);
        var list = new NodeRefList<CMwNod> { null };

        list.InsertReference(0, reference);
        IReadOnlyList<NodeRef<CMwNod>> view = list.References;
        var stored = view.ToArray();
        reference.Clear();

        await Assert.That(view.Count).IsEqualTo(2);
        await Assert.That(stored[0]).IsSameReferenceAs(list.GetReference(0));
        await Assert.That(stored[0]).IsNotSameReferenceAs(reference);
        await Assert.That(stored[0].Descriptor).IsSameReferenceAs(file);
        await Assert.That(stored[1].Kind).IsEqualTo(NodeRefKind.None);
        await Assert.That(calls).IsEqualTo(0);
        var fixedList = NodeRefList<CMwNod>.CreateFixed(1);
        await Assert.That(() => fixedList.InsertReference(0, stored[0])).Throws<NotSupportedException>();
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
        var target = new CPlugCrystal();
        var table = new GbxRefTable
        {
            ExternalNodes = new SortedDictionary<string, Func<Gbx>>(StringComparer.OrdinalIgnoreCase)
            {
                ["TARGET.GBX"] = () => target.ToGbx()
            }
        };
        var reference = new NodeRef<CPlugCrystal>();
        reference.SetExternal(table.CreateFile("target.Gbx"));

        await Assert.That(await reference.ResolveAsync()).IsSameReferenceAs(target);
        await Assert.That(() => table.ExternalNodes = null!).Throws<ArgumentNullException>();
        await Assert.That(() => table.ExternalNodesAsync = null!).Throws<ArgumentNullException>();
        await Assert.That(reference.CachedTarget).IsSameReferenceAs(target);
    }
}
