# ChunkL in GBX.NET

GBX.NET uses [ChunkL](https://github.com/BigBang1112/chunkl) files to describe the binary layouts of Gbx classes. The GBX.NET source generator reads those layouts and generates the class members and serialization code for every target framework.

This page covers the repository workflow. For the complete language grammar and semantics, use the [ChunkL language specification](https://github.com/BigBang1112/chunkl/blob/main/SPECIFICATION.md).

## Where layouts belong

Create layouts in `Src/GBX.NET/Engines/<Engine>/<Class>.chunkl`. The file name must match the class name, and the project includes layouts only from the immediate engine directories with `Engines/*/*.chunkl`.

When a class also has handwritten C# code, declare that type as `partial`. The generator adds the generated members to it.

> [!WARNING]
> Do not edit generated files under `Src/GBX.NET/Generated/<TargetFramework>/`!

## Basic layout

Each layout begins with its class name and 32-bit class ID. A chunk uses a three-digit hexadecimal offset, which is combined with the class ID to create its full chunk ID. Field declarations are indented with two spaces.

This illustrative layout defines a versioned, skippable chunk:

```chunkl
CExample 0x03000000

0x002 (skippable) [TM2020.v1] // Example payload.
  version
  int Value
  v1+
    bool IsEnabled
```

The generated class exposes `Value`, `IsEnabled`, and `Version`, then emits read/write code for chunk `0x03000002`. The version qualifier documents the observed game context and highest observed chunk version. It does not establish the runtime version, so the chunk body must read or write `version` or `versionb` before a version block.

Use an unnamed `archive` for a class self-archive, and named `archive` declarations for reusable value layouts. Layouts can also declare enums, flags, properties, constructor defaults, fixed or variable arrays, nullable values, casts, version blocks, and control flow. See the language specification for their syntax and constraints.

Reference a named archive from another class with its dotted type name, such as `CGameCtnMacroBlockInfo.BlockSpawn[] Blocks`. The generator uses the enclosing class layout to resolve the archive and its serialization.

## Nullable signed integers

Use `(nullable)` on a scalar signed integer field to read `-1` as `null` and write `null` as `-1`:

```chunkl
0x001
  int AuthorScore (nullable)
  short OptionalCount (nullable)
```

The generated properties are nullable. The flag supports `sbyte`, `short`, `int`, `long`, and `int128`, including their `int8`, `int16`, `int32`, and `int64` aliases. Other values, including negative values below `-1`, retain their values. It also works with `(time)` on an integer field.

A `?` alone makes the C# type nullable without selecting the integer sentinel conversion. Add `(nullable)` when the wire format uses `-1` for a missing value. The flag does not support unsigned integers, arrays, lists, or casts.

## GBX.NET attributes

ChunkL attributes describe how GBX.NET should generate a chunk:

- `header` generates a header chunk.
- `skippable` generates a size-prefixed skippable chunk.
- `ignore` marks the generated chunk as ignored.
- `demonstration` documents a known layout without generating its serialization implementation.
- `base: 0xHHH` extends a previous chunk. Use `base` in the body where inherited serialization should run.
- `struct: Name` describes the raw name of a struct used for a header chunk.

At the class level, `inherits: ParentClass` creates the C# inheritance relationship and makes inherited chunks available. Named archives can use `inherits: BaseArchive` and `contextual` when their serialization needs the enclosing class node.

Version lists such as `[TM10.v3, TMF.v11, TM2020.v13]` are format research metadata. Keep them accurate when adding or changing a layout, but do not treat them as generated runtime conditions.

## Game-version qualifiers

Use game-version qualifiers to record the games that write a chunk. A game that only reads a legacy chunk should not be added to that chunk's version list. Keep qualifiers for earlier games that wrote it.

Use the write bit (`0x02`) from [GetChunkInfo](ManiaPlanetGetChunkInfo.md#flags) to make this distinction. For a binary verified as ManiaPlanet 4, common results translate as follows:

| `GetChunkInfo` result | ChunkL declaration | Game-version qualifier |
| --- | --- | --- |
| `1` (`0x01`) | Keep the legacy chunk's layout. | Do not add `MP4` merely because it can read the chunk. |
| `3` (`0x03`) | `0x002 [MP4]` | Add `MP4` because the game writes the chunk. |
| `7` (`0x07`) | `0x002 [MP4]` | Add `MP4`. Release exclusion does not exclude the game, since other archive modes can write it. |
| `0x13` | `0x002 (skippable) [MP4]` | Add `MP4` and mark the chunk as skippable. |

Replace the chunk offset and game label with the verified values, and preserve any existing qualifiers. The skippable bit (`0x10`) applies independently of the write bit. For example, `0x11` means a skippable legacy chunk that is readable but not written, so it needs `(skippable)` without adding the current game. Editor exclusion (`0x08`) also restricts the archive mode rather than the game-version list.

If an earlier game writes a chunk and MP4 only reads it, keep the earlier game's qualifier:

```chunkl
0x002 [TMF] // MP4 can still read this legacy chunk.
```

If both games write the chunk, list both:

```chunkl
0x002 [TMF, MP4]
```

Add a `.vN` suffix only when the payload serializer or a Gbx sample establishes the highest observed chunk version for that game. For example, `[MP3.v3, MP4.v5]` records payload versions 3 and 5. **The `GetChunkInfo` mask is not the chunk version**, so a return value of `3` does not imply `[MP3.v3]`. When the payload version is unknown or the chunk is unversioned, use `[MP3]`.

The qualifiers generate `ChunkGameVersion` metadata and the chunk's `GameVersion` flags. They do not control serialization by game or archive mode. Read the payload's version with `version` or `versionb` when needed, and document release or editor exclusions separately. If write support has not been verified, leave the game's qualifier unset rather than inferring it from a readable layout.

## References

- [ChunkL language specification](https://github.com/BigBang1112/chunkl/blob/main/SPECIFICATION.md)
- [GBX.NET project configuration](../Src/GBX.NET/GBX.NET.csproj), which includes engine layouts as generator inputs
- [GBX.NET source generator](../Generators/GBX.NET.Generators/GbxGenerator.cs)
- [Representative layout](../Src/GBX.NET/Engines/Game/CGameCtnBlock.chunkl)
