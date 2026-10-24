# GBX.NET.Builders

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.Builders?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.Builders/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Generated fluent builders for GBX.NET 2. A builder creates a node with the chunks and defaults of a selected game.

The game version interfaces, such as `IGameCtnMediaClipTMF`, are part of the main GBX.NET package.

## Framework support

- .NET 10
- .NET 9
- .NET 8
- .NET Standard 2.0

## Usage

The package adds a `Create()` method to each supported node class:

```cs
using GBX.NET.Engines.Game;

CGameCtnMediaClip clip = CGameCtnMediaClip.Create()
    .WithName("Intro")
    .ForTMF()
    .Build();
```

See [Generated node builders](https://github.com/BigBang1112/gbx-net/blob/master/Docs/NodeBuilders.md) for game selection, interfaces and custom recipes.
