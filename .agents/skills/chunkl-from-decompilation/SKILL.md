---
name: chunkl-from-decompilation
description: Create or fill GBX.NET ChunkL layouts from native decompilation.
---

# ChunkL from decompilation

Work on the requested classes and chunks. Read the repository instructions, existing layouts and handwritten C#, and `Docs/ChunkL.md`.

Check programs in this order using GhidraMCP:

1. `Maniaplanet.exe`
1. `TmForever.exe`
1. `Trackmania.exe`
1. `ManiaPlanetLogs.exe`
1. `TrackmaniaTurboLogs.exe` (ask first)
1. `TmNationsESWC.exe` (ask first)
1. `TmSunrise.exe` (ask first)
1. `TrackManiaPU.exe` (ask first)
1. `TrackMania2003.exe` (ask first)

Check chunk serializers (`Chunk` function implementations) to verify field order, widths, counts, references, defaults and version branches.

Create or fill `Src/GBX.NET/Engines/<Engine>/<Class>.chunkl` using the verified information from the decompilation. Try to fill in **as many member names as possible**.

- Add member names when they are missing in the ChunkL files.
- Update member names when the exact names are known or if better meaning is found.
- Create obsolete members where possible in the current architecture.
- Use unknown field names in archives when meaning is unverified, remove the unknown naming from chunks.
- Set defaults when found in the constructor. Ignore them if it's the default value of the type.
- Use comments to describe what it does, rather than how it is implemented. On chunks, use short informative description, such as "legacy tracks and name". Do not use period for very short descriptions. In case the chunk has just one member, use the member name as the comment.
- Prefer signed types for integer fields unless there is a specific reason to use unsigned, such as flags.

Add `- inherits <BaseClass>` when the class inherits from another class, except for `CMwNod`.

Do not change generator behaviour unless absolutely necessary, but also do not transition to C# code right away if you identify a problem.

Build using `.agents/generator-verification.md`, inspect the diff, and use relevant fixtures when available. Report the binaries checked, changed chunks, verification and unresolved evidence.

## Game versions

Use the verified `GetChunkInfo` write bit (`0x02`) to add the current game's qualifier.

- Read support alone (`0x01`) does not qualify.
- Preserve earlier writer qualifiers.
- Release/editor exclusions (`0x04`/`0x08`) do not exclude the game.
- Use the skippable bit (`0x10`) independently of write support: `0x13` means a skippable writer, while `0x11` means a skippable reader only.
- Add `(skippable)` accordingly.

Enter individual chunk versions if the chunk is versioned, including version 0.

- For `Trackmania.exe`, fill `TM2020`.
- For `ManiaPlanetLogs.exe`, fill `MP4`.
- For `TrackmaniaTurboLogs.exe`, fill `TMT`.
- For `TmForever.exe`, fill `TMF`.
- For `TmNationsESWC.exe`, fill `TMNESWC`.
- For `TmSunrise.exe`, fill `TMSX`.
- For `TrackManiaPU.exe`, fill `TMPU`.
- For `TrackMania2003.exe`, fill `TM10`.

**Do not fill game version qualifiers for other executables unless specified.**