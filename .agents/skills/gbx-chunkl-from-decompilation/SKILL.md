---
name: gbx-chunkl-from-decompilation
description: Create or fill GBX.NET ChunkL layouts from native decompilation, including writer game versions and skippability, and replace handwritten C# serialization where ChunkL can express it.
---

# ChunkL from decompilation

Work on the requested classes and chunks. Read the repository instructions, existing layouts and handwritten C#, and `Docs/ChunkL.md`.

- Check both `Trackmania.exe` and `Maniaplanet.exe` when available in GhidraMCP. Use explicit program names and verify each game/build. Compare chunk serializers, their helpers and relevant `GetChunkInfo` implementations across both binaries, including inherited dispatch. Do not infer wire layout from object offsets.
- Create or fill `Src/GBX.NET/Engines/<Engine>/<Class>.chunkl` using verified field order, widths, counts, references, defaults and version branches. Update member names when the exact names are known or if better meaning is found, create obsolete members where possible in current architecture. Use unknown field names in archives when meaning is unverified, remove the unknown naming from chunks. Set defaults when found in the constructor.
- Use the verified `GetChunkInfo` write bit (`0x02`) to add the current game's qualifier. Read support alone (`0x01`) does not qualify. Preserve earlier writer qualifiers. Release/editor exclusions (`0x04`/`0x08`) do not exclude the game.
- Use the skippable bit (`0x10`) independently of write support: `0x13` means a skippable writer, while `0x11` means a skippable reader only. Add `(skippable)` accordingly. Investigate conflicting evidence before changing existing framing.
- Add `.vN` only for the highest payload version established for that game by serialization or samples. Never use the flag mask as the version. Use a bare game label when payload version is unknown or absent. Runtime version branches still need `version` or `versionb`. Do not add `.vN` for version 0.
- Remove redundant handwritten C# serializers and members when generated code preserves behavior, defaults and public API. Remove `demonstration` when enabling generated serialization. Keep necessary custom code and prefer combined `ReadWrite`, removing `SeparateReadAndWrite` when practical. Never edit generated files.
- Build using `.agents/generator-verification.md`, inspect the diff, and use relevant fixtures when available. Report the binaries checked, changed chunks, verification and unresolved evidence.

## Maniaplanet.exe game versions

**For `Maniaplanet.exe` (case-insensitive), require the user to explicitly specify whether to fill game-version qualifiers.** A general request to complete ChunkL is insufficient. If unspecified, don't add game-version qualifiers and continue independent layout, skippability and C# work.

