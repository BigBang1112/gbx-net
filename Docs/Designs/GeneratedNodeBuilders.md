# Design: Generated node builders and game version interfaces

Generate fluent node builders and game version interfaces from the same model of a node's members, serialization, and supported games. Adding or extending a normal `.chunkl` layout should update both APIs without a handwritten builder, interface, registration, or factory method.

**Status: implemented for layouts with sufficient game metadata.** See [Generated node builders](../NodeBuilders.md) for usage, exceptional recipes, and current metadata limits. Optional partial declarations provide conveniences and handle behavior that cannot be derived from the layout.

## Existing APIs and design direction

The reference is the [`v1.2` branch](https://github.com/BigBang1112/gbx-net/tree/v1.2), inspected at commit `2e32bc47557731c56eff27c59c92b5d965558a3a`. Its builders establish these conventions:

| Convention | Reference |
| --- | --- |
| `CGameCtnMediaClip.Create()` starts a builder. | [Node factory](https://github.com/BigBang1112/gbx-net/blob/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Engines/Game/CGameCtnMediaClip.cs) |
| `WithName(...)` and collection overloads configure common values before choosing a game. | [Clip builder](https://github.com/BigBang1112/gbx-net/blob/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Builders/Engines/Game/CGameCtnMediaClipBuilder.cs) |
| `ForTMUF()`, `ForTM2()`, and similar methods select nested game builders. | [Clip builder](https://github.com/BigBang1112/gbx-net/blob/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Builders/Engines/Game/CGameCtnMediaClipBuilder.cs) |
| A game builder adds game-specific properties, defaults, and chunks. | [TMUF implementation](https://github.com/BigBang1112/gbx-net/blob/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Builders/Engines/Game/CGameCtnMediaClipBuilder.TMUF.cs) |
| Semantic helpers such as `KeepsPlaying()` complement ordinary setters. | [Track builder](https://github.com/BigBang1112/gbx-net/blob/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Builders/Engines/Game/CGameCtnMediaTrackBuilder.cs) |

Keep the recognizable `Create().With...().For...().Build()` workflow. Generate the repeated construction code and use the current `GameVersion` names. In particular, use `TMF` rather than the historical builder name `TMUF`. Provide `ForTM2()` and `ForSM()` as aliases for `ForMP4()`, as specified below.

The [README's game version interfaces](../../README.md#game-version-interfaces) describe three connected features: chunk game specification, scoped interfaces, and builders. This design completes that relationship. A selected game determines the builder surface, construction recipe, and returned interface together.

`Build()` returns the concrete node, following v1.2's convention. `BuildInterface()` returns the selected game's scoped interface. Normalizing game names and replacing existing experimental interface definitions require migration notes.

## Proposed usage

The following examples illustrate the generated API. Available game selectors depend on the node's verified layout and creation recipe.

Create a clip and a track for TrackMania Forever:

```cs
using GBX.NET.Engines.Game;
using GBX.NET.Interfaces.Game;

IGameCtnMediaTrackTMF track = CGameCtnMediaTrack.Create()
    .WithName("Camera")
    .ForTMF()
    .WithIsKeepPlaying(true)
    .BuildInterface();

IGameCtnMediaClipTMF clip = CGameCtnMediaClip.Create()
    .WithName("Intro")
    .ForTMF()
    .WithTracks(track)
    .WithLocalPlayerClipEntIndex(-1)
    .BuildInterface();

clip.Save("Intro.Clip.Gbx");
```

Common setters also remain available after game selection. A builder for TMF does not expose `WithStopWhenRespawn`, which belongs to later clip formats. The interface returned from `BuildInterface()` hides that property too.

Use `Build()` to return the concrete node:

```cs
CGameCtnMediaClip clip = CGameCtnMediaClip.Create()
    .ForTM2020()
    .WithName("Intro")
    .WithStopWhenRespawn(true)
    .Build();
```

`Build()` and `BuildInterface()` run the same construction code. Only their declared return types differ. Both methods require a selected game. There is no implicit default game.

### Builder shape

Emit builders in `GBX.NET.Builders.Engines.<Engine>` with the name `<Node>Builder`. Emit each game stage as a nested `public partial class`, such as `CGameCtnMediaClipBuilder.TMF`. The outer builder is a generated partial declaration as well.

The builders and their runtime live in the `GBX.NET.Builders` package, while the nodes and game version interfaces stay in `GBX.NET`. The builder package cannot add members to node classes from another assembly, so `Create()` is a generated C# 14 static extension member. Each node gets its own `<Node>BuilderExtensions` class in the node's namespace, because extension members with the same signature cannot share a container. A handwritten parameterless static `Create()` on the node suppresses the extension.

The builder package runs the same generator with `GbxGeneratorTarget` set to `Builders`. It receives the layouts and the handwritten `GBX.NET` sources as additional files and adds those sources to its analysis compilation. The generation model therefore sees the same node, chunk, and interface declarations as the main library, which keeps builder availability consistent with the game interfaces.

The outer builder exposes the intersection of writable members available in every emitted game stage. Each game stage exposes all members for that game, including the common members, and every fluent method returns its exact stage type. A game stage does not inherit the outer builder's game selectors. This avoids inherited methods that lose the selected type or let a caller switch games accidentally.

The implementation shares common setters through a generated `<Node>BuilderBase<TSelf>`. The outer builder and each game stage use their own type as `TSelf`, preserving fluent return types while avoiding repeated assignment code for every game.

For each eligible property, generate `With<Property>(value)` using the property's actual type, including nullable value types. Omit builder properties and their accessors to reduce assembly size. Configure builders through fluent methods, and access node or game interface properties after building. Generate `WithIsKeepPlaying(bool)` mechanically. Names such as `KeepsPlaying()` and grouped convenience methods belong in optional partials, since English inflection and domain groupings are not layout rules.

Do not require an empty handwritten builder class or an attribute opting each node into generation. Abstract node classes contribute inherited members and interfaces but have no instantiating factory. Generate factories for concrete nodes when a supported creation recipe can be established.

### Compatibility when the common intersection changes

Adding support for another game can shrink the common member intersection. Accept that the generated common API changes to reflect the supported games. The generator does not retain obsolete members automatically or maintain a history of earlier intersections.

When an existing common method would disappear, maintainers are encouraged to preserve its signature and return type in a handwritten partial, marked with `[Obsolete("Select a supported game with For...() before calling this method.")]`. Use a warning rather than an error so existing callers have a migration path. Name the applicable game selectors in the actual message where practical. Preserve removed properties or common interface members similarly when needed for compatibility, including their forwarding implementations.

A compatibility member should preserve the previous behavior for games that support it, including pending assignment tracking. If the caller selects a game that does not support the assigned value, reject the build with a clear message instead of silently dropping it. Keeping an obsolete member in a partial does not make that member available in newly generated game stages or count as evidence of game support.

The generator emits a `Set<Property>(state, value)` helper when a handwritten builder partial references it and at least one game supports the member. A handwritten obsolete common setter can use that helper after its ordinary common setter disappears, preserving assignment order and the supported-game mask. Generate descriptor-preserving `Set<Property>Reference` helpers under the same rule. Ordinary generated setters call the shared assignment descriptors directly, avoiding forwarding methods that no partial needs.

### v1.2 methods requiring migration

The following audit compares all public methods in the [v1.2 builders directory](https://github.com/BigBang1112/gbx-net/tree/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Builders) with the current layouts and this proposal. These are planned compatibility declarations, not methods already marked obsolete in the library. Constructors and internal `NewNode()` helpers are outside the method inventory.

No existing common setter is currently confirmed to disappear solely because the intersection shrinks. Across the documented writer formats, the old common fields remain common: effect keys and centering, clip names and tracks, track names and blocks, sound keys and playback parameters, text and color, ghost offsets, entity offsets and notice records, camera time ranges, and triangle indices. The handwritten triangle payload still needs its recipe established. Adding more verified legacy formats can change this conclusion later.

The concrete obsolete bridges needed by the current naming and type rules are:

| Existing method | Existing owners | Replacement and compatibility behavior |
| --- | --- | --- |
| `ForTMUF()` | `CControlEffectSimiBuilder`, `CGameCtnMediaBlockCameraGameBuilder`, `CGameCtnMediaBlockGhostBuilder`, `CGameCtnMediaBlockSoundBuilder`, `CGameCtnMediaBlockTextBuilder`, `CGameCtnMediaBlockTriangles2DBuilder`, `CGameCtnMediaBlockTriangles3DBuilder`, `CGameCtnMediaClipBuilder`, `CGameCtnMediaTrackBuilder` | Retain a handwritten obsolete selector directing callers to `ForTMF()`. Delegate to the verified TMF recipe, including any required handwritten triangle recipe. |
| `WithColorBlendMode(int colorBlendMode)` | `CControlEffectSimiBuilder.TMU`, `.TMUF`, `.TM2`, and `.TM2020` | Use `WithColorBlendMode(CControlEffectSimi.EColorBlendMode)`. Where the corresponding stage is supported, retain an obsolete integer overload that converts to the enum and delegates to the generated setter. The current layout establishes TMF, MP4, and TM2020 support, but does not establish a TMU recipe. |

The [old effect game stages](https://github.com/BigBang1112/gbx-net/blob/2e32bc47557731c56eff27c59c92b5d965558a3a/Src/GBX.NET/Builders/Engines/Control/CControlEffectSimiBuilder.TMUF.cs) expose `ColorBlendMode` as `int`. The [current layout](../../Src/GBX.NET/Engines/Control/CControlEffectSimi.chunkl) exposes `EColorBlendMode`, so this is a type migration rather than a loss of game support.

An obsolete selector returning the canonical `TMF` stage preserves ordinary fluent call sites, but does not preserve an explicitly named historical `<Node>Builder.TMUF` type. Likewise, the integer overload belongs on the canonical `TMF` or `MP4` stage after migration. Full compatibility with named old stage types would require handwritten adapter types as separate work. Do not claim binary or complete source compatibility from selector forwarding alone.

The legacy `ForTMUF()` declarations are handwritten obsolete exceptions to the generated selector naming rules. Do not add them to the generator's alias table. `ForTM2()` remains a supported generated alias for `ForMP4()` and is **not obsolete**.

### v1.2 conveniences that can remain in partials

The following methods are not generated mechanically, but do not need to become obsolete merely because they move into partials. Keep them as optional conveniences that delegate to the listed setters. This preserves the existing design's use of helpers such as `KeepsPlaying()`.

| Existing owner | Existing methods | Generated operations used by the partial |
| --- | --- | --- |
| `CControlEffectSimiBuilder` | `Centered()` | `WithCentered(true)` |
| `CControlEffectSimiBuilder.TMU`, `.TMUF`, `.TM2`, `.TM2020` | `ContinousEffect()` | `WithIsContinousEffect(true)` on supported canonical stages. Preserve the property's current spelling. |
| `CControlEffectSimiBuilder.TMUF`, `.TM2`, `.TM2020` | `Interpolated()` | `WithIsInterpolated(true)` |
| `CGameCtnMediaBlockCameraGameBuilder` and `CGameCtnMediaBlockGhostBuilder.TMUF` | `StartingAt(TimeSingle)`, `EndingAt(TimeSingle)`, `WithTimeRange(TimeSingle, TimeSingle)` | `WithStart(...)`, `WithEnd(...)`, or both in order. Ghost start and end are serialized by the TMF format, while modern ghost formats use keys. |
| `CGameCtnMediaBlockSoundBuilder` | `Loops()` | `WithIsLooping(true)` |
| `CGameCtnMediaBlockSoundBuilder.TMUF`, `.TM2`, `.TM2020` | `WithMusic(bool)` | `WithIsMusic(...)` |
| `CGameCtnMediaBlockSoundBuilder.TM2`, `.TM2020` | `StopsWithClip(bool)` | `WithStopWithClip(...)` |
| `CGameCtnMediaClipBuilder.TM2`, `.TM2020` | `StopsWhenRespawn()`, `StopsWhenLeave()` | `WithStopWhenRespawn(true)`, `WithStopWhenLeave(true)` |
| `CGameCtnMediaTrackBuilder` | `KeepsPlaying()`, `ReadOnly()` | `WithIsKeepPlaying(true)`, `WithIsReadOnly(true)` |
| `CGameCtnMediaTrackBuilder.TM2`, `.TM2020` | `Cycles()` | `WithIsCycling(true)` |

Historical stage names in this inventory identify where methods were declared in v1.2. Implement these conveniences on their supported canonical stages, using `TMF` for `TMUF` and `MP4` for `TM2`. Do not recreate a TMU stage solely because v1.2 declared one. Current game qualifiers for effect, sound, and text do not establish their TMU recipes, so those selectors need verified metadata before restoration. The current clip and track layouts do establish TMU writer support.

### v1.2 migrations requiring more than an obsolete wrapper

| Existing method | Current representation and required action |
| --- | --- |
| `CGameCtnMediaBlockSoundBuilder.WithSound(FileRef)` | The current `Sound` property uses `PackDesc`. Generate `WithSound(PackDesc?)`. `FileRef` no longer exists in the current source, so an obsolete overload cannot compile unless a separate legacy type and conversion are restored. Document the type migration instead of promising a trivial bridge. |
| `CGameCtnMediaBlockCameraGameBuilder.TMUF.WithGameCam(EGameCam)` | The historical `EGameCam` enum contains `Behind`, `Close`, `Internal`, and `Orbital`. Those values now belong to `EGameCamOld`, while the TMF recipe serializes `GameCamId`. Migration needs an explicit semantic mapping to camera IDs. Casting the old numeric values to today's `EGameCam` would change their meaning. |
| `CGameCtnMediaBlockCameraGameBuilder.TM2020.WithGameCam(EGameCam2)` | The old `EGameCam2` values correspond to today's `EGameCam`, and the old enum type is absent. Update callers to `WithGameCam(EGameCam)`. A legacy overload would also require restoring the old enum type. |
| `CGameCtnMediaBlockTriangles2DBuilder.TMUF.WithKeys(Func<CGameCtnMediaBlockTriangles2D, IList<CGameCtnMediaBlockTriangles.Key>>)` and the corresponding `Triangles3D` overload | v1.2 invokes the callback immediately with an already constructed node. Pending-state builders have no node at setter time. Keep callback-based configuration only through an explicitly designed partial, or migrate to value-based `WithKeys(...)` and node customization. Do not silently defer the callback and claim identical behavior. |

Other existing `With...` methods whose names and value types still match the model remain ordinary generated setters or compatible collection overloads. For example, `WithAudioToSpeech(bool)`, `WithAudioToSpeechTarget(int)`, and `WithLocalPlayerClipEntIndex(int)` do not become obsolete. Keep existing collection overloads where possible when adding scoped overloads, rather than deprecating them merely because a more specific overload is available. `Build()` continues to return the concrete node and is not obsolete.

### Game selectors: `For[GameVersion]()`

Generate a parameterless `For[GameVersion]()` selector for each game with a supported creation recipe on that node. The suffix matches the enum member exactly. Each selector chooses one exact game context and returns that game's nested builder stage.

| Selector | Selected context | Returned stage |
| --- | --- | --- |
| `ForTM10()` | `GameVersion.TM10` | `<Node>Builder.TM10` |
| `ForTMPU()` | `GameVersion.TMPU` | `<Node>Builder.TMPU` |
| `ForTMS()` | `GameVersion.TMS` | `<Node>Builder.TMS` |
| `ForTMO()` | `GameVersion.TMO` | `<Node>Builder.TMO` |
| `ForTMSX()` | `GameVersion.TMSX` | `<Node>Builder.TMSX` |
| `ForTMNESWC()` | `GameVersion.TMNESWC` | `<Node>Builder.TMNESWC` |
| `ForTMU()` | `GameVersion.TMU` | `<Node>Builder.TMU` |
| `ForVSK5()` | `GameVersion.VSK5` | `<Node>Builder.VSK5` |
| `ForTMF()` | `GameVersion.TMF` | `<Node>Builder.TMF` |
| `ForMP1()` | `GameVersion.MP1` | `<Node>Builder.MP1` |
| `ForMP2()` | `GameVersion.MP2` | `<Node>Builder.MP2` |
| `ForMP3()` | `GameVersion.MP3` | `<Node>Builder.MP3` |
| `ForTMT()` | `GameVersion.TMT` | `<Node>Builder.TMT` |
| `ForMP4()` | `GameVersion.MP4` | `<Node>Builder.MP4` |
| `ForTM2020()` | `GameVersion.TM2020` | `<Node>Builder.TM2020` |
| `ForTM2()` | `GameVersion.MP4` | `<Node>Builder.MP4` |
| `ForSM()` | `GameVersion.MP4` | `<Node>Builder.MP4` |

`ForTM2()` and `ForSM()` are the two naming aliases. Generate them centrally whenever `ForMP4()` is available, with direct delegation:

```cs
public MP4 ForTM2() => ForMP4();
public MP4 ForSM() => ForMP4();
```

All three selectors have identical pending-state snapshot behavior, defaults, chunks, versions, and return types. `Build()` returns the concrete node, while `BuildInterface()` returns the MP4 interface, such as `IGameCtnMediaClipMP4`. Do not generate separate `TM2` or `SM` stages, interfaces, or recipe copies for these aliases.

The aliases always select MP4. They do not choose the latest available ManiaPlanet recipe, fall back to MP3, or select a title-specific format. Callers targeting earlier ManiaPlanet formats use `ForMP1()`, `ForMP2()`, or `ForMP3()` explicitly. A shared MP4 format does not by itself guarantee that every node or value is usable in both Trackmania 2 and Shootmania.

Do not emit `ForUnspecified()` or selectors for combined flags. If a node has no supported MP4 recipe, omit `ForMP4()` and both aliases together. All other generated selector names follow `GameVersion` without additional aliases. Handwritten obsolete selectors for historical compatibility follow the migration inventory above. Adding a supported game recipe generates its selector automatically, and the two MP4 aliases require no per-node handwritten declarations.

### Builder state and reuse

Builders hold pending values, not a partially constructed node. Every slot records whether the caller assigned it, separately from its value. This distinguishes an omitted value from an explicit `null`, `false`, zero, or empty collection.

`ForTMF()` snapshots the common pending state into a new game stage. Later edits to the outer builder do not change that stage. Each build creates a fresh root, chunks, mutable default values, and collection containers. It does not consume the builder or reuse the previous root.

Collection setters snapshot containers when called. Child node instances are shared unless cloning is explicitly requested. Mutating a supplied child can therefore affect multiple results, while adding a track to one built clip cannot change another clip's track container. Builders are not thread-safe.

Apply pending assignments in caller order, including repeated setters. This matters for related setters such as `Flags` and `IsGround`. The last assignment wins through the normal node setter. Preserve this ordering when snapshotting into a game stage.

## One model for interfaces and construction

Extend the existing [generation plan](../../Generators/GBX.NET.Generators/Generation/GenerationPlan.cs) with a shared game model. Interface emission and builder emission must not independently infer whether a member belongs to a game.

| Model | Information |
| --- | --- |
| Node member | Public type, accessibility, inherited owner, setter behavior, reference kind, and source location. |
| Member availability | Games, applicable chunk version ranges, conditional presence, and required or optional value contract. |
| Game creation recipe | Exact game, node initialization, header and body chunks, chunk versions, dependencies, and conditional inclusion rules. |
| Scoped projection | Interface name, child interface types, method signatures, and validation needed when forwarding. |

Derive these models from existing information before introducing more metadata:

- `.chunkl` class inheritance, fields, properties, archives, defaults, game qualifiers, and version conditions.
- [Property-to-chunk ranges](../../Generators/GBX.NET.Generators/Generation/AppliedWithChunkRanges.cs), including base chunk payloads and computed properties.
- Handwritten `AppliedWithChunk` annotations, public signatures, and existing game-specific interface contracts.
- Existing `MethodGameVersion` annotations for methods. Their version information must be interpreted in the owning serialization context, not assumed to be a chunk version globally.
- [Game-specific defaults](../ChunkL.md#game-specific-defaults) and the constructors already produced by `GameDefaultsWriter`.

Do not scan generated C# or use reflection at runtime to recover this information. Generated attributes are outputs of the model, not a second input database.

### Availability rules

For a chunk-backed property, a game is eligible when a writable chunk for that game contains the property at the recipe's selected chunk version. Union valid occurrences of the same property, including inherited serialization. A declaration under `v2+` must not appear in a game stage whose selected payload version is 1.

Conditional serialization needs two separate decisions. A property may be supported by a game while its value is absent for a particular node. Preserve that distinction for flags, nullable node references, and mode-dependent payloads. Version range analysis alone does not prove required presence.

Unqualified chunks mean that writer support is unknown. They do not mean support for every game. Demonstration-only layouts describe serialization without necessarily providing a constructible chunk. Ignored, read-only, or opaque chunks cannot become generated creation steps just because their fields are known.

Self-archives and parent-supplied versions need their actual context. For example, `CGameCtnBlock` has an archive whose version is supplied by its owner. Its game profile must derive that version from the owning format or an existing explicit contract. The generator must not invent a block chunk or infer that archive version from the game's enum value.

For handwritten computed properties, follow explicit chunk mappings or a supported dependency expression. Do not attempt arbitrary C# dataflow analysis. Likewise, do not expose every public method to every game. Reuse existing scoped declarations during migration and explicit availability annotations where method behavior is not derivable.

When evidence is incomplete, produce a diagnostic and omit the unsupported member or game factory. Keep unrelated nodes generating. A coverage report should identify missing evidence by node and game so incomplete metadata is visible without creating hundreds of handwritten builders.

## Generated game version interfaces

Preserve `GBX.NET.Interfaces.<Engine>` and the existing naming convention, such as `IGameCtnChallengeTMF`. Generate partial interfaces and make the node implement them directly. Keep `IClassVersion<TNode>` as the common contract so scoped results retain `Save`, `ToGbx`, and game compatibility inspection.

Generate a common interface from the intersection of members supported by its emitted game interfaces. Do not infer interface inheritance from release order. One later game can remove a member or change its type. Reuse a game interface as a base only when all inherited contracts remain valid.

Mirror node inheritance separately from game release order. A derived common interface inherits its base node's common interface, and its game interfaces inherit the corresponding base game interfaces where available. Redeclare shared `IClassVersion<T>` operations on the derived common view so that retaining both the derived and base generic contracts does not make `Save()` or `ToGbx()` ambiguous.

When this intersection shrinks, follow the [compatibility guidance](#compatibility-when-the-common-intersection-changes) for handwritten obsolete members. The generated intersection remains derived from current game support.

Parsing and construction should expose the same scoped contract:

```cs
var parsed = Gbx.ParseNode<CGameCtnChallenge, IGameCtnChallengeTMF>(path);
var created = CGameCtnChallenge.Create().ForTMF().BuildInterface();

foreach (var block in created.GetBlocks())
{
    Console.WriteLine(block.Name);
    // block.Color is unavailable through IGameCtnBlockTMF.
}
```

Generate explicit forwarding implementations when the scoped signature differs from the raw node signature. For a required nullable-backed member, follow the existing convention:

```cs
string IGameCtnChallenge.MapUid
{
    get => MapUid ?? throw new MemberNullException(nameof(MapUid));
    set => MapUid = value;
}
```

Do not remove `?` simply because a member belongs to a game. Keep optional values nullable, including conditional values such as a free block's position. A non-null interface contract requires evidence of required presence from the layout, an existing scoped contract, or exceptional metadata. Required getters throw `MemberNullException` when their backing value is absent. Resolution and parse errors keep their original exception behavior.

### Scoped child nodes and collections

Project node-valued members and supported method results into interfaces for the same game. `GetBlocks()` on a TMF map returns `IEnumerable<IGameCtnBlockTMF>`. A scoped clip exposes tracks through `IReadOnlyList<IGameCtnMediaTrackTMF>`, rather than exposing its raw `NodeRefList<CGameCtnMediaTrack>`.

Generate live collection adapters and forwarding overloads. A read-only collection view prevents replacement through that view, while its elements remain mutable through their scoped interfaces. Whole-collection setters can accept scoped values and forward through generated conversion code. Existing raw mutable collection exposure on experimental interfaces needs a documented compatibility change.

Never cast `NodeRefList<TNode>` to a collection of interfaces. Mutable generic collections are invariant. Generated adapters must project elements individually and retain the owning reference handles. Optional element nullability follows the element contract.

When a builder accepts a scoped child, generated conversion code checks that it is backed by a supported GBX.NET node implementation. All game interfaces may be implemented by the same runtime class, so a successful interface cast alone is not proof of game compatibility. Callers are responsible for supplying compatible children. Unsupported user implementations produce a clear argument exception.

Scoped child access projects the underlying nodes without walking their graphs or checking chunk game masks. Keep existing parsing and reference resolution behavior.

## Constructing a node for a game

A game qualifier means that the game writes that chunk. It does not by itself establish that every such chunk is mandatory in every node. The [qualifier documentation](../ChunkL.md#game-version-qualifiers) also distinguishes writer support from reader support and archive mode restrictions.

For a node with an unambiguous ordinary chunk layout, derive the default recipe from its writable chunks for the selected game, including base node chunks. Include all unconditional recipe chunks, even when the caller did not set any corresponding property. Creating only chunks touched by setters would produce incomplete nodes.

For example, the current `CGameCtnMediaClip` layout gives this candidate selection before any exceptional conditions:

| Game | Body chunks | Version selection |
| --- | --- | --- |
| TMSX | `0x003` | Unversioned. |
| TMU | `0x003`, `0x004` | Unversioned. |
| TMF | `0x004`, `0x005`, `0x007` | Unversioned. `0x005` reuses the payload of `0x003` without requiring a separate `0x003` instance. |
| MP4 | `0x00D` | Version 0. |
| TM2020 | `0x00D`, `0x00E` | Version 1 for both chunks. |

The TMF selection matches the historical builder. TM2020 also includes the currently documented `0x00E` chunk, rather than freezing the old branch's chunk list. These selections are serializer recipes, not claims that a game has been tested to load every possible result.

Derive conditions and dependencies when the layout establishes them. If alternative chunks, editor modes, handwritten writers, or required opaque data prevent a unique recipe, require exceptional recipe metadata or a partial customization. Report the ambiguity instead of selecting the lowest chunk ID, emitting every alternative, or choosing a chunk because its version is highest.

Prefer putting reusable facts in the layout or shared metadata model. The implementation supplies the C# `NodeBuilderRecipe(GameVersion, params Type[])` attribute for exceptional chunk selections. It replaces the inferred recipe for one exact game without changing ChunkL syntax. Partial hooks cover domain customization after ordinary assignments.

### Build sequence

Each call to `Build()` or `BuildInterface()` performs these steps:

1. Create a fresh node using its game constructor when available. Otherwise use its ordinary constructor, provided the recipe establishes that its defaults are valid for this game.
2. Create the recipe's unconditional chunks with the exact game context and correct initial versions. Use typed construction and the normal node chunk collection. The existing Gbx conversion separates header chunks for header serialization.
3. Apply pending assignments through public setters in caller order. Copy reference handles and collection containers without resolving external files.
4. Run the optional customization hook, then evaluate conditional recipe steps and establish their dependencies. Reuse existing chunks instead of creating duplicates.
5. Return the concrete node from `Build()`, or the node through its scoped interface from `BuildInterface()`.

A failed build leaves the builder reusable. Recipes establish the initial chunks and versions. Customizations are responsible for preserving their validity. Callers supply required values and compatible children. Builders do not recheck recipe chunks, versions, required members, or child graphs.

`CMwNod.GameVersion` is an inferred compatibility mask, not stored build intent. A node built for TMF can remain compatible with several games. `Unspecified` means insufficient or conflicting evidence and cannot prove either compatibility or incompatibility on its own. Builders use the creation recipe to select chunks and defaults and do not gate construction on this mask.

### Defaults and required input

Keep defaults in the existing node and chunk initialization model. Explicit caller values override those defaults. Do not copy default expressions into a separate builder database or evaluate defaults for games that were not selected.

Game selection accepts one exact `GameVersion`. Any runtime selector added later must reject `Unspecified`, unknown bits, and combined flags. Chunk versions come from the selected game's constructor rules, not from the largest version ever observed for the chunk. Signed integer version branches use `value >= 1` where applicable.

Use existing fallback defaults and constructors as they are defined. Do not invent names such as `"Unnamed clip"`, create arbitrary child nodes, or substitute empty values just to satisfy a non-null contract. If a required child or required data has no valid default, the caller is responsible for supplying it. Building leaves omitted values at their existing defaults.

Callers or handwritten customizations enforce domain requirements such as a minimum key count. Required node defaults may be generated from an explicit construction expression, but automatic recursive creation of every non-null child risks cycles and invalid placeholder graphs.

## References and external files

Builders must preserve the [node reference model](../NodeReferences.md). Generate ordinary node setters, descriptor-preserving reference setters, and collection overloads from the member's wire kind.

- `WithTracks(params ...)` and enumerable overloads accept scoped child interfaces on a selected stage. Raw node overloads remain available where overload resolution is unambiguous.
- An overload accepting `NodeRefList<T>` copies its stored handles with `EnumerateReferences()`, rather than enumerating resolved nodes.
- A scalar reference overload copies a `NodeRef<T>` into the destination member's reference handle. Assigning a plain node intentionally selects an embedded reference through the existing node setter.
- Descriptor assignment is available only where the wire operation supports external references. Inline nodes and value archives cannot acquire file descriptors through a convenience overload.

Building copies reference descriptors and handles without resolving external files. Scoped access resolves references only when needed and preserves resolver failures. Callers are responsible for child compatibility, including children supplied through external references.

## Optional partial customizations

Ordinary nodes need no partials. Emit an optional customization hook in the concrete game stage for exceptional defaults and compound values. For example:

```cs
namespace GBX.NET.Builders.Engines.Game;

public partial class CGameCtnMediaTrackBuilder
{
    public partial class TMF
    {
        public TMF KeepsPlaying() => WithIsKeepPlaying(true);

        partial void CustomizeNode(CGameCtnMediaTrack node)
        {
            // Optional domain customization after pending assignments.
        }
    }
}
```

Generate an optional `partial void CustomizeNode(TNode node)` declaration. Exceptional recipe changes and any domain-specific checks belong in this hook. Property normalization hooks may be added where needed, but do not generate a large public extension framework by default.

Custom methods delegate to generated setters so assignment tracking and ordering remain correct. Generate construction even when a partial exists. An arbitrary method named `Build` or `BuildInterface` must not silently replace construction logic.

Partial interfaces can add domain members, with handwritten node implementations where needed. During migration, recognize existing members by full signature, including generic arity, parameter modifiers, and explicit interface ownership. Suppress an equivalent generated declaration, and diagnose an incompatible collision. A same-name method alone is not a customization contract.

## Generator integration

Implement the feature inside `GBX.NET.Generators` so its outputs consume the same parsed layouts as serialization:

1. Extend type inventory to include handwritten builders and interfaces. `ExistingTypeAnalysis` currently filters primarily to engine types and the root namespace, so it does not yet inventory these namespaces.
2. Build a `GameNodeModel` and per-game creation recipes from the existing `GenerationPlan`, field mappings, and handwritten contracts.
3. Use `GameVersionWriter` to emit partial interfaces and explicit node forwarding implementations.
4. Use the same writer for factories, pending state, common and game stages, and typed construction.
5. Generate diagnostics and a deterministic coverage summary from the same model.

`GameNodeModel`, `GameMember`, `GameChunk`, and `GameMethod` hold the shared model. Use semantic symbols for handwritten property types and accessibility checks where syntax matching is insufficient. Generated engine members are absent from the input compilation, so derive their signatures from the normalized layout model. Do not depend on running one source generator before another or reparsing emitted source.

Emit sources with stable names for nodes, builders, interfaces, and adapters. Sort games and chunk identities deterministically, while preserving default expression order and caller assignment order where behavior depends on them. Partition incremental inputs by node and its dependencies where practical. A layout change should invalidate dependent projections and recipes as well as serialization.

Generated output stays under the project's configured compiler-generated directory. Never edit those files directly. Keep implementation compatible with all library targets, including `netstandard2.0`, and preserve trimming and NativeAOT support through direct calls and generated dispatch.

### Sharing generated code

Keep member implementations proportional to distinct contracts and recipes rather than repeating them for every game:

- Common setters share a generic base. Overlapping game-specific member sets share additional bases where at least two stages use them, retaining each stage's fluent return type. Handwritten replacements stay on the affected stage.
- A property's declaring node owns its shared assignment descriptor. Derived builders supply their own availability masks, preserving setter order and snapshot behavior. Initialize descriptors lazily so using one setter does not root all setters during trimming.
- Generated setters call those descriptors directly. Emit `Set<Property>` and `Set<Property>Reference` compatibility bridges only when a handwritten builder partial references them, including method groups and `nameof` expressions.
- Identical ordered chunk recipes and versions share typed creation methods. Pass the selected game to constructors to retain game-specific defaults. Keep the optional customization hook on each concrete stage.
- Derived interfaces inherit equivalent parent members. Game interfaces share member contracts, with generic parameters for scoped child interfaces. Keep existing handwritten interface declarations and their accessor slots.
- Scoped collection views share projection code, while their collection getter follows replacement of the owner's collection.

Generated member-group names are implementation details and may change when metadata changes. Preserve the named node and game interfaces, selectors, stage return types, and `Build()`/`BuildInterface()` behavior. Compare assembly size as well as generated source size when changing this structure.

### Diagnostics

Allocate diagnostic IDs during implementation. At minimum, distinguish:

| Condition | Behavior |
| --- | --- |
| Missing game or parent archive context | Report the missing evidence and omit only the affected projection or factory. |
| Conflicting creation alternatives or defaults | Report an error for that recipe. Do not emit a callable factory that fails inevitably. |
| Required value has no default | Generate an ordinary setter. The caller supplies the value when needed. |
| Unsupported setter or opaque writer | Explain which member or recipe needs metadata or a partial implementation. |
| Invalid partial, signature collision, or inaccessible constructor | Report the conflicting declaration at its source. |

Diagnostics should point to the layout or handwritten declaration that can fix the problem. Metadata gaps must not force users to maintain parallel per-game builder classes.

## Delivery and verification

Start with the MediaTracker clip, track, and text block builders as an end-to-end slice. They exercise v1.2's API, defaults, inherited members, references, and several chunk formats. Include `CGameCtnChallenge` and `CGameCtnBlock` when migrating the existing scoped interfaces, especially parent-controlled archive versions and computed flag properties.

Generate interfaces and builders together from the beginning. Expand coverage by improving shared inference and correcting source metadata. Avoid introducing handwritten stubs as an intermediate requirement for every node. Keep old experimental interfaces only where they remain compatible, and document deliberate signature changes before replacing them.

Verification for implementation should cover:

- Compilation of generated sources and consumer snippets. Unsupported setters and properties must fail to compile through a selected game stage or interface, except for explicitly retained obsolete compatibility members.
- An added game shrinking the common intersection. A handwritten obsolete compatibility member preserves old calls with a warning, supports migration to game-specific setters, and rejects unsupported assignments when building.
- Return types: `Build()` returns the concrete node and `BuildInterface()` returns the selected game's interface. Both methods apply the same construction rules.
- Selector names match `GameVersion`. `ForTM2()` and `ForSM()` return the MP4 stage and behave identically to `ForMP4()`, including scoped return types and snapshots. All three are absent when MP4 construction is unsupported.
- Construction of the expected header and body chunks with the selected versions. Cover inherited chunks, base payload reuse, alternatives, conditional payloads, and self-archives.
- Omitted values versus explicit `null`, `false`, zero, and empty collections. Check that building preserves omitted required values and setter ordering for overlapping flags. Required scoped getters retain their null checks.
- Builder reuse, game-stage snapshots, and independent collection containers. Check that build failures do not consume pending state.
- Scoped child projections, optional values, incompatible children, and descriptor copying that does not resolve files.
- Partial customization, signature collisions, and a new layout generating its factory, builder, and interfaces without handwritten companion files.
- Save and parse round trips against representative fixtures for each supported recipe. Claim in-game validity only after actual game verification.
- Deterministic output, dependency invalidation, all target frameworks, and focused trimming and NativeAOT smoke checks.

Use the repository's [generator verification commands](../../.agents/generator-verification.md) when implementing the feature. This design-only change needs Markdown review, link validation, and `git diff --check`.
