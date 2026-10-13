# Node references

Node properties retain their reference-table descriptors and resolve external targets when accessed. A missing target returns `null`. Parsing, I/O, unsupported-resolution, and type errors still propagate.

## Inspecting and replacing references

Use the owner's generated `References` view to inspect metadata without loading a target:

```cs
var reference = item.References.EntityModelEdition;
Console.WriteLine(reference.File?.FilePath);
Console.WriteLine(reference.Kind);
Console.WriteLine(reference.State);

reference.SetExternal(replacementFile);
var edition = item.EntityModelEdition;
```

`Descriptor`, `File`, `Resource`, `Kind`, `State`, `Error`, and `CachedTarget` never start resolution. `Descriptor.RefTable` exposes the table that owns an external descriptor. An unavailable external file retains its descriptor and has `Missing` state, so it remains distinct from an absent reference.

Assigning a node property stores an embedded node and clears its external descriptor. Assigning `null` clears the reference. `SetExternal` replaces the descriptor and invalidates the member's cached target. `CopyFrom` copies reference storage into an existing handle without resolving either target. A captured handle remains attached to its owning member after reassignment.

Changing `GbxRefTableFile.FilePath` invalidates the shared target, missing result, and errors for every handle using that descriptor. Assigning the same path preserves the cache. Null paths and changes during resolution are rejected. To change only one handle, create a replacement descriptor and use `SetExternal`.

`Clear()` removes the target and descriptor from one handle. `Reset()` keeps the descriptor and invalidates its shared resolution cache without loading anything. Other handles using that descriptor also see the invalidation. A descriptor cannot be reset while its load is running.

## Reference tables

Create descriptors through a table, then attach them to a stored reference:

```cs
var table = new GbxRefTable();
var file = table.CreateFile("dependency.Gbx");
var resource = table.CreateResource(resourceIndex: 42);
reference.SetExternal(file);
```

`CreateFile` and `CreateResource` provide the usual flags and `UseFile` defaults. They do not load a target or add entries to the header preview. `SetExternal` accepts the common `GbxRefTableNode` type, so metadata tools can work with either descriptor kind.

`destinationTable.Import(descriptor)` copies a descriptor into another table. The typed overloads retain the file or resource type. File paths are rebased when both filesystem locations are known, and importing never modifies the source. Rebasing that needs an unknown filesystem location throws an exception.

`Files` and `Resources` contain unlinked header-preview entries. Changing those entries affects saves with a raw body. Normal saves rebuild the table from the descriptors stored in the node's references.

Configure `ReadSettings` and `FileSystemPath` when constructing a table. The synchronous and asynchronous resolver maps accept `IDictionary` implementations and can be populated or replaced. Changing a resolver or `AncestorLevel` does not invalidate existing cached results. Use `Reset` or `Retry` on the affected handles when you want to resolve their descriptors with the changed table settings.

## Resolution

`Resolve` follows the same nullable behavior as a node property. `TryResolve` returns `false` for an absent or missing target, with a null output. Other errors propagate from both methods.

```cs
if (reference.TryResolve(out var node))
{
    Console.WriteLine(node);
}
else
{
    Console.WriteLine(reference.File?.FilePath);
}

var resolved = await reference.ResolveAsync(cancellationToken);
var result = await reference.TryResolveAsync(cancellationToken);
if (result.Success)
{
    Console.WriteLine(result.Target);
}
```

`TryResolveAsync` returns `NodeRefResolveResult<T>`, with `Success` and `Target` properties. Its default value represents an absent target. Resolver, type, and cancellation errors propagate. Neither public resolution method nor its result exposes cache generations.

Filesystem resolution retains the owner's read settings. For custom resolution, synchronous calls use `GbxRefTable.ExternalNodes`. Asynchronous calls prefer `ExternalNodesAsync` and fall back to `ExternalNodes`. A synchronous fallback runs directly and can block until it returns. A synchronous call with only an asynchronous resolver throws `NotSupportedException`.

Tools can use the same resolution, retry, reset, and clear methods on an untyped `NodeRef`, including the handles returned by `EnumerateReferences()`. Resolution through an untyped handle still checks the stored handle's declared target type.

