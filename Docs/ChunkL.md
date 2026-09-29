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

## References

- [ChunkL language specification](https://github.com/BigBang1112/chunkl/blob/main/SPECIFICATION.md)
- [GBX.NET project configuration](../Src/GBX.NET/GBX.NET.csproj), which includes engine layouts as generator inputs
- [GBX.NET source generator](../Generators/GBX.NET.Generators/GbxGenerator.cs)
- [Representative layout](../Src/GBX.NET/Engines/Game/CGameCtnBlock.chunkl)