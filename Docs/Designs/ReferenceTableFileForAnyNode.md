# Design: Reference table files for any node reference

This proposal lets any node reference preserve an external Gbx file with metadata available through the owner's typed `References` view. Ordinary access should stay readable, such as `item.EntityModelEdition.MeshCrystal.Layers`.

**Status: implemented.** Reference storage, generated views, nullable resolution, collection serialization, and cloning are implemented. Scalar metadata is exposed through `References`. The API changes need a major release. There is no committed release version. See [Node references](NodeReferences.md) for usage.

## Current behavior

The implementation separates the loaded node from its reference descriptor:

- Scalar node properties project targets stored in typed reference handles.
- Reference collections use `NodeRefList<T>`, with metadata exposed through typed element handles.
- `ReadNodeRef`, `WriteNodeRef`, and combined `NodeRef` serialization preserve descriptors without resolving targets.
- Missing files return `null`. Parse errors, unsupported resolution, and incompatible loaded types propagate as errors.
- Resolution can use the file system or `GbxRefTable.ExternalNodes`. A reference is not necessarily backed by a local file.

See [GbxRefTableFile](../Src/GBX.NET/Components/GbxRefTableFile.cs), [GbxRefTable](../Src/GBX.NET/Components/GbxRefTable.cs), [External<T>](../Src/GBX.NET/External.cs), and [generated property emission](../Generators/GBX.NET.Generators/Generation/EngineWriter.cs).

## Goals and scope

The design should:

- Preserve every reference-table entry used by a node reference, including unresolved files, through their actual wire operation.
- Keep scalar node properties typed as `T?`, with no `.Node` step in ordinary access.
- Expose reference metadata and replacement operations without loading the target.
- Distinguish an absent reference from an external reference whose target is unavailable or invalid.
- Write an unresolved external reference back without loading its target.
- Keep abstract engine classes abstract and preserve the actual runtime type of loaded nodes.
- Generate direct member access and class dispatch from layouts and compiler symbols. Use `typeof(T).Name` for type-mismatch diagnostics without requiring a generated type-name string.

Here, "any node reference" means a member serialized through `NodeRef`, including collection elements and references in chunks or named archives. A directly serialized node, a `MetaRef`, or a value archive has different wire rules. Giving it a file descriptor does not make an external reference valid. The generator and writer must reject external assignment where the layout or serialization context cannot encode it.

Reference-table resources also exist. They must retain their resource index, flags, and identity, even if resolving them is outside the first implementation. An unsupported resource resolver must report an explicit unsupported operation instead of presenting the entry as an absent node.

## Why the file cannot live only on CMwNod

An alternative would attach incoming-reference metadata to each target node. That is convenient for a successfully loaded node, but it does not cover the full reference lifecycle.

| Problem | Consequence |
| --- | --- |
| A missing file has no node instance. | Metadata still needs somewhere to live before resolution. |
| A reference may be declared as an abstract base class. | Instantiating that declared type is impossible, and removing `abstract` does not reveal the file's actual class. |
| A normal node filled with defaults looks usable. | Every data getter, setter, method, and public field would need to prevent access to an unresolved placeholder. A base class cannot intercept ordinary member access. |
| Several owners can refer to the same loaded node through different files, flags, or paths. | Metadata on the target cannot describe every incoming reference. |
| Replacing a placeholder with a loaded node changes object identity. | Previously obtained placeholders and other references do not automatically follow the replacement. |
| Arrays expose stored elements directly. | Ordinary arrays cannot resolve or validate an element through a custom indexer. |

The authoritative metadata therefore belongs to the **reference held by the owner**, not to the target node. A future node-level provenance API could describe where a node was loaded from, but it must not decide how an owner serializes that node.

## Proposed reference storage

Use a reference object behind each node property. The proposed name is `NodeRef<T> where T : CMwNod`, with a non-generic `NodeRef` base for common metadata and inspection. It stores the reference kind, descriptor, resolution state, and cached target. It does not require `new T()`.

