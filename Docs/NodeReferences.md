# Node references

Node properties retain their reference-table descriptors and resolve external nodes when accessed. A missing node returns `null`. Parsing, I/O, unsupported-resolution, and type errors still propagate.

## Inspecting and replacing references

Use the owner's generated `References` view to inspect metadata without loading a node:

```cs
var reference = item.References.EntityModelEdition;
Console.WriteLine(reference.File?.FilePath);
Console.WriteLine(reference.Kind);
Console.WriteLine(reference.State);

reference.Descriptor = replacementFile;
var edition = item.EntityModelEdition;
```

`Descriptor`, `File`, `Resource`, `Kind`, `State`, and `Exception` never start resolution. `Descriptor.RefTable` exposes the table that owns an external descriptor. An unavailable external file retains its descriptor and has `Missing` state, so it remains distinct from an absent reference.

Assigning an owner's node property or a reference handle's `Node` stores an embedded node and clears its external descriptor. Assigning `Descriptor` stores an external reference without loading it and invalidates the member's cached node. Assigning `null` to either property clears the reference. `CopyFrom` copies reference storage into an existing handle without resolving either node. A captured handle remains attached to its owning member after reassignment. Assignments through an untyped handle still validate its declared node type before replacing stored data.

Scalar reference handles on nodes start as null. Reading an empty node property, assigning null, generated serialization, and cloning preserve unallocated storage. A non-null node, a stored external descriptor, or explicit metadata access through `References` or `EnumerateReferences()` creates a handle on demand. Once created, the handle stays attached to its property, including when the property is cleared.

Changing `GbxRefTableFile.FilePath` invalidates the shared node, missing result, and errors for every handle using that descriptor. Assigning the same path preserves the cache. Null paths and changes during resolution are rejected. To change only one handle, assign a replacement to its `Descriptor`.

`Clear()` removes the node and descriptor from one handle. `Reset()` keeps the descriptor and invalidates its shared resolution cache without loading anything. Other handles using that descriptor also see the invalidation. A descriptor cannot be reset while its load is running.

## Reference tables

Create descriptors through a table, then attach them to a stored reference:

```cs
var table = new GbxRefTable();
var file = table.CreateFile("dependency.Gbx");
var resource = table.CreateResource(resourceIndex: 42);
reference.Descriptor = file;
```

`CreateFile` and `CreateResource` provide the usual flags and `UseFile` defaults. They do not load a node or add entries to the header preview. `Descriptor` accepts the common `GbxRefTableNode` type, so metadata tools can work with either descriptor kind.

`destinationTable.Import(descriptor)` copies a descriptor into another table. The typed overloads retain the file or resource type. File paths are rebased when both filesystem locations are known, and importing never modifies the source. Rebasing that needs an unknown filesystem location throws an exception.

`Files` and `Resources` contain unlinked header-preview entries. Changing those entries affects saves with a raw body. Normal saves rebuild the table from the descriptors stored in the node's references.

Configure `ReadSettings` and `FileSystemPath` when constructing a table. The synchronous and asynchronous resolver maps accept `IDictionary` implementations and can be populated or replaced. Changing a resolver or `AncestorLevel` does not invalidate existing cached results. Use `Reset` or `Retry` on the affected handles when you want to resolve their descriptors with the changed table settings.

## Resolution

`Node` and `Resolve()` share the same behavior: they resolve on first access and reuse the shared cached result until it is invalidated. Both return `null` for an absent or missing node and propagate resolution or type errors. `TryResolve` returns `false` for an absent or missing node, with a null output. Cache-only access stays internal for serialization and cloning.

```cs
Console.WriteLine(reference.Node);

if (reference.TryResolve(out var node))
{
    Console.WriteLine(node);
}
else
{
    Console.WriteLine(reference.File?.FilePath);
}

if (await reference.ResolveAsync(cancellationToken) is { } asyncNode)
{
    Console.WriteLine(asyncNode);
}
```

`ResolveAsync` returns the nullable node directly. A non-null node means resolution succeeded. Resolver, type, and cancellation errors propagate. Cache generations stay internal.

Filesystem resolution retains the owner's read settings. For custom resolution, synchronous calls use `GbxRefTable.ExternalNodes`. Asynchronous calls prefer `ExternalNodesAsync` and fall back to `ExternalNodes`. A synchronous fallback runs directly and can block until it returns. A synchronous call with only an asynchronous resolver throws `NotSupportedException`.

Tools can use the same resolution, retry, reset, and clear methods on an untyped `NodeRef`, including the handles returned by `EnumerateReferences()`. Resolution through an untyped handle still checks the stored handle's declared node type.

References to one table entry share a load. Success, missing nodes, and errors are cached until `Reset`, `Retry`, `RetryAsync`, or a path change. Replacing a descriptor attaches the handle to that descriptor's cache. Retrying a shared entry invalidates its members' previous type checks. Canceling an asynchronous waiter leaves the shared load available to other callers. Resolver callbacks receive `CancellationToken.None` because an individual waiter's token does not control the shared load.

External resources retain their index, flags, and table identity. They can be inspected, cloned, and written. Resolving a resource currently throws `NotSupportedException`.

## Collections and archives

Node-reference collections use `NodeRefList<T>`. Indexing, enumeration, and `CopyTo` resolve elements and return null elements for absent or missing nodes. `Count`, `Get`, and `EnumerateReferences()` do not resolve elements.

