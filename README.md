![GBX.NET](logo_outline.png)

GBX.NET is a GameBox (.Gbx) file parser library written in C# for .NET software framework. This file type can be seen in many of the Nadeo games like TrackMania, ShootMania or Virtual Skipper.

For any questions, open an issue, join [my Discord server](https://discord.gg/perAcdxscQ) or message me via DM: BigBang1112#9489.

[![Nuget](https://img.shields.io/nuget/v/GBX.NET?style=for-the-badge)](https://www.nuget.org/packages/GBX.NET/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/gbx-net?include_prereleases&style=for-the-badge)](https://github.com/BigBang1112/gbx-net/releases)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/BigBang1112/gbx-net?style=for-the-badge)](#)

- GBX.NET can recognize **entire Gbx files**, however **cannot read all of the existing files**. Gbx file is basically a serialized class from the GameBox engine, and all of these classes must be known to read. Exploring new chunks can be done by reverse engineering the games.
- GBX.NET can **write** most of the Gbx file types (those that can be read by the parser).
- All versions of Gbx are supported: ranging from TM1.0 to TM®, except the Gbx versions below 3 (which haven't been seen so far, even in the oldest game).
- **GBX.NET 0.10.0+ is separated into MIT and GPL3.0, see [License](#License)**.
- Reading text-formatted Gbx is not currently supported.
- Reading compressed reference tables is not currently supported.
- Reading Pak files is partially supported with the **GBX.NET.PAK** sublibrary, but it applies only to Paks from TMU/F.

Here are some of the useful classes/types to start with:

| Latest extension | Class | Can read | Can write | Other extension/s
| --- | --- | --- | --- | ---
| Map.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes | Challenge.Gbx
| Replay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | **No<sup>1</sup>**
| Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | **Yes**
| Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| Item.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Block.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Mat.Gbx | [CPlugMaterialUserInst](Src/GBX.NET/Engines/Plug/CPlugMaterialUserInst.cs) | Yes | Yes
| Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | Yes | Yes
| LightMapCache.Gbx | [CHmsLightMapCache](Src/GBX.NET/Engines/Hms/CHmsLightMapCache.cs) | Yes | Yes
| SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes
| Scores.Gbx | [CGamePlayerScore](Src/GBX.NET/Engines/Game/CGamePlayerScore.cs) | Yes | No

- <sup>1</sup>Safety reasons. Consider extracting `CGameCtnGhost` from `CGameCtnReplayRecord`, transfer it over to `CGameCtnMediaBlockGhost`, add it to `CGameCtnMediaClip`, and save it as `.Clip.Gbx`, which you can then import in MediaTracker.
- <sup>2</sup>Only up to TMUF.

**Full list of supported file types is available in the [SUPPORTED GBX FILE TYPES](SUPPORTED_GBX_FILE_TYPES.md)**.

## Compatibility and build

- GBX.NET is compatible down to **.NET Standard 2.0** and **.NET Framework 4.6.2**.
- Current C# language version is **10**.

To build the solution:
- Installing Visual Studio 2022 with default .NET tools and **.NET Framework 4.6.2 Targeting Pack** is the easiest option.
- JetBrains Rider also works as it should. Visual Studio Code may work with a bit more setup.
- Make sure you have all the needed targetting packs installed (currently .NET 6.0, .NET Standard 2.1, .NET Standard 2.0, and .NET Framework 4.6.2).

*(reminder: you can just use the NuGet packages in any IDE or text editor that supports them)*

## Techniques

The Gbx format compatibility is extensible through the `GBX.NET.Engines` namespace.

Through the Gbx format, internal game objects (called **nodes**) are being serialized and deserialized through **chunks**. In GBX.NET, chunks are presented as node's **nested classes** and they are named using the pattern `Chunk[chunkId]` (where `chunkId` is formatted on 8 digits).

Nodes inherit `CMwNod` or other class that inherits `CMwNod` and each node class has a `NodeAttribute` with its **latest ID** and a **protected constructor**. Chunks inherit the `Chunk<T>`/`SkippableChunk<T>`/`HeaderChunk<T>` class based on the behaviour and each chunk class has a `ChunkAttribute` with its **latest ID**.

Node caching is being done to increase the performance of reflection that handles the rules above (applies to both reading and writing):

- All nodes are cached when calling the `NodeCacheManager.GetClassTypeById` (reading) or `NodeCacheManager.GetClassIdByType` (writing) for the first time - using the `NodeCacheManager.CacheClassTypesIfNotCached()` method, causing a slight delay on the first parse (rougly between 7-20 ms) and additional 1MB usage of memory. This may increase very slightly with future library additions.
- *Selective chunk caching* only caches the chunk types and their attributes when a "new" node type appears during reading/writing. This was done to improve the delay of node caching and to value memory usage to things that are needed. Not caching the chunk types together with node types (at the same time) improves the performance by around 80%.

The library also speeds up parse time by ignoring unused skippable chunks with *discover* feature:

- Discover basically means "read a skippable chunk".
- Skippable chunks are read in-depth only if methods or properties request for them.
- Calling certain properties will discover all needed chunks synchronously before returning the value.
- You can pre-discover certain chunks on different threads to increase your code's performance.

## Benchmarks

Maps were selected from all kinds of Trackmania official campaigns picked by the biggest file size.

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1889 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.100-preview.6.22352.1
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT  [AttachedDebugger]
  Job-LTRNAC : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
```

| File name | Read | Read header | Write
| --- | --- | --- | ---
| 0_TrackMania\1.2.5 DemoSolo\DemoRaceB1 | 0,23 ms | 0,04 ms | 0,26 ms
| 4_TrackMania United\2\snowC5 | 0,30 ms | 0,05 ms | 0,34 ms
| 2_TrackMania Original\1.5 Demo\DemoRace3 | 0,33 ms | 0,05 ms | 0,29 ms
| 1_TrackMania Sunrise\1.4.7\CleanLanding | 0,33 ms | 0,04 ms | 0,34 ms
| 0_TrackMania\1.2.3\RaceF7 | 0,39 ms | 0,05 ms | 0,37 ms
| 4_TrackMania United\2.0.8\DesertE | 0,43 ms | 0,06 ms | 0,43 ms
| 1_TrackMania Sunrise\1.4.5\AirControl | 0,50 ms | 0,04 ms | 0,67 ms
| 3_TrackMania Nations ESWC\1.7.5\Pro A-4 | 0,64 ms | 0,05 ms | 1,34 ms
| 1_TrackMania Sunrise\1.4.5 Nvidia\TrialTime | 0,68 ms | 0,04 ms | 1,03 ms
| 1_TrackMania Sunrise\1.5\TrialTime | 0,76 ms | 0,05 ms | 1,01 ms
| 1_TrackMania Sunrise\1.4.6\LittleWalk | 0,80 ms | 0,04 ms | 1,38 ms
| 1_TrackMania Sunrise\1.5 Demo\DemoRace1 | 0,88 ms | 0,04 ms | 1,92 ms
| 8_Trackmania 2020\Training\cR­ç»f - 20 | 0,95 ms | 0,05 ms | 2,17 ms
| 8_Trackmania 2020\Training\Training - 20 | 0,99 ms | 0,07 ms | 1,96 ms
| 6_TrackMania 2\MP4\BaseValley | 1,10 ms | 0,04 ms | 2,07 ms
| 5_TrackMania Forever\2.11.11 Nations\E02-Endurance | 1,28 ms | 0,04 ms | 2,74 ms
| 7_TrackMania Turbo\VR\VR_Stadium_007 | 1,29 ms | 0,06 ms | 2,79 ms
| 1_TrackMania Sunrise\1.4\Paradise Island | 1,33 ms | 0,04 ms | 4,43 ms
| 6_TrackMania 2\MP4Valley\D13 | 1,51 ms | 0,06 ms | 3,00 ms
| 6_TrackMania 2\MP4Lagoon\B01 | 1,63 ms | 0,06 ms | 3,02 ms
| 6_TrackMania 2\MP3Platform\E03 - Ultimate Nightmare | 1,68 ms | 0,04 ms | 3,04 ms
| 5_TrackMania Forever\2.11.11 United\StuntC1 | 1,76 ms | 0,03 ms | 6,35 ms
| 5_TrackMania Forever\2.11.25\StarStadiumE | 1,81 ms | 0,04 ms | 2,42 ms
| 7_TrackMania Turbo\Solo\100 | 1,82 ms | 0,05 ms | 3,69 ms
| 6_TrackMania 2\MP3Valley\E01 | 1,85 ms | 0,08 ms | 3,86 ms
| 0_TrackMania\1.1\RaceD1 | 1,94 ms | 0,04 ms | 10,93 ms
| 0_TrackMania\1 Demo\Track6 | 1,98 ms | 0,03 ms | 10,21 ms
| 6_TrackMania 2\MP3Stadium\E02 | 1,99 ms | 0,06 ms | 4,48 ms
| 2_TrackMania Original\1.5\StuntsD1 | 2,14 ms | 0,04 ms | 8,96 ms
| 0_TrackMania\1 Beta\Track6 | 2,23 ms | 0,05 ms | 10,60 ms
| 0_TrackMania\1\PuzzleF2 | 2,28 ms | 0,05 ms | 17,25 ms
| 6_TrackMania 2\MP3Canyon\B10 | 2,92 ms | 0,03 ms | 3,24 ms
| 8_Trackmania 2020\Royal\NoTechLogic | 2,92 ms | 0,12 ms | 6,96 ms
| Community\CCP#04 - ODYSSEY | 3,31 ms | 0,05 ms | 8,62 ms
| 8_Trackmania 2020\20201001\秋季 2020 - 12 | 3,55 ms | 0,05 ms | 9,08 ms
| 8_Trackmania 2020\20201001\Fall 2020 - 12 | 3,56 ms | 0,06 ms | 8,96 ms
| 8_Trackmania 2020\20210401\Spring 2021 - 23 | 3,56 ms | 0,06 ms | 9,39 ms
| 8_Trackmania 2020\20200701\Summer 2020 - 11 | 3,62 ms | 0,07 ms | 8,98 ms
| 8_Trackmania 2020\20200701\夏季赛 2020 - 11 | 3,81 ms | 0,08 ms | 9,31 ms
| 8_Trackmania 2020\20210101\Winter 2021 - 15 | 4,48 ms | 0,06 ms | 12,76 ms
| 8_Trackmania 2020\20210701\Summer 2021 - 25 | 6,01 ms | 0,05 ms | 15,53 ms
| 8_Trackmania 2020\20211001\Fall 2021 - 16 | 13,46 ms | 0,12 ms | 19,10 ms

## Dependencies

### GBX.NET

- 0.0.1 - 0.4.1: SharpZipLib.NETStandard
- 0.1.0 - 0.4.1: Microsoft.CSharp
- 0.0.1 - 0.9.0: System.Drawing.Common
- 0.13.0+: TmEssentials
- 0.15.0+: Microsoft.Extensions.Logging.Abstractions

### GBX.NET.Imaging
- System.Drawing.Common

### GBX.NET.Json
- Newtonsoft.Json

## Usage

To parse a Gbx with a known type:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var map = GameBox.ParseNode<CGameCtnChallenge>("MyMap.Map.Gbx");
```

To parse a Gbx with an unknown type:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var node = GameBox.ParseNode("MyMap.Map.Gbx");

if (node is CGameCtnChallenge map)
{
    // Node data is available in map
}
else if (node is CGameCtnReplayRecord replay)
{
    // Node data is available in replay
}

// C# 7+

switch (node)
{
    case CGameCtnChallenge map:
        // Node data is available in map
        break;
    case CGameCtnReplayRecord replay:
        // Node data is available in replay
        break;
}
```

To get the Gbx metadata, use the `node.GetGbx()` property (only possible on the main node).

To save changes of the parsed Gbx file:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var node = GameBox.ParseNode("MyMap.Map.Gbx");

if (node is CGameCtnChallenge map)
{
    // Do changes with CGameCtnChallenge

    map.Save("MyMap.Map.Gbx");
}
else if (node is CGameCtnGhost ghost)
{
    // Do changes with CGameCtnGhost

    ghost.Save("MyGhost.Ghost.Gbx");
}
```

To save any supported `Node` to a Gbx file:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var replay = GameBox.ParseNode<CGameCtnReplayRecord>("MyReplay.Replay.Gbx");

// Ghosts property can be null if you would use the ParseNodeHeader method.
if (replay.Ghosts is not null)
{
    foreach (CGameCtnGhost ghost in replay.Ghosts)
    {
        ghost.Save($"{ghost.GhostNickname}.Ghost.Gbx");
    }
}
```

Compressed Gbx files require to include the GBX.NET.LZO library (or any similar implementation, but there are no other compatible at the moment). In most of the cases, the LZO compression is automatically detected after just referencing the library in the project. You don't require to have a `using GBX.NET.LZO;` anywhere.

On specific platforms like Blazor WebAssembly though, the dependency system works differently and (currently) GBX.NET struggles to automatically detect the LZO library.

In these cases (if just following the `MissingLzoException` message didn't solve the problem), add this line above the first attempt of parsing the Gbx. It should be called just once.

```cs
GBX.NET.Lzo.SetLzo(typeof(GBX.NET.LZO.MiniLZO));
```

## Conventions

### This convention is no longer relevant in GBX.NET 0.11.0+ when using the ParseNode method.

Make the code cleaner by **aliasing** the `Node` from the parsed `GameBox`:

```cs
var gbx = GameBox.Parse<CGameCtnChallenge>("MyMap.Map.Gbx");
var map = gbx.Node; // Like this

var bronzeTime = gbx.Node.BronzeTime; // WRONG !!!
var silverTime = map.SilverTime; // Correct
```

## License

- **The sub-library GBX.NET.LZO is licensed with [GNU General Public License v3.0](GBX.NET.LZO/LICENSE.GPL-3.0-or-later.md). If you're going to use this library, please license your work under GPL-3.0-or-later.**
- **Everything in the Samples folder is also licensed with [GNU General Public License v3.0](Samples/LICENSE.GPL-3.0-or-later.md).**
- The libraries GBX.NET, GBX.NET.Imaging, GBX.NET.Json, GBX.NET.Localization and the project DocGenerator **are licensed with MIT** and you can use them much more permissively.
- Information gathered from the project (chunk structure, parse examples, data structure, wiki information, markdown) is usable with [The Unlicense](https://unlicense.org/).
- Font of the logo is called [Raleway](https://fonts.google.com/specimen/Raleway) licensed under [SIL Open Font License](https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL). You can use the logo as a part of your media.

### Conclusion

Your work doesn't have to fall under the GNU GPL license if you're interested in either reading the header data only, or reading certain uncompressed Gbx files (usually the internal ones inside PAK files). If you're looking to read the content of a **compressed Gbx body** (applies to maps, replays and other user generated content), you **have to license your work with GNU GPL v3.0 or later**.

## Special thanks

Without these people, this project wouldn't be what it is today:

- Stefan Baumann (Solux)
- Melissa (Miss)
- florenzius
- Kim
- tilman
- James Romeril
- Mika Kuijpers (TheMrMiku)
- donadigo

And many thanks to every bug reporter!

## Alternative Gbx parsers

- [gbx.js](https://github.com/ThaumicTom/gbx.js) by ThaumicTom (Gbx header parser for clientside JavaScript)
- [ManiaPlanetSharp](https://github.com/stefan-baumann/ManiaPlanetSharp) by Solux (C# toolkit for accessing ManiaPlanet data, including Gbx parser used by ManiaExchange)
- [pygbx](https://github.com/donadigo/pygbx) by Donadigo (Gbx parser for Python)
