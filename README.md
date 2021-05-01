![GBX.NET](logo_outline.png)

[![Nuget](https://img.shields.io/nuget/v/GBX.NET?style=for-the-badge)](https://www.nuget.org/packages/GBX.NET/)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/BigBang1112/gbx-net?include_prereleases&style=for-the-badge)](https://github.com/BigBang1112/gbx-net/releases)
[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/BigBang1112/gbx-net?style=for-the-badge)](#)
[![GitHub](https://img.shields.io/github/license/BigBang1112/gbx-net?style=for-the-badge)](https://github.com/BigBang1112/gbx-net/blob/master/LICENSE)

GBX.NET is a GameBox (.Gbx) file parser library written in C# for .NET software framework. This file type can be seen in many of the Nadeo games like TrackMania, ShootMania or Virtual Skipper.

- GBX.NET can recognize **entire GBX files**, however **can't read all of the possible files**. GBX file is basically a serialized class from the GameBox engine, and all of these classes must be known to read. This is where you can help contributing to the project, by [exploring new chunks](https://github.com/BigBang1112/gbx-net/wiki/How-to-discover-nodes-and-chunks) (available very soon).
- GBX.NET can write GBX files which can be read by the parser, however this may not apply to all readable GBXs.
- All versions of GBX are supported: ranging from TM1.0 to TM®.
- Reading text-formatted GBX is not currently supported.
- Reading PAK file isn't currently supported.

| Extension | Node | Can read | Can write
| --- | --- | --- | ---
| Map.Gbx | [CGameCtnChallenge](GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes
| Replay.Gbx | [CGameCtnReplayRecord](GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | No
| Ghost.Gbx | [CGameCtnGhost](GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | Yes
| Clip.Gbx | [CGameCtnMediaClip](GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| EDClassic.Gbx | [CGameCtnBlockInfoClassic](GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.cs) | Yes | No
| Campaign.Gbx | [CGameCtnCampaign](GBX.NET/Engines/Game/CGameCtnCampaign.cs) | Yes | Yes
| Block.Gbx | [CGameItemModel](GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Macroblock.Gbx | [CGameCtnMacroBlockInfo](GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | Yes | No
| Item.Gbx | [CGameItemModel](GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| SystemConfig.Gbx | [CSystemConfig](GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes

## Compatibility

GBX.NET is compatible with **.NET Standard 2.0** since version 0.2.0. Earlier versions are built on .NET Standard 2.1.

Since version 0.5.2, the library is also compatible with **.NET Framework 4.5**.

The library is **not able to save GBX files in 64bit environment** due to the LZO compression implementation. To be able to save your GBX, set your platform target to **x86**.

## Techniques

The library does node caching to speed up node parsing:

- Currently, all nodes are cached when accessing the `Node` class for the first time, causing a slight (about 50 ms) delay which may go up with future library additions.
- *Selective node caching* would cache only the most common nodes related to the parsed GBX. This feature is planned for the future.

The library speeds up parse time by ignoring unused skippable chunks with *discover* feature:

- Discover basically means "parse a skippable chunk".
- Skippable chunks are parsed in-depth only if methods or properties request for them.
- Calling certain properties will discover all needed chunks synchronously before returning the value.
- You can pre-discover certain chunks on different threads to increase your code's performance.

## Benchmarks

Maps were selected from all kinds of Trackmania official campaigns picked by the biggest file size.

- CPU: **AMD Ryzen 3700X** (AMD64 Family 23 Model 113 Stepping 0, AuthenticAMD)
- Parse time: Required time to parse the full GBX file without skippable chunks
- Discover time: Required time to parse all of the skippable chunks
- **Delay on the first parse: 108.56ms**

| Map name | File size | Parse time (avg) | Discover time (avg)
| --- | --- | --- | ---
| PuzzleF2 | 14.31 kB | 18.74 ms | 70.08 ms
| Track6 | 10.44 kB | 14.72 ms | 1.06 ms
| Track6 | 10.44 kB | 14.53 ms | 0.92 ms
| RaceD1 | 9.43 kB | 13.58 ms | 0.87 ms
| RaceF7 | 6.65 kB | 4.45 ms | 1.16 ms
| DemoRaceB1 | 4.67 kB | 2.60 ms | 0.89 ms
| Paradise Island | 10.45 kB | 13.76 ms | 1.90 ms
| AirControl | 8.06 kB | 4.60 ms | 0.56 ms
| TrialTime | 12.17 kB | 11.13 ms | 1.07 ms
| LittleWalk | 12.97 kB | 7.93 ms | 0.85 ms
| CleanLanding | 7.30 kB | 3.57 ms | 0.39 ms
| TrialTime | 13.15 kB | 7.85 ms | 1.09 ms
| DemoRace1 | 10.88 kB | 7.01 ms | 4.12 ms
| StuntsD1 | 10.98 kB | 14.96 ms | 0.82 ms
| DemoRace3 | 6.88 kB | 3.23 ms | 0.38 ms
| Pro A-4 | 7.88 kB | 4.54 ms | 1.08 ms
| SnowC5 | 25.68 kB | 4.54 ms | 0.41 ms
| DesertE | 22.51 kB | 9.98 ms | 0.98 ms
| E02-Endurance | 32.12 kB | 13.76 ms | 1.42 ms
| StuntC1 | 27.49 kB | 16.19 ms | 0.94 ms
| StarStadiumE | 78.32 kB | 12.83 ms | 1.08 ms
| B10 | 585.46 kB | 66.31 ms | 224.22 ms
| E03 - Ultimate Nightmare | 512.32 kB | 14.66 ms | Exception
| E02 | 645.38 kB | 21.46 ms | 197.08 ms
| E01 | 768.55 kB | 14.30 ms | 190.28 ms
| BaseValley | 696.02 kB | 9.08 ms | 132.54 ms
| B01 | 582.04 kB | 9.69 ms | 106.62 ms
| D13 | 652.96 kB | 8.54 ms | 103.08 ms
| 100 | 777.73 kB | 11.15 ms | 112.63 ms
| VR_Stadium_007 | 631.45 kB | 5.03 ms | 146.11 ms
| Summer 2020 - 11 | 1432.61 kB | 27.62 ms | 232.01 ms
| ç§‹å­£ 2020 - 12 | 1610.14 kB | 21.89 ms | 349.09 ms

## Dependencies

### GBX.NET

- 0.0.1+
  - System.Drawing.Common

#### Obsolete

- 0.0.1 - 0.4.1: SharpZipLib.NETStandard
- 0.1.0 - 0.4.1: Microsoft.CSharp

### GBX.NET.Json
- Newtonsoft.Json

## Usage

To parse a GBX with a known type:

```cs
var gbx = GameBox.Parse<CGameCtnChallenge>("MyMap.Map.Gbx");
// Node data is available in gbx.MainNode
```

To parse a GBX with an unknown type:

```cs
var gbx = GameBox.Parse("MyMap.Map.Gbx");

if (gbx is GameBox<CGameCtnChallenge> gbxMap)
{
    // Node data is available in gbxMap.MainNode
}
else if (gbx is GameBox<CGameCtnReplayRecord> gbxReplay)
{
    // Node data is available in gbxReplay.MainNode
}
```

To save changes of the parsed GBX file:

```cs
var gbx = GameBox.Parse("MyMap.Map.Gbx");

if (gbx is GameBox<CGameCtnChallenge> gbxMap)
{
    // Do changes with CGameCtnChallenge

    gbxMap.Save("MyMap.Map.Gbx"); // Can be also a new file
}
else if (gbx is GameBox<CGameCtnGhost> gbxGhost)
{
    // Do changes with CGameCtnGhost

    gbxGhost.Save("MyGhost.Ghost.Gbx"); // Can be also a new file
}

gbx.Save(); // will throw an error
// GameBox with unspecified/unknown type can't be currently written back
```

To save any supported `Node` to a GBX file:

```cs
var gbxReplay = GameBox.Parse<CGameCtnReplayRecord>("MyReplay.Replay.Gbx");
CGameCtnReplayRecord replay = gbxReplay.MainNode;

foreach (CGameCtnGhost ghost in replay.Ghosts)
{
    var gbxGhost = new GameBox<CGameCtnGhost>(ghost); // Create a GameBox<T> with the Node object
    gbxGhost.Save("MyExtractedGhost.Ghost.Gbx"); // Save the new GameBox object to a GBX file
}
```

## Conventions

Make the code cleaner by **aliasing** the `MainNode` from the parsed `GameBox<T>`:

```cs
var gbx = GameBox.Parse<CGameCtnChallenge>("MyMap.Map.Gbx");
CGameCtnChallenge map = gbx.MainNode; // Like this

var bronzeTime = gbx.MainNode.BronzeTime; // WRONG !!!
var silverTime = map.SilverTime; // Correct
```

## License

The source code files fall under [GNU General Public License v3.0](LICENSE). Information gathered from the project (chunk structure, parse examples, data structure, wiki information, markdown) is usable with [The Unlicense](https://unlicense.org/).

Font of the logo is called [Raleway](https://fonts.google.com/specimen/Raleway) licensed under [SIL Open Font License](https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL). You can use the logo as a part of your media.

## Alternative GBX parsers

- [gbx.js](https://github.com/ThaumicTom/gbx.js) (GBX header parser for clientside JavaScript)
- [ManiaPlanetSharp](https://github.com/stefan-baumann/ManiaPlanetSharp) (C# toolkit for accessing ManiaPlanet data, including GBX parser used by ManiaExchange)