```cs
var first = item.NadeoSkinFids?[0];
var firstReference = item.References.NadeoSkinFids?[0];
Console.WriteLine(firstReference?.File?.FilePath);

foreach (var stored in item.References.NadeoSkinFids ?? [])
{
    Console.WriteLine(stored.Descriptor);
}
```

`Get(index)` returns an element's handle, and `EnumerateReferences()` iterates the stored handles without loading nodes. `Add(node)` and `Insert(index, node)` store embedded nodes, while overloads taking `NodeRef<T>` copy reference storage into a new element handle without loading anything.

The owner's `References` view exposes collections as `IReadOnlyList<NodeRef<T>>?`. Indexing and enumerating these lists return stored handles without resolving nodes. Each list uses a cached read-only wrapper that follows insertion and removal. The handles themselves remain writable. A null collection returns null without creating a list.

With C# 13 or later, [overload resolution priority](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-13.0/overload-resolution-priority) makes `Add(null)` and `Insert(index, null)` select the node overload and create an absent reference. Earlier compilers need an explicit node type, such as `Add((T?)null)`, to distinguish the overloads. Passing a typed null handle throws `ArgumentNullException`.

Accessing an owner's collection property returns its current list without resolving elements. A captured list remains that list if the owner replaces it. An element handle keeps its entry identity when an element is removed or its index changes. Reference lists allow insertion and removal regardless of the wire format's length. `ToArray` creates a resolved snapshot and does not retain external metadata.

Generated views also cover references in chunks and named archives. Jagged reference collections expose each row's handle list through `References`, projecting the current rows on access without resolving nodes. Each row's wrapper remains live for its source list. Handwritten dictionary serializers such as `CGameCtnDecorationAudio.Sounds` and `Musics` keep `NodeRef<T>` values directly.

`CGameCtnMediaClipGroup.Clips` keeps its combined `List<ClipTrigger>` API. Each pair exposes its clip metadata through `pair.References.Clip` and keeps the trigger with its clip when entries are reordered.

## Writing and cloning

Writing traverses stored references without resolving dependencies. Unresolved, missing, and failed external descriptors remain writable, including references assigned to a Gbx that originally had no reference table. Encapsulation and inline formats reject external references that their wire format cannot encode.

Cloning preserves aliases and copies already loaded nodes without resolving dependencies. Unresolved, missing, failed, and in-flight references start unresolved in the clone. Loaded dependency cycles and embedded references to the root retain their identity.

The generator emits direct accessors, class IDs, factories, enumeration, and clone operations. Reference access does not use member-name lookup or runtime reflection. Type-mismatch diagnostics read `typeof(T).Name`. Tools can inspect stored handles through `EnumerateReferences()`.

## Migration

Scalar node properties keep their nullable node types. Node-reference arrays, lists, and `External<T>` collections now use `NodeRefList<T>`.

Use `reference.Node = node` in place of `SetEmbedded(node)` and `reference.Descriptor = descriptor` in place of `SetExternal(descriptor)`. Both assignments preserve the handle's identity. Reading `Node` resolves on demand, while reading `Descriptor` only inspects metadata.

Arrays and lists of embedded nodes can initialize a reference collection. An obsolete conversion from an `External<T>[]` retains its descriptors. Use the typed metadata view for new code, and use an explicit resolved snapshot when an ordinary array is needed.

The old `GbxRefTable.LoadNode` overloads, `GbxRefTableFile.GetNode(ref ...)`, and `External<T>.GetNode(settings)` have been removed. Use a stored reference's `Resolve` or `ResolveAsync`, and configure its table's `ReadSettings`. `ImportFile` and `ImportResource` are replaced by the `Import` overloads. Asynchronous resolution returns the nullable node directly.

Handwritten serializers use `ReadNodeRef`, `WriteNodeRef`, and combined `GbxReaderWriter.NodeRef`. Collections use the corresponding `ListNodeRef` operations. These APIs preserve descriptors for every node reference. Direct nodes, meta references, and inline archives keep their own wire formats.

For a direct node stored in a nullable reference handle, use `rw.Node(ref reference)`. Reading stores the inline node without replacing an existing handle. Writing uses the cached node without resolving external dependencies or changing the handle's metadata.

Pass an existing handle to `ReadNodeRef`, or pass `null` to create one. `ReadListNodeRef<T>()` reads a length-prefixed list and retains descriptors. Supplying a length omits the length prefix and reads that many entries. The resulting list remains resizable:

```cs
var reference = reader.ReadNodeRef<CMwNod>(null);
var refs = reader.ReadListNodeRef<CMwNod>();
writer.WriteNodeRef(reference);
writer.WriteListNodeRef(refs);
```

`WriteListNodeRef(refs, length: count)` writes exactly that many entries, padding with null references or omitting extra elements as needed. It does not resize the list. `ReadJaggedListNodeRef<T>()` returns an array of resizable reference lists, with optional inner and outer wire lengths.

Use `reader.ReadNodeRef(ref reference)` or combined `rw.NodeRef(ref reference)` for nullable scalar storage. A null wire reference keeps unallocated storage null and clears an existing handle without replacing it. Writing nullable storage emits the null marker without creating a handle.

The [design document](Designs/ReferenceTableFileForAnyNode.md) describes the storage and migration decisions.