The public node property projects the resolved target. A generated, strongly typed `References` view on the owner exposes reference handles under the same member names. This keeps the wrapper out of normal property chains and groups metadata access under one property.

The following illustrates the generated shape:

```cs
private NodeRef<CMwNod> entityModelEditionRef = new();

public CMwNod? EntityModelEdition
{
    get => entityModelEditionRef.Resolve();
    set => entityModelEditionRef.SetEmbedded(value);
}
```

Reference owners include `CMwNod`, chunks, and named archives that contain node references. Access to `item.EntityModelEdition` resolves the target. Access to `item.References.EntityModelEdition` returns its existing `NodeRef<CMwNod>` directly, without evaluating the node property or starting I/O. The caller does not supply a member-name string, selector expression, or generic type argument.

For example, metadata inspection and replacement could look like this:

```cs
var reference = item.References.EntityModelEdition;

// Inspecting File does not load EntityModelEdition.
Console.WriteLine(reference.File?.FilePath);

// Change the file even when the old target is missing.
reference.SetExternal(replacementFile);

// Explicitly switch to an embedded node and clear the external descriptor.
item.EntityModelEdition = replacementNode;
```

The generated view is a readonly struct holding its owner. It adds no heap allocation when accessed and stores no copies of the member's reference state. An illustrative derived owner emits this shape alongside its node properties:

```cs
public new NodeRefView References => new(this);

public new readonly struct NodeRefView
{
    private readonly CGameItemModel owner;

    internal NodeRefView(CGameItemModel owner)
    {
        this.owner = owner;
    }

    public NodeRef<CMwNod> EntityModelEdition => owner.entityModelEditionRef;
}
```

The view is nested inside the owner so its generated properties can access private reference fields directly. Owners without an inherited view omit `new`. A derived view also exposes inherited reference members, delegating those properties through the statically selected base owner's view. The struct itself does not need to inherit. Access through a base-typed owner exposes that base type's reference members, following normal C# member access.

Each view property has the exact `NodeRef<T>` type declared by its node member. Unknown member names and incompatible handle assignments are compile-time errors. A member that cannot encode a node reference has no entry in the view. Member hiding that would make a reference ambiguous produces a generator diagnostic.

A captured view or scalar handle remains attached to that owner's member when the member is reassigned. Reassigning one member must not change another member merely because both previously referenced the same table entry. There is no string lookup, runtime type selection, member dictionary, property discovery, expression-tree inspection, or accessor compilation.

Tools that need to inspect an owner without knowing its concrete type can use generated reference enumeration with constant member names and non-generic handles. That operation returns stored references without resolving targets and does not become a member-name lookup API. The concrete typed view remains the API for selecting and editing a known member.

## Reference kind and resolution state

Keep storage kind separate from resolution state. Loading a file changes its state, not its external storage kind.

| Kind | Stored data | Scalar property access | Serialization |
| --- | --- | --- | --- |
| None | No target or descriptor | Returns `null` | Writes the null-reference marker |
| Embedded | A concrete node | Returns that node | Writes an embedded node reference |
| External file | File descriptor and optional cached target | Resolves lazily and returns the typed target, returns `null` if missing, or throws for resolution errors | Writes the file reference regardless of resolution state |
| External resource | Resource descriptor and optional cached target | Uses a supported resource resolver, or reports unsupported resolution | Writes the resource reference |

An external reference has these resolution states:

| State | Meaning |
| --- | --- |
| Unresolved | No resolution attempt has completed. |
| Resolving | A resolution attempt is in progress. |
| Resolved | A target has been loaded and validated for this member. |
| Missing | The configured resolver reported that the target is unavailable. |
| Failed | Resolution encountered a parse, I/O, unsupported-class, or type error. The error is retained. |

`Kind`, `State`, `File`, resource metadata, and inspection of an already cached target must never start I/O. A missing target retains its descriptor. A failed target retains both its descriptor and the original error.

## Access and error handling

Direct node properties and `Resolve` return nullable targets:

