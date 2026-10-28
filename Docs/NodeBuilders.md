# Generated node builders

GBX.NET generates fluent builders and game version interfaces from node layouts and their game metadata. Ordinary nodes need no handwritten builder, interface, or `Create()` method.

The builders are in the separate `GBX.NET.Builders` package, so an application that only reads and writes Gbx files does not carry them. The game version interfaces, such as `IGameCtnMediaClipTMF`, stay in the main `GBX.NET` package.

```powershell
dotnet add package GBX.NET.Builders
```

## Creating a node

The package adds a static `Create()` extension member to each node with a builder. It is declared in the node's namespace, so `using GBX.NET.Engines.Game;` is enough. Select a game before building. `Build()` returns the concrete node:

```cs
using GBX.NET.Engines.Game;

CGameCtnMediaClip clip = CGameCtnMediaClip.Create()
    .WithName("Intro")
    .ForTMF()
    .WithLocalPlayerClipEntIndex(-1)
    .Build();
```

The builder creates the game's chunks and uses the existing node and chunk defaults. Explicit assignments override those defaults. Common setters work before and after game selection. Game-specific setters are available on the corresponding stage.

Configure generated builders through `With<Property>(value)` methods. Generated builder properties are omitted to reduce assembly size. Node and game interface properties remain available on the result of `Build()` or `BuildInterface()`.

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

`Build()` and `BuildInterface()` perform the same construction. Each call creates a fresh node. Scoped collection views follow the owner's current collection and resolve individual external references only when elements are accessed.

The generated interfaces retain `IClassVersion<T>` operations such as `Save()` and `ToGbx()`. Existing handwritten game-specific methods remain available through their partial interfaces. Public methods annotated with `MethodGameVersion` can also be projected automatically when their context is unambiguous.

Derived node interfaces inherit their base node's interfaces for the same game. For example, an `IGameCtnMediaBlockTextTMF` can be supplied to a track's `WithBlocks()` method as an `IGameCtnMediaBlockTMF`. Shared file operations remain unambiguous through the derived view.

## Reusing builders and references

Choosing a game snapshots the common pending assignments. Changing the original builder afterwards does not change the selected stage. Repeated builds create independent collection containers while sharing supplied child nodes. Assignments run in caller order, including repeated assignments to overlapping properties such as `Flags` and `IsGround`.

Collection inputs and reference handles are copied without resolving external files. A scalar `WithEffectReference(NodeRef<CControlEffectSimi>)` setter preserves its descriptor, while `WithEffect(CControlEffectSimi?)` assigns an embedded node. An overload accepting `NodeRefList<T>` preserves the collection's reference descriptors.

Callers are responsible for supplying required values and compatible children. Building and scoped child access do not scan the node graph or check chunk game compatibility. Game interfaces describe available members. Required scoped getters still throw `MemberNullException` for an absent value, and reference resolution keeps its normal error behavior.

## Customizing generated builders

Add convenience methods or optional customization hooks in partials inside the `GBX.NET.Builders` project. For example:

```cs
namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public partial class TMF
    {
        public TMF KeepsPlaying() => WithIsKeepPlaying(true);

        partial void CustomizeNode(CGameCtnMediaTrack node)
        {
            // Apply domain-specific values after pending assignments.
        }
    }
}
```

The existing track builder already supplies `KeepsPlaying()` before game selection. This example illustrates how to add a convenience to a selected stage.

`CustomizeNode` runs after pending assignments. It is responsible for preserving valid chunks and versions when changing the recipe. A failed build leaves the pending state available for another build. Hooks should avoid mutating shared child nodes if those changes must not survive a failure.

The outer builder and game stages share a generated generic base for common setters. A handwritten method replacing an inherited common setter can call `base.WithName(value)` to retain the generated assignment behavior.

Stages also share bases for overlapping sets of game-specific setters. Assignment descriptors are shared by the property's declaring node, including its derived builders. Matching chunk recipes share creation methods, while game-specific defaults and partial hooks still receive their selected context. Descriptors initialize on demand so an unused setter can be trimmed.

Game interfaces reuse inherited members and shared member contracts. Generic contracts carry each game's child interface types. Collection adapters share projection code, follow collection replacements, and resolve references only on access. On .NET 8 and later, the interfaces implement these projections as default interface members, so a generic member group carries one body for all of its games. This keeps the main assembly much smaller. The .NET Standard 2.0 build implements them explicitly on the nodes instead. The game interfaces are meant to be implemented by GBX.NET nodes only. Use the named node interfaces, game interfaces, and concrete builder stages in application signatures. Generated member-group type names can change with the metadata.

If a member stops being common, an obsolete method in the outer builder partial can call `Set<Property>(state, value)` to preserve pending assignments. The generator supplies that helper when a handwritten partial references it, while at least one game supports the member. The same rule applies to `Set<Property>Reference` helpers. Ordinary generated setters call the shared assignment descriptors directly. Building for an unsupported game reports the incompatible member.

## Exceptional creation recipes

Most builders use the game's chunk writer qualifiers. Chunks marked `ignore` are left out, they are not meant to be used in builders. When that selection needs an explicit override, add `NodeBuilderRecipe` to the node partial. The supplied types replace the inferred chunk selection for that game, including inherited and header chunks:

```cs
using GBX.NET.Attributes;

namespace GBX.NET.Engines.Game;

[NodeBuilderRecipe(GameVersion.TMF,
    typeof(CGameCtnMediaClip.Chunk03079004),
    typeof(CGameCtnMediaClip.Chunk03079005),
    typeof(CGameCtnMediaClip.Chunk03079007))]
public partial class CGameCtnMediaClip;
```

This example records the clip's existing TMF selection. Use an override only when the default recipe needs correction. Invalid contexts, duplicate declarations, unknown chunk types, and chunks not documented for the selected game are diagnosed. An override does not add writer support to a chunk that cannot serialize its payload.

Unqualified chunks do not establish support for every game. Every game with chunk writer qualifiers gets a builder stage, even when a recipe chunk has no writer implementation. Building such a node succeeds, and writing it throws `NotImplementedException` for that chunk. A node marked with `WriteNotSupported`, and any node derived from it, gets no builder.

| Diagnostic | Meaning |
| --- | --- |
| `GBXNETGEN300` | Invalid game version API declaration, recipe metadata, or builder customization. |
| `GBXNETGEN302` | A generic or version-scoped method needs an explicit game interface contract. |

Native editor modes, domain requirements such as minimum key counts, and arbitrary handwritten setter semantics cannot all be inferred from chunk qualifiers. Keep reusable serialization facts in layouts, and use recipe overrides or partials for the exceptional behavior.

See the [design and v1.2 migration inventory](Designs/GeneratedNodeBuilders.md) for compatibility decisions.
