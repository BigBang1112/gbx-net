# GetChunkInfo in ManiaPlanet

`GetChunkInfo(chunkId)` returns a 32-bit mask that tells the engine how to read and write a Gbx body chunk. It describes read and write support, release and editor exclusions, and whether the payload uses skippable framing. The chunk's serializer handles the payload separately.

## Input and result

The virtual method takes a full chunk ID, including its class ID. A simplified declaration is:

```cpp
uint32_t GetChunkInfo(uint32_t chunkId) const;
```

The native declaration uses `unsigned long`, which is 32 bits on Windows even in this x64 executable. The virtual call passes the object in `RCX`, the chunk ID in `EDX`, and returns the mask in `EAX`.

Implementations generally switch over their own chunk IDs and delegate unrecognized IDs to their parent class. The result does not contain a payload size or serialized version. Those values, when present, are handled while reading the chunk.

## Flags

`CMwNod::Archive` at `0x1403686D0` consumes the following bits:

| Bit | Meaning | Reading | Writing |
| --- | --- | --- | --- |
| `0x01` | `EMwChunk_Read` | Allows payload deserialization. | Does not determine whether the chunk is written. |
| `0x02` | Write support | Does not determine whether the chunk is read. | Includes the chunk when the archive mode allows it. |
| `0x04` | `EMwChunk_NotInRelease` | Asserts if a readable chunk appears in a release archive. | Omits the chunk from release archives. |
| `0x08` | `EMwChunk_NotInEditor` | Asserts if a readable chunk appears in an editor archive. | Omits the chunk from editor archives. |
| `0x10` | Skippable framing | Reads a marker and size before the payload, or skips the payload when read support is absent. | Buffers the payload, then writes its marker, size, and bytes. |

Common combinations are:

| Result | Meaning |
| --- | --- |
| `1` (`0x01`) | Readable, omitted when saving. This allows older chunks to remain supported. |
| `3` (`0x03`) | Read and write. |
| `7` (`0x07`) | Read and write, excluded from release archives. |
| `0x13` | Read and write with skippable framing. |

These are the bits observed in the body archive routine, rather than a recovered declaration of the complete native enum.

For the corresponding ChunkL declarations and game-version qualifiers, see [Game-version qualifiers](ChunkL.md#game-version-qualifiers).

## Lookup paths

`Archive` obtains the current node's `CMwClassInfo` and checks its chunk metadata pointer at descriptor offset `+0xD8`.

If that pointer is null, it calls the node's virtual `GetChunkInfo` at vtable offset `+0x90`. Derived implementations can pass inherited chunk IDs to their parent implementation.

If chunk metadata is available, it calls `ClassInfoGetChunkInfo` at `0x140369280`. That function reads `ClassInfo->m_ChunkInfos->GetInfoCallback` at metadata offset `+0x08`, asserts that the callback exists, and forwards the chunk ID to it. It returns the callback's result without interpreting the flags.

For writing, chunk enumeration is separate from the lookup:

| Operation | Virtual path | Metadata path |
| --- | --- | --- |
| Get chunk count | Vtable `+0xA0` | First 32-bit value in the chunk metadata. |
| Convert index to chunk ID | Vtable `+0x98` | `ClassInfo_GetUidChunkFromIndex` at `0x1403692D0`. |
| Get handling flags | Vtable `+0x90` | `ClassInfoGetChunkInfo` at `0x140369280`. |
| Serialize payload | Vtable `+0x78` (`Chunk`) | Same virtual `Chunk` call. |

`ClassInfo_GetUidChunkFromIndex` uses parent chunk counts to find the class that owns the index, then combines the local index with that class's ID.