- An absent reference returns `null`.
- A missing external target returns `null` when the owner property resolves it. The reference retains its External file kind, Missing state, and file descriptor.
- Malformed data, access-denied errors, unsupported classes, and incompatible types remain errors. They must not be converted into a missing-file result.
- Type validation uses `node is T` against the member's declared type. A concrete subtype is accepted. For incompatible targets, diagnostics use `typeof(T).Name` and the actual Gbx class ID.

File metadata remains available through the reference handle when resolution returns `null`. `Kind` and `State` distinguish an absent reference from a missing external target. A filesystem path may be unavailable for an `ExternalNodes` resolver.

For partial exports and explorers, `TryResolve` provides an explicit success check on the specific reference:

```cs
var reference = item.References.EntityModelEdition;

if (reference.TryResolve(out var edition))
{
    Console.WriteLine(reference.State);
}
else
{
    // Kind distinguishes an absent reference from a missing external target.
    Console.WriteLine(reference.File?.FilePath);
}
```

`TryResolve` returns `false` with a `null` output only for an absent reference or a missing target. It still propagates parse, I/O, type, and unsupported-resolution errors. A caller accessing a longer property chain can use null-conditional access at each nullable node boundary.

The original `AsNullable<TResult>(Func<TNode, TResult>)` helper is removed. Nullable node access already handles absent and missing targets. Also, unconstrained `TResult?` does not reliably provide `Nullable<T>` semantics for value types, so `default` can turn unavailable data into a valid-looking zero or `false`.

## Resolution, caching, and identity

Reading the parent Gbx records external descriptors without resolving them. Ordinary node getters resolve synchronously on demand. Explicit `ResolveAsync(CancellationToken)` and `TryResolveAsync(CancellationToken)` operations provide asynchronous access through the reference handle, with the same nullable results and error handling as their synchronous counterparts. They must use an asynchronous resolver rather than requiring property access or blocking on an async task.

The resolution context carries the owner's path, ancestor level, `ExternalNodes` resolver, and read settings. Nested loads retain the relevant settings, such as size limits, game context, and logging. A public overload must not silently substitute default settings for the settings used to read the owner.

Target construction uses generated class-ID factories with direct constructor calls for concrete nodes. Handwritten node types supply an explicitly registered factory when needed. There is no `Activator.CreateInstance`, assembly scanning, dynamic member dispatch, or reflection fallback. An unknown class ID remains an unsupported-class error.

Resolution caches successful loads and terminal missing or failed outcomes. Repeated inspection of an unavailable dependency should not repeatedly hit the file system. Explicit `Retry` clears a terminal failure and attempts resolution again. Replacing the descriptor clears the member's cached target and error. Descriptors exposed by the new API must be immutable or edited through operations that perform this invalidation, since today's mutable `GbxRefTableFile.FilePath` can otherwise leave a stale cached node.

Repeated references to the same source table entry share a resolution entry and loaded target. Each owning member still retains its own declared-type check and replacement control. Do not merge distinct table entries solely because their paths match. Relative paths and flags depend on the owning table, and two entries can have different serialization intent even if a resolver returns the same object.

Concurrent resolution of one entry shares one in-flight load. Cancellation of one waiter must not cancel other callers' work or permanently mark the reference missing or failed. Reassignment during a load must prevent the old result from being published into the replacement reference. These guarantees apply to resolution, not to arbitrary concurrent edits of a node graph.

Register a successfully parsed node in the load context before traversing its external dependencies. Returning a registered node allows cycles after parsing. Re-entering an entry that is still being parsed must report a cycle or defer that resolution, rather than recursively loading it or waiting on itself.

## Assignment and serialization

Assignment has explicit meaning:

| Operation | Result |
| --- | --- |
| Assign `null` to a node property | Clears the target, descriptor, and error, producing None |
| Assign a node to a node property | Stores it as Embedded and clears any external descriptor |
| Call `SetExternal(file)` | Stores an external descriptor and invalidates previous resolution state |
| Resolve an external reference | Caches the loaded target while retaining External file kind |