References to one table entry share a load. Success, missing targets, and errors are cached until `Reset`, `Retry`, `RetryAsync`, or a path change. Replacing a descriptor attaches the handle to that descriptor's cache. Retrying a shared entry invalidates its members' previous type checks. Canceling an asynchronous waiter leaves the shared load available to other callers. Resolver callbacks receive `CancellationToken.None` because an individual waiter's token does not control the shared load.

External resources retain their index, flags, and table identity. They can be inspected, cloned, and written. Resolving a resource currently throws `NotSupportedException`.

## Collections and archives

Node-reference collections use `NodeRefList<T>`. Indexing, enumeration, and `CopyTo` resolve elements and return null elements for absent or missing targets. `Count`, `GetReference`, and the collection's `References` view do not resolve elements.

```cs
var first = item.NadeoSkinFids?[0];
var firstReference = item.References.NadeoSkinFids[0];
Console.WriteLine(firstReference.File?.FilePath);

foreach (var stored in item.NadeoSkinFids!.References)
{
    Console.WriteLine(stored.Descriptor);
}
```

`NodeRefList<T>.References` is an enumerable read-only view of the stored handles. Enumerating it does not resolve targets. `AddReference` and `InsertReference` copy reference storage into a new element handle without loading anything.

An owner's collection metadata view follows replacement of the whole collection. An element handle keeps its entry identity when an element is removed or its index changes. Fixed collections allow element replacement and reject structural changes. `ToArray` creates a resolved snapshot and does not retain external metadata.

Generated views also cover references in chunks and named archives. Jagged reference collections expose metadata at each reference-bearing level. Handwritten dictionary serializers such as `CGameCtnDecorationAudio.Sounds` and `Musics` keep `NodeRef<T>` values directly.

`CGameCtnMediaClipGroup.Clips` keeps its combined `List<ClipTrigger>` API. Each pair exposes its clip metadata through `pair.References.Clip` and keeps the trigger with its clip when entries are reordered.

## Writing and cloning

Writing traverses stored references without resolving dependencies. Unresolved, missing, and failed external descriptors remain writable, including references assigned to a Gbx that originally had no reference table. Encapsulation and inline formats reject external references that their wire format cannot encode.

Cloning preserves aliases and copies already loaded targets without resolving dependencies. Unresolved, missing, failed, and in-flight references start unresolved in the clone. Loaded dependency cycles and embedded references to the root retain their identity.

The generator emits direct accessors, class IDs, factories, enumeration, and clone operations. Reference access does not use member-name lookup or runtime reflection. Type-mismatch diagnostics read `typeof(T).Name`. Tools can inspect stored handles through `EnumerateReferences()`.

## Migration

Scalar node properties keep their nullable node types. Node-reference arrays, lists, and `External<T>` collections now use `NodeRefList<T>`.

Arrays and lists of embedded nodes can initialize a reference collection. An obsolete conversion from an `External<T>[]` retains its descriptors. Use the typed metadata view for new code, and use an explicit resolved snapshot when an ordinary array is needed.

The old `GbxRefTable.LoadNode` overloads, `GbxRefTableFile.GetNode(ref ...)`, and `External<T>.GetNode(settings)` have been removed. Use a stored reference's `Resolve` or `ResolveAsync`, and configure its table's `ReadSettings`. `ImportFile` and `ImportResource` are replaced by the `Import` overloads. Async try-resolution now returns a named result instead of a tuple.

Handwritten serializers use `ReadNodeRef`, `WriteNodeRef`, and combined `GbxReaderWriter.NodeRef`. Collections use the corresponding `ListNodeRef` operations. These APIs preserve descriptors for every node reference. Direct nodes, meta references, and inline archives keep their own wire formats.

Pass an existing handle to `ReadNodeRef`, or pass `null` to create one. `ReadListNodeRef<T>()` reads a length-prefixed list and retains descriptors. Supplying a length omits the length prefix and creates a fixed-size list:

```cs
var reference = reader.ReadNodeRef<CMwNod>(null);
var refs = reader.ReadListNodeRef<CMwNod>();
writer.WriteNodeRef(reference);
writer.WriteListNodeRef(refs);
```

The [design document](Designs/ReferenceTableFileForAnyNode.md) describes the storage and migration decisions.
