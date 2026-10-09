# GBX.NET.Generators

ChunkL fields normally generate properties or chunk fields. Add `local` to keep a value in the serialization method's current scope. Every `local` field requires a `write` expression that supplies its value when writing:

```chunkl
0x001
  int Count (local, write: "Items.Length")
  Item[Count] Items
```

Later statements in the same scope can use `Count`, including conditions and array lengths. Generated local variables use camelCase, so `Count` becomes `count` in C#. Local fields do not generate members, property attributes, or clone assignments.

Use `write` alone to override the serialized value while retaining the field's normal storage:

```chunkl
  int Count (write: "Items.Length")
```

Both flags work with separate `Read`/`Write` methods and combined `ReadWrite` methods. Combined methods pass the write expression as the reader/writer's value argument and store its returned value. Separate `Write` methods evaluate the expression without changing the stored property. Reads populate the stored value in both cases.

Write expressions use the same identifier and `.` member access rules as other ChunkL expressions. Combined methods guard the argument so the expression is evaluated only when a writer is present.

Declare game-specific chunk versions with `.vN` qualifiers on the chunk:

```chunkl
0x00B [MP3.v0, TMT.v0, MP4.v1, TM2020.v1]
  version
```

Generated chunk `GameVersion` constructors select `Version` from these qualifiers. A plain `version` or `versionb` default is rejected when the chunk declares explicit `.vN` versions. Constructors leave `Version` unassigned for an unspecified game, a game without a `.vN` qualifier, or a combination of games. Handwritten constructors own their initialization. Node constructors do not create chunks.

Game-specific defaults on `version` and `versionb`, such as `version [MP3 = 0]`, are rejected. Other fields can still declare game-specific defaults.
