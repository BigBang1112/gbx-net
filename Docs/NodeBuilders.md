# Generated node builders

GBX.NET generates fluent builders and game version interfaces from node layouts and their game metadata. Ordinary nodes need no handwritten builder, interface, or `Create()` method.

## Creating a node

Select a game before building. `Build()` returns the concrete node:

```cs
using GBX.NET.Engines.Game;

CGameCtnMediaClip clip = CGameCtnMediaClip.Create()
    .WithName("Intro")
    .ForTMF()
    .WithLocalPlayerClipEntIndex(-1)
    .Build();
```

The builder creates the game's chunks and uses the existing node and chunk defaults. Explicit assignments override those defaults. Common setters work before and after game selection. Game-specific setters are available on the corresponding stage.

Selectors match `GameVersion` names. `ForTM2()` and `ForSM()` are aliases for `ForMP4()`. All three return the same MP4 stage and select the same format. The historical `ForTMUF()` selectors remain as handwritten obsolete aliases for `ForTMF()` on the migrated v1.2 builders.

## Building through a game interface

Use `BuildInterface()` to keep subsequent access scoped to the selected game:

```cs
using GBX.NET.Interfaces.Game;
using GBX.NET.Engines.Game;

IGameCtnMediaTrackTMF track = CGameCtnMediaTrack.Create()
    .ForTMF()
    .WithName("Camera")
    .BuildInterface();

IGameCtnMediaClipTMF clip = CGameCtnMediaClip.Create()
    .ForTMF()
    .WithTracks(track)
    .BuildInterface();

Console.WriteLine(clip.Tracks[0].Name);
```

`Build()` and `BuildInterface()` perform the same construction and validation. Each call creates a fresh node. Scoped collection views follow the owner's current collection and resolve individual external references only when elements are accessed.

The generated interfaces retain `IClassVersion<T>` operations such as `Save()` and `ToGbx()`. Existing handwritten game-specific methods remain available through their partial interfaces. Public methods annotated with `MethodGameVersion` can also be projected automatically when their context is unambiguous.

Derived node interfaces inherit their base node's interfaces for the same game. For example, an `IGameCtnMediaBlockTextTMF` can be supplied to a track's `WithBlocks()` method as an `IGameCtnMediaBlockTMF`. Shared file operations remain unambiguous through the derived view.

## Reusing builders and references

Choosing a game snapshots the common pending assignments. Changing the original builder afterwards does not change the selected stage. Repeated builds create independent collection containers while sharing supplied child nodes. Assignments run in caller order, including repeated assignments to overlapping properties such as `Flags` and `IsGround`.

Collection inputs and reference handles are copied without resolving external files. A scalar `WithEffectReference(NodeRef<CControlEffectSimi>)` setter preserves its descriptor, while `WithEffect(CControlEffectSimi?)` assigns an embedded node. An overload accepting `NodeRefList<T>` preserves the collection's reference descriptors.

Validation rejects known incompatible chunks in supplied child graphs and handles cycles by node identity. Unresolved external references retain deferred compatibility. Access through a scoped child projection validates a resolved child for that game. Validation describes the graph at the time of the check. Callers can still modify nodes afterwards.

## Customizing generated builders

Add convenience methods or optional customization hooks in partials. For example:

```cs
namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public partial class TMF
    {
        public TMF KeepsPlaying() => WithIsKeepPlaying(true);

        partial void CustomizeNode(CGameCtnMediaTrack node)
        {
            // Apply domain-specific values before final validation.
        }
    }
}
```

The existing track builder already supplies `KeepsPlaying()` before game selection. This example illustrates how to add a convenience to a selected stage.

`CustomizeNode` runs after pending assignments. Required chunks and their versions are checked afterwards. `ValidateNode` can add domain validation. A failed build leaves the pending state available for another build. Hooks should avoid mutating shared child nodes if those changes must not survive a failure.

The outer builder and game stages share a generated generic base for common setters. A handwritten method replacing an inherited common setter can call `base.WithName(value)` to retain the generated assignment behavior.

Stages also share bases for overlapping sets of game-specific setters. Assignment descriptors are shared by the property's declaring node, including its derived builders. Matching chunk recipes share creation and validation methods, while game-specific defaults and partial hooks still receive their selected context. Descriptors initialize on demand so an unused setter can be trimmed.

Game interfaces reuse inherited members and shared member contracts. Generic contracts carry each game's child interface types. Collection adapters share conversion and validation code, follow collection replacements, and resolve references only on access. Use the named node interfaces, game interfaces, and concrete builder stages in application signatures. Generated member-group type names can change with the metadata.

If a member stops being common, its generated `Set<Property>(state, value)` helper remains available while at least one game supports it. An obsolete method in the outer builder partial can use that helper to preserve pending assignments. Building for an unsupported game then reports the incompatible member.

## Exceptional creation recipes

Most builders use the game's verified chunk writer qualifiers. When that selection needs an explicit override, add `NodeBuilderRecipe` to the node partial. The supplied types replace the inferred chunk selection for that game, including inherited and header chunks:

```cs
using GBX.NET.Attributes;

namespace GBX.NET.Engines.Game;

[NodeBuilderRecipe(GameVersion.TMF,
    typeof(CGameCtnMediaClip.Chunk03079004),
    typeof(CGameCtnMediaClip.Chunk03079005),
    typeof(CGameCtnMediaClip.Chunk03079007))]
public partial class CGameCtnMediaClip;
```

This example records the clip's existing TMF selection. Use an override only when the default recipe needs correction. Invalid contexts, duplicate declarations, unknown chunk types, and chunks without writable implementations are diagnosed. An override does not add writer support to a chunk that cannot serialize its payload.

Unqualified chunks do not establish support for every game. Nodes with incomplete recipes can have generated interfaces without a callable builder for the affected game. The generated `Builders/NodeBuilderCoverage.g.cs` contains a deterministic summary of missing game metadata and incomplete recipes.

| Diagnostic | Meaning |
| --- | --- |
| `GBXNETGEN300` | Invalid game API declaration, recipe metadata, or builder customization. |
| `GBXNETGEN301` | A game recipe contains a chunk without a writable implementation. |
| `GBXNETGEN302` | A generic or version-scoped method needs an explicit game interface contract. |

Native editor modes, domain requirements such as minimum key counts, and arbitrary handwritten setter semantics cannot all be inferred from chunk qualifiers. Keep reusable serialization facts in layouts, and use recipe overrides or partials for the exceptional behavior.

See the [design and v1.2 migration inventory](Designs/GeneratedNodeBuilders.md) for compatibility decisions.