Assigning a node that was loaded from a file does not implicitly copy its incoming reference metadata. To keep it external, retain the existing reference or explicitly use `SetExternal`.

The writer consumes reference storage directly. It must not call public node getters or enumerate a resolving collection to recover metadata. A missing or malformed dependency can still be referenced in the output without loading it.

Serialization and cloning use generated calls to the known reference fields. Handwritten members provide explicit serialization and clone code or a declared generation hook. Neither path discovers members at runtime. Diagnostic member names and declared type names are emitted as constants by the generator.

Layout conditions must also use stored reference state when deciding whether a reference is absent. Node null comparisons in layouts are emitted as None-kind checks. Audit handwritten conditions and generated expressions so that changing the storage model preserves branch selection without introducing I/O.

For external entries, preserve the path or resource index, flags, `UseFile`, ancestor semantics, and shared table-entry identity. Output node indices can be reassigned, but repeated references to one entry must still point to one output entry. For embedded nodes, preserve shared object identity through the node dictionary.

Editing a cached external node does not embed it or save its Gbx file when the parent is saved. Saving the dependency is a separate action. To include its data in the parent, explicitly switch that member to Embedded.

A descriptor imported from another owning table needs validation and path rebasing against the destination table's ancestor level and output location. The operation must not mutate the original table. The same validation applies when creating an external reference in a parent that previously had no reference table.

Encapsulated or direct-node serialization contexts that cannot encode an external entry must fail explicitly. Saving raw body data remains a separate path: [GbxRefTable.Files and Resources](../Src/GBX.NET/Components/GbxRefTable.cs) describe the unlinked entries used there. Changes to live member references must not be advertised as edits to a body that is being copied unchanged.

## Collections

Ordinary `T[]` and `List<T>` cannot provide lazy resolution and retain unresolved element metadata through their usual element access. A proposed `NodeRefList<T>` stores references and exposes resolved `T?` elements through its indexer. A fixed-size equivalent can cover fixed-length layouts without allowing structural changes.

The owner's typed view also exposes a metadata view for each reference collection. For example, `item.References.NadeoSkinFids[0]` returns the first element's `NodeRef<CMwNod>` without resolving it, while `item.NadeoSkinFids[0]` resolves that element. Both paths use the same underlying storage. The metadata view follows the owner's current collection if the whole collection is replaced. An element handle identifies its stored entry, so removing that entry does not retarget the handle to whichever element later occupies its index.

The collection contract needs to be consistent:

- `Count` and `GetReference(index)` do not resolve elements.
- The indexer resolves only the requested element using the scalar access rules.
- Enumeration and `CopyTo` resolve elements in order, yielding or copying `null` for absent or missing targets and throwing at an element with a resolution error.
- Element assignment follows scalar assignment rules. Metadata operations on an element handle do not load it.
- `Contains`, `IndexOf`, and `Remove(T)` compare resolved targets and may therefore perform I/O. Metadata-only edits use `GetReference(index)` and `RemoveAt(index)`.
- Serialization and cloning traverse reference storage directly, preserving null entries, unresolved descriptors, aliases, order, and fixed lengths.

A resolving iterator continues past missing targets with `null` elements. Exporters that need to distinguish absent references from missing targets can iterate indices and inspect each reference handle's `Kind` and `State`.

Replacing existing arrays, lists, and `External<T>` collections is a public API break. Snapshot conversion to an ordinary array should be an explicit operation because it resolves elements and cannot retain unresolved descriptors. Jagged collections need the same contract at each reference-bearing level.

Dictionary support is not required for the first implementation. It should be evaluated against actual layouts and their ordering rules.

## Implementation and migration

The implementation should be staged:

