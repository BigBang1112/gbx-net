# GBX.NET.NewtonsoftJson

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.NewtonsoftJson?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.NewtonsoftJson/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Provides extensions for JSON serialization with `Newtonsoft.Json`.

## Framework support

- .NET 10
- .NET 9
- .NET 8
- .NET Standard 2.0

## Usage

### Convert Gbx and any `CMwNod` to JSON

Additional package `GBX.NET.LZO` is required in this example.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.NewtonsoftJson; // Add this

Gbx.LZO = new Lzo();

var gbx = Gbx.Parse<CGameCtnChallenge>("Path/To/My.Map.Gbx");

string jsonGbx = gbx.ToJson();
string jsonNode = gbx.Node.ToJson();
```

### Print JSON of Gbx to console using `TextWriter`

Additional package `GBX.NET.LZO` is required in this example.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.NewtonsoftJson; // Add this

Gbx.LZO = new Lzo();

var gbx = Gbx.Parse<CGameCtnChallenge>("Path/To/My.Map.Gbx");

gbx.ToJson(Console.Out);
```

## License

GBX.NET.NewtonsoftJson library is MIT Licensed.