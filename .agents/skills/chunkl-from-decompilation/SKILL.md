---
name: chunkl-from-decompilation
description: Create or fill GBX.NET ChunkL layouts from native decompilation.
---

# ChunkL from decompilation

Work on the requested classes and chunks. Read the repository instructions, existing layouts and handwritten C#, and `Docs/ChunkL.md`.

## Check decompilation

Check programs in this order using GhidraMCP:

1. `Maniaplanet.exe`
1. `TmForever.exe`
1. `Trackmania.exe`
1. `ManiaPlanetLogs.exe`

If the earlier programs leave anything unclear, continue in this order:

1. `TrackmaniaTurboLogs.exe`
1. `TmNationsESWC.exe`
1. `TmSunrise.exe`
1. `TrackManiaPU.exe`
1. `TrackMania2003.exe`
1. `Vsk5Online.exe`

Use `Resources/ClassNameMapping.txt` to trace older class names and check their `Chunk`/`GetClassInfo` functions.

Check chunk serializers (`Chunk` function implementations) to verify field order, widths, counts, references, defaults and version branches.

## Update layouts

Create or fill `Src/GBX.NET/Engines/<Engine>/<Class>.chunkl` using verified decompilation findings. Investigate member names deeply and fill in as many as possible.

- Add missing member names and update existing names when the exact name or a better meaning is verified. **Prefer exact name.**
- Create obsolete members where the current architecture allows it. Do not add obsolete to members that are only available in certain games.
- Keep unknown field names in archives when meaning is unverified. Remove unknown naming from chunks.
- Set defaults found in constructors, except when they match the type's default value.
- Prefer signed integer types unless there is a specific reason to use unsigned, such as flags.
- Add `- inherits <BaseClass>` when the class inherits from another class, except for `CMwNod`.

If new types are discovered, create them.

Use comments to describe what the data does, rather than how it is implemented. Keep chunk descriptions short and informative, such as "legacy tracks and name", without a period for very short descriptions. For a chunk with one member, use the member name as the comment.

Do not change generator behavior unless absolutely necessary, but do not immediately switch to handwritten C# when you identify a problem.

Do not implement chunks unused across the supported games whose values are discarded in all games, unless explicitly requested.

## Game versions

Use the verified `GetChunkInfo` write bit (`0x02`) to add the current game's qualifier.

- Read support alone (`0x01`) does not qualify.
- Preserve earlier writer qualifiers.
- Release/editor exclusions (`0x04`/`0x08`) do not exclude the game.
- Use the skippable bit (`0x10`) independently of write support: `0x13` means a skippable writer, while `0x11` means a skippable reader only.
- Add `(skippable)` accordingly.

Enter individual chunk versions if the chunk is versioned, including version 0.

| Executable | Game qualifier |
| --- | --- |
| `Trackmania.exe` | `TM2020` |
| `ManiaPlanetLogs.exe` | `MP4` |
| `TrackmaniaTurboLogs.exe` | `TMT` |
| `TmForever.exe` | `TMF` |
| `TmNationsESWC.exe` | `TMNESWC` |
| `TmSunrise.exe` | `TMSX` |
| `TrackManiaPU.exe` | `TMPU` |
| `TrackMania2003.exe` | `TM10` |
| `Vsk5Online.exe` | `VSK5` |

**Do not fill game version qualifiers from other executables unless specified.**

## Verify and report

Build using `.agents/generator-verification.md`, inspect the diff, and use relevant fixtures when available. Report the binaries checked, changed chunks, verification and unresolved evidence.