1. Introduce reference storage and a resolver context with tests for kind, state, type validation, aliases, and errors. Preserve the current public API while the foundation is evaluated.
2. Add combined `ReadWrite` support for reference storage in [GbxReaderWriter](../Src/GBX.NET/Serialization/GbxReaderWriter.cs), with reader and writer paths that retain metadata and never require resolution to write.
3. Update [EngineWriter](../Generators/GBX.NET.Generators/Generation/EngineWriter.cs), [SerializationWriter](../Generators/GBX.NET.Generators/Generation/SerializationWriter.cs), and [WireTypes](../Generators/GBX.NET.Generators/Generation/WireTypes.cs) to cover every `NodeRef` member, including chunk fields, named archives, collections, and conditions that inspect reference presence. Emit diagnostics for incompatible layout annotations and validate external assignments at runtime. Keep direct nodes and meta references on their existing wire paths.
4. Prototype the generated typed views and collection API with representative classes before converting layouts broadly. Verify inherited view members and captured views after reassignment. Audit handwritten getters, public fields, serializers, and member-name collisions. Tools that inspect references use generated reference enumeration and metadata handles rather than reflected properties. Use Roslyn symbols and layout data during generation, with no reflection fallback in the emitted runtime code.
5. Generate explicit clone operations for reference fields and loaded targets to preserve aliases within the cloned graph without resolving dependencies or discovering members through reflection. Clone already loaded targets using the same graph context while retaining their external kind. Unresolved, missing, failed, or in-flight external entries start Unresolved in the clone, with fresh resolution state and no copied tasks or errors.
6. Expose scalar metadata only through `References`. Remove sibling file properties and their getter adapters. Document collection migration with explicit examples.
7. Remove obsolete members and adopt the proposed public contract in a major release once generation and handwritten serialization are covered.

The ability to encode a reference-table entry comes from the actual wire operation and context.

Generated files under `Src/GBX.NET/Generated/<TargetFramework>/` are build output and must not be edited directly. Use the repository's [generator verification commands](../.agents/generator-verification.md) when implementing these changes.

## Acceptance checks

Before adopting the public API, verify these behaviors:

- Null, embedded, unresolved external, resolved external, missing external, and failed external references preserve their storage kind through a read/write cycle.
- Reading, metadata inspection, cloning, and writing do not invoke an external resolver. Direct access invokes it lazily and only once per shared entry until an explicit retry or replacement.
- Missing files and absent resolver entries return `null` through scalar properties, collection access, and synchronous or asynchronous resolution, while remaining inspectable and serializable. `TryResolve` and `TryResolveAsync` report failure with a null target. Malformed files, incompatible classes, permission errors, and unsupported resources keep distinct errors.
- Abstract declared types resolve to valid concrete subtypes without placeholder instances or changes to abstract class declarations.
- Repeated references preserve aliases. Two references with different descriptors can share a loaded node without overwriting each other's metadata or output entries.
- Reassignment, retry, cancellation, concurrent loads, and external cycles follow the stated cache and identity rules.
- Scalar properties, chunk fields, named archives, variable and fixed-size collections, and jagged collections retain external metadata and wire ordering.
- Importing descriptors across tables preserves the intended location and flags. Encapsulation rejects unsupported external output, and raw-body writes keep their separate editing rules.
- Assigning a node clears a previous external descriptor, while resolving or editing an external target never silently changes the parent's storage kind.
- Generated typed views expose inherited members and reject ambiguous or reserved names during generation. Invalid member access and mismatched handle assignments fail at compile time. Accessing or capturing a view never evaluates a node getter.
- Captured scalar views and handles follow member reassignment. Collection metadata views follow collection replacement, while element handles retain their entry identity across index changes or removal.
- Reference access, resolution dispatch, serialization, cloning, and inspection run using generated accessors and factories in a trimmed or Native AOT test application. Type-mismatch diagnostics read `typeof(T).Name`. Member access and construction require no reflected member lookup, dynamic code generation, or reflection fallback.

The remaining prototype decisions are collection metadata-view interfaces, descriptor construction and rebasing API, resource resolver support, and the allocation cost of reference storage. The scalar access shape is `owner.References.Member`, with the declared type supplied by generation. The remaining decisions need implementation evidence before the release scope is fixed.
