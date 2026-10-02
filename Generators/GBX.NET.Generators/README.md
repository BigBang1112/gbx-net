# GBX.NET.Generators

ChunkL fields normally generate properties or chunk fields. Add `local` to keep a value in the serialization method's current scope. Every `local` field requires a `write` expression that supplies its value when writing:

```chunkl
0x001
  int Count (local, write: "Items::Length")
  Item[Count] Items
```

Later statements in the same scope can use `Count`, including conditions and array lengths. Local fields do not generate members, property attributes, or clone assignments.

Use `write` alone to override the serialized value while retaining the field's normal storage:

```chunkl
  int Count (write: "Items::Length")
```

Reads still populate `Count`. Writes evaluate the expression without changing the stored property. Both flags work with separate `Read`/`Write` methods and combined `ReadWrite` methods. Write expressions use the same identifier and `::` member access rules as other ChunkL expressions, and are evaluated only when writing.
