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
- Reading PAK files isn't currently supported.

| Extension | Node | Can read | Can write
| --- | --- | --- | ---
| Map.Gbx | [CGameCtnChallenge](GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes
| Replay.Gbx | [CGameCtnReplayRecord](GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | **No\***
| Ghost.Gbx | [CGameCtnGhost](GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | **Yes**
| Clip.Gbx | [CGameCtnMediaClip](GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| EDClassic.Gbx | [CGameCtnBlockInfoClassic](GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.cs) | Yes | No
| Campaign.Gbx | [CGameCtnCampaign](GBX.NET/Engines/Game/CGameCtnCampaign.cs) | Yes | Yes
| Block.Gbx | [CGameItemModel](GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Macroblock.Gbx | [CGameCtnMacroBlockInfo](GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | Yes | No
| Item.Gbx | [CGameItemModel](GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| SystemConfig.Gbx | [CSystemConfig](GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes

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

| File name | Median [ms] |
|----------------------------------------------------------------- |------------:|
|             0_TrackMania\1.2.5 DemoSolo\DemoRaceB1.Challenge.Gbx |   0.2655 ms |
|                       4_TrackMania United\2\snowC5.Challenge.Gbx |   0.3346 ms |
|            1_TrackMania Sunrise\1.4.7\CleanLanding.Challenge.Gbx |   0.3533 ms |
|                          0_TrackMania\1.2.3\RaceF7.Challenge.Gbx |   0.3625 ms |
|           2_TrackMania Original\1.5 Demo\DemoRace3.Challenge.Gbx |   0.3650 ms |
|                  4_TrackMania United\2.0.8\DesertE.Challenge.Gbx |   0.4306 ms |
|                   8_Trackmania 2020\Training\cR­ç»f - 20.Map.Gbx |   0.6456 ms |
|                 8_Trackmania 2020\Training\Training - 20.Map.Gbx |   0.6863 ms |
|              1_TrackMania Sunrise\1.4.5\AirControl.Challenge.Gbx |   0.7173 ms |
|                            6_TrackMania 2\MP4\BaseValley.Map.Gbx |   0.8236 ms |
|                 1_TrackMania Sunrise\1.5\TrialTime.Challenge.Gbx |   1.0711 ms |
|            3_TrackMania Nations ESWC\1.7.5\Pro A-4.Challenge.Gbx |   1.0362 ms |
|        1_TrackMania Sunrise\1.4.5 Nvidia\TrialTime.Challenge.Gbx |   1.0148 ms |
|                     7_TrackMania Turbo\VR\VR_Stadium_007.Map.Gbx |   1.1537 ms |
|                             6_TrackMania 2\MP4Valley\D13.Map.Gbx |   1.3145 ms |
|            1_TrackMania Sunrise\1.5 Demo\DemoRace1.Challenge.Gbx |   1.3534 ms |
|                             6_TrackMania 2\MP4Lagoon\B01.Map.Gbx |   1.6401 ms |
|              1_TrackMania Sunrise\1.4.6\LittleWalk.Challenge.Gbx |   1.7067 ms |
|      6_TrackMania 2\MP3Platform\E03 - Ultimate Nightmare.Map.Gbx |   1.8748 ms |
|                             6_TrackMania 2\MP3Valley\E01.Map.Gbx |   1.9222 ms |
|                              7_TrackMania Turbo\Solo\100.Map.Gbx |   1.9219 ms |
|                            6_TrackMania 2\MP3Stadium\E02.Map.Gbx |   2.1972 ms |
| 5_TrackMania Forever\2.11.11 Nations\E02-Endurance.Challenge.Gbx |   2.2746 ms |
|                      8_Trackmania 2020\Royal\NoTechLogic.Map.Gbx |   2.3433 ms |
|          5_TrackMania Forever\2.11.25\StarStadiumE.Challenge.Gbx |   2.6439 ms |
|              8_Trackmania 2020\20200701\Summer 2020 - 11.Map.Gbx |   3.1039 ms |
|           1_TrackMania Sunrise\1.4\Paradise Island.Challenge.Gbx |   3.1968 ms |
|                             6_TrackMania 2\MP3Canyon\B10.Map.Gbx |   2.8478 ms |
|              8_Trackmania 2020\20201001\ç§<a­L 2020 - 12.Map.Gbx |   3.5120 ms |
|        5_TrackMania Forever\2.11.11 United\StuntC1.Challenge.Gbx |   3.5581 ms |
|           8_Trackmania 2020\20200701\a¤?a­Lcu> 2020 - 11.Map.Gbx |   3.6620 ms |
|                8_Trackmania 2020\20201001\Fall 2020 - 12.Map.Gbx |   3.7494 ms |
|              8_Trackmania 2020\20210401\Spring 2021 - 23.Map.Gbx |   3.9715 ms |
|                         0_TrackMania\1 Demo\Track6.Challenge.Gbx |   4.5466 ms |
|                 2_TrackMania Original\1.5\StuntsD1.Challenge.Gbx |   4.3262 ms |
|              8_Trackmania 2020\20210101\Winter 2021 - 15.Map.Gbx |   4.2236 ms |
|                            0_TrackMania\1.1\RaceD1.Challenge.Gbx |   4.3978 ms |
|                         0_TrackMania\1 Beta\Track6.Challenge.Gbx |   4.5578 ms |
|                               Community\CCP#04 - ODYSSEY.Map.Gbx |   4.6079 ms |
|                            0_TrackMania\1\PuzzleF2.Challenge.Gbx |   5.3316 ms |
|              8_Trackmania 2020\20210701\Summer 2021 - 25.Map.Gbx |   5.7061 ms |
|                8_Trackmania 2020\20211001\Fall 2021 - 16.Map.Gbx |  14.8491 ms |

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
var map = GameBox.ParseNode<CGameCtnChallenge>("MyMap.Map.Gbx");
```

To parse a GBX with an unknown type:

```cs
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
