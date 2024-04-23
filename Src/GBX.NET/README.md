# GBX.NET

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

A general purpose library for Gbx files - data from Nadeo games like Trackmania or Shootmania, written in C#/.NET. It supports high performance serialization and deserialization of 200+ Gbx classes.

For more details, see the main README.

## Framework support

Due to the recently paced evolution of .NET, framework support has been limited only to a few ones compared to GBX.NET 1:

- .NET 8
- .NET 6
- .NET Standard 2.0

You can still use GBX.NET 2 on the old .NET Framework, but the performance of the library could be degraded.

## Usage

> These examples expect you to have `<ImplicitUsings>enable</ImplicitUsings>`. If this does not work for you, add `using System.Linq;` at the top.

### Map information

Additional package `GBX.NET.LZO` is required in this example.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

Gbx.LZO = new MiniLZO();

var map = Gbx.ParseNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

foreach (var block in map.GetBlocks().GroupBy(x => x.Name))
{
    Console.WriteLine($"{block.Key}: {block.Count()}");
}
```

### Map information from Gbx header

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

var map = Gbx.ParseHeaderNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

Console.WriteLine(map.MapName);
Console.WriteLine(map.Xml);
```

Header contains a lot less information than the full node.

### Modify and save a map

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

Gbx.LZO = new MiniLZO();

var gbx = Gbx.Parse<CGameCtnChallenge>("Path/To/My.Map.Gbx");
var map = gbx.Node; // See Clarity section for more info

map.MapName = "My new map name";

gbx.Save("Path/To/MyNew.Map.Gbx");
```

The trick here is that the Gbx properties are saved in the `gbx` object variable (`Gbx` class).

If you were to go with `ParseNode` in this case, this would **not work for TMF and older games**, but it is still possible if you specify the Gbx parameters in the `Save` method:

```cs
map.Save("Path/To/MyNew.Map.Gbx", new()
{
    PackDescVersion = 2 // Latest known PackDesc version in TMF
});
```

For TMS or TMN ESWC, you would have to specify `ClassIdRemapMode` for example:

```cs
map.Save("Path/To/MyNew.Map.Gbx", new()
{
    ClassIdRemapMode = ClassIdRemapMode.Id2006
    PackDescVersion = 1
});
```

These save parameters depend on the game of choice, but **since Trackmania 2, this does not matter**.

### Processing multiple Gbx types

Additional package `GBX.NET.LZO` is required in this example.

This example shows how you can retrieve ghost objects from multiple different types of Gbx:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

Gbx.LZO = new MiniLZO();

var node = Gbx.ParseNode("Path/To/My.Gbx");

var ghost = node switch
{
    CGameCtnReplayRecord replay => replay.GetGhosts().FirstOrDefault(),
    CGameCtnMediaClip clip => clip.GetGhosts().FirstOrDefault(),
    CGameCtnGhost g => g,
    _ => null
};

if (ghost is null)
{
    Console.WriteLine("This Gbx file does not have any ghost.");
}
else
{
    Console.WriteLine("Time: {0}", ghost.RaceTime);
}
```

Using pattern matching with non-generic Parse methods is a safer approach (no exceptions on different Gbx types), but less trim-friendly.

### Read a large amount of replay metadata quickly

In case you only need the most basic information about many of the most common Gbx files (maps, replays, items, ...), do not read the full Gbx file, but only the header part. It is a great performance benefit for disk scans.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

foreach (var filePath in Directory.EnumerateFiles("Path/To/My/Directory", "*.Replay.Gbx", SearchOption.AllDirectories))
{
    try
    {
        DisplayBasicReplayInfo(filePath);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Gbx exception occurred {Path.GetFileName(filePath)}: {ex}");
    }
}

void DisplayBasicReplayInfo(string filePath)
{
    var nodeHeader = Gbx.ParseHeaderNode(filePath);

    if (nodeHeader is CGameCtnReplayRecord replay)
    {
        Console.WriteLine($"{replay.MapInfo}: {replay.Time}");
    }
}
```

## File types

Some of the common types to start with (a lot more are supported):

| Latest extension | Class | Can read | Can write | Other extension/s
| --- | --- | --- | --- | ---
| Map.Gbx | CGameCtnChallenge | Yes | Yes | Challenge.Gbx
| Replay.Gbx | CGameCtnReplayRecord | Yes | No
| Ghost.Gbx | CGameCtnGhost | Yes | Yes
| Clip.Gbx | CGameCtnMediaClip | Yes | Yes
| Item.Gbx | CGameItemModel | Yes | Yes | Block.Gbx
| Mat.Gbx | CPlugMaterialUserInst | Yes | Yes
| Mesh.Gbx | CPlugSolid2Model | Yes | Yes
| Shape.Gbx | CPlugSurface | Yes | Yes
| Macroblock.Gbx | CGameCtnMacroBlockInfo | Yes | Yes
| LightMapCache.Gbx | CHmsLightMapCache | No | No
| SystemConfig.Gbx | CSystemConfig | Yes | Yes
| FidCache.Gbx | CMwRefBuffer | Yes | Yes
| Scores.Gbx | CGamePlayerScore | Yes | No

## Supported games

Many *essential* Gbx files from many games are supported:

- **Trackmania (2020)**, April 2024 update
- **ManiaPlanet 4**(.1), TM2/SM
- **Trackmania Turbo**
- ManiaPlanet 3, TM2/SM
- ManiaPlanet 2, TM2/SM
- ManiaPlanet 1, TM2
- **TrackMania Forever**, Nations/United
- Virtual Skipper 5
- TrackMania United
- **TrackMania Nations ESWC**
- **TrackMania Sunrise eXtreme**
- TrackMania Original
- TrackMania Sunrise
- TrackMania Power Up
- TrackMania (1.0)

## License

GBX.NET library *(this package)* is MIT Licensed.

However, if you would use `GBX.NET.LZO` package with it (which is usually required), you'd need to follow the GNU GPL v3 License. See License section on the main README for more details.

## Special thanks

Without these people, this project wouldn't be what it is today (ordered by impact):

- Stefan Baumann (Solux)
- Melissa (Miss)
- florenzius
- Kim
- tilman
- schadocalex
- James Romeril
- frolad (Juice)
- Mika Kuijpers (TheMrMiku)
- donadigo

And many thanks to every bug reporter!