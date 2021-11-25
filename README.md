![GBX.NET](logo_outline.png)

GBX.NET is a GameBox (.Gbx) file parser library written in C# for .NET software framework. This file type can be seen in many of the Nadeo games like TrackMania, ShootMania or Virtual Skipper.

[![Nuget](https://img.shields.io/nuget/v/GBX.NET?style=for-the-badge)](https://www.nuget.org/packages/GBX.NET/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/gbx-net?include_prereleases&style=for-the-badge)](https://github.com/BigBang1112/gbx-net/releases)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/BigBang1112/gbx-net?style=for-the-badge)](#)

- GBX.NET can recognize **entire GBX files**, however **can't read all of the possible files**. GBX file is basically a serialized class from the GameBox engine, and all of these classes must be known to read. This is where you can help contributing to the project, by [exploring new chunks](https://github.com/BigBang1112/gbx-net/wiki/How-to-discover-nodes-and-chunks) (available very soon).
- GBX.NET can write GBX files which can be read by the parser, however this may not apply to all readable GBXs.
- All versions of GBX are supported: ranging from TM1.0 to TM®.
- **GBX.NET 0.10.0+ is separated into MIT and GPL3.0, see [License](#License)**.
- Reading text-formatted GBX is not currently supported.
- Reading PAK files is partially supported with **GBX.NET.PAK** sublibrary, but it applies only to PAKs from TMUF and below, and most of contents cannot be read or crashes during decryption.

| Extension | Node | Can read | Can write
| --- | --- | --- | ---
| Map.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes
| Replay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | **No\***
| Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | **Yes**
| Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| EDClassic.Gbx | [CGameCtnBlockInfoClassic](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.cs) | Yes | No
| Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.cs) | Yes | Yes
| Block.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | Yes | No
| Item.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes

\* Consider extracting `CGameCtnGhost` from `CGameCtnReplayRecord`, transfer it over to `CGameCtnMediaBlockGhost`, add it to `CGameCtnMediaClip`, and save it as `.Clip.Gbx`, which you can then import in MediaTracker.

## Compatibility

- GBX.NET is compatible down to **.NET Standard 2.0** and **.NET Framework 4.8**.
- Current language version is 10 and you need Visual Studio 2022 to contribute to the project **(you don't need Visual Studio 2022 if you install it as NuGet package)**.
- The library supports **saving GBX files in 64bit environment** since 0.10.0. In the older versions, to be able to save your GBX, set your platform target to **x86**.

## Techniques

The library does node caching to speed up node parsing:

- Currently, all nodes are cached when accessing the `Node` class for the first time, causing a slight (up to 100 ms) delay which may go up with future library additions.
- *Selective node caching* would cache only the nodes the developer is interested in. This feature is planned for the future.

The library speeds up parse time by ignoring unused skippable chunks with *discover* feature:

- Discover basically means "parse a skippable chunk".
- Skippable chunks are parsed in-depth only if methods or properties request for them.
- Calling certain properties will discover all needed chunks synchronously before returning the value.
- You can pre-discover certain chunks on different threads to increase your code's performance.

## Benchmarks

Maps were selected from all kinds of Trackmania official campaigns picked by the biggest file size.

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT  [AttachedDebugger]
  Job-QCSCDP : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
```

| File name | Read | Read header | Write
| --- | --- | --- | ---
| 0_TrackMania\1.2.5 DemoSolo\DemoRaceB1 | 0,24 ms | 0,05 ms | 15,55 ms
| 4_TrackMania United\2\snowC5 | 0,33 ms | 0,06 ms | 17,39 ms
| 1_TrackMania Sunrise\1.4.7\CleanLanding | 0,35 ms | 0,06 ms | 22,24 ms
| 0_TrackMania\1.2.3\RaceF7 | 0,39 ms | 0,04 ms | 26,53 ms
| 2_TrackMania Original\1.5 Demo\DemoRace3 | 0,40 ms | 0,04 ms | 21,36 ms
| 4_TrackMania United\2.0.8\DesertE | 0,40 ms | 0,06 ms | 28,52 ms
| 8_Trackmania 2020\Training\cR­ç»f - 20 | 0,58 ms | 0,10 ms | 8,89 ms
| 8_Trackmania 2020\Training\Training - 20 | 0,60 ms | 0,10 ms | 9,44 ms
| 1_TrackMania Sunrise\1.4.5\AirControl | 0,71 ms | 0,06 ms | 16,76 ms
| 6_TrackMania 2\MP4\BaseValley | 0,84 ms | 0,09 ms | 51,70 ms
| 1_TrackMania Sunrise\1.4.5 Nvidia\TrialTime | 1,02 ms | 0,05 ms | 29,22 ms
| 3_TrackMania Nations ESWC\1.7.5\Pro A-4 | 1,07 ms | 0,05 ms | 14,08 ms
| 1_TrackMania Sunrise\1.5\TrialTime | 1,11 ms | 0,05 ms | 32,68 ms
| 7_TrackMania Turbo\VR\VR_Stadium_007 | 1,16 ms | 0,08 ms | 20,48 ms
| 1_TrackMania Sunrise\1.4.6\LittleWalk | 1,25 ms | 0,05 ms | 21,82 ms
| 1_TrackMania Sunrise\1.5 Demo\DemoRace1 | 1,30 ms | 0,05 ms | 27,46 ms
| 6_TrackMania 2\MP4Valley\D13 | 1,33 ms | 0,09 ms | 35,13 ms
| 6_TrackMania 2\MP4Lagoon\B01 | 1,57 ms | 0,06 ms | 41,17 ms
| 7_TrackMania Turbo\Solo\100 | 1,92 ms | 0,08 ms | 53,07 ms
| 6_TrackMania 2\MP3Platform\E03 - Ultimate Nightmare | 1,97 ms | 0,10 ms | 64,17 ms
| 6_TrackMania 2\MP3Valley\E01 | 2,01 ms | 0,10 ms | 63,66 ms
| 6_TrackMania 2\MP3Stadium\E02 | 2,03 ms | 0,10 ms | 85,97 ms
| 8_Trackmania 2020\Royal\NoTechLogic | 2,10 ms | 0,15 ms | 66,99 ms
| 5_TrackMania Forever\2.11.11 Nations\E02-Endurance | 2,15 ms | 0,09 ms | 23,97 ms
| 5_TrackMania Forever\2.11.25\StarStadiumE | 2,65 ms | 0,07 ms | 25,71 ms
| 1_TrackMania Sunrise\1.4\Paradise Island | 2,87 ms | 0,04 ms | 28,37 ms
| 5_TrackMania Forever\2.11.11 United\StuntC1 | 3,34 ms | 0,09 ms | 57,83 ms
| 8_Trackmania 2020\20200701\a¤?a­Lcu> 2020 - 11 | 3,37 ms | 0,10 ms | 185,57 ms
| 8_Trackmania 2020\20201001\ç§<a­L 2020 - 12 | 3,43 ms | 0,09 ms | 152,00 ms
| 8_Trackmania 2020\20201001\Fall 2020 - 12 | 3,43 ms | 0,09 ms | 151,81 ms
| 8_Trackmania 2020\20200701\Summer 2020 - 11 | 3,54 ms | 0,11 ms | 185,16 ms
| 6_TrackMania 2\MP3Canyon\B10 | 3,87 ms | 0,07 ms | 24,26 ms
| 2_TrackMania Original\1.5\StuntsD1 | 3,97 ms | 0,04 ms | 44,63 ms
| Community\CCP#04 - ODYSSEY | 3,99 ms | 0,09 ms | 104,41 ms
| 0_TrackMania\1 Demo\Track6 | 4,17 ms | 0,06 ms | 37,04 ms
| 0_TrackMania\1 Beta\Track6 | 4,21 ms | 0,05 ms | 36,77 ms
| 8_Trackmania 2020\20210401\Spring 2021 - 23 | 4,24 ms | 0,09 ms | 82,10 ms
| 8_Trackmania 2020\20210101\Winter 2021 - 15 | 4,26 ms | 0,14 ms | 135,10 ms
| 0_TrackMania\1.1\RaceD1 | 4,40 ms | 0,04 ms | 39,06 ms
| 0_TrackMania\1\PuzzleF2 | 5,20 ms | 0,04 ms | 57,21 ms
| 8_Trackmania 2020\20210701\Summer 2021 - 25 | 5,55 ms | 0,09 ms | 160,57 ms
| 8_Trackmania 2020\20211001\Fall 2021 - 16 | 14,03 ms | 0,16 ms | 1204,03 ms

## Dependencies

### GBX.NET

- 0.0.1 - 0.4.1: SharpZipLib.NETStandard
- 0.1.0 - 0.4.1: Microsoft.CSharp
- 0.0.1 - 0.9.0: System.Drawing.Common

### GBX.NET.Imaging
- System.Drawing.Common

### GBX.NET.Json
- Newtonsoft.Json

## Usage

**Since GBX.NET 0.11.0, the base of parsing has been simplified.**

To parse a GBX with a known type:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var map = GameBox.ParseNode<CGameCtnChallenge>("MyMap.Map.Gbx");
```

To parse a GBX with an unknown type:

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

To get GBX metadata or header chunks, use the `node.GBX` property.

To save changes of the parsed GBX file:

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

To save any supported `Node` to a GBX file:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var replay = GameBox.ParseNode<CGameCtnReplayRecord>("MyReplay.Replay.Gbx");

foreach (CGameCtnGhost ghost in replay.Ghosts)
{
    ghost.Save("MyExtractedGhost.Ghost.Gbx");
}
```

## Conventions

### This convention is no longer relevant in GBX.NET 0.11.0+ when using the ParseNode method.

Make the code cleaner by **aliasing** the `Node` from the parsed `GameBox<T>`:

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

Your work doesn't have to fall under the GNU GPL license if you're interested in either reading the header data only, or reading certain uncompressed GBX files (usually the internal ones inside PAK files). If you're looking to read the content of a **compressed GBX body** (applies to maps, replays and other user generated content), you **have to license your work with GNU GPL v3.0 or later**.

## Alternative GBX parsers

- [gbx.js](https://github.com/ThaumicTom/gbx.js) by ThaumicTom (GBX header parser for clientside JavaScript)
- [ManiaPlanetSharp](https://github.com/stefan-baumann/ManiaPlanetSharp) by Solux (C# toolkit for accessing ManiaPlanet data, including GBX parser used by ManiaExchange)
- [pygbx](https://github.com/donadigo/pygbx) by Donadigo (GBX parser for Python)
