# GBX.NET.Imaging.SkiaSharp

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.Imaging.SkiaSharp?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.Imaging.SkiaSharp/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Provides extensions for image handling in GBX.NET using SkiaSharp.

## Framework support

- .NET 10
- .NET 9
- .NET 8
- .NET Standard 2.0

## Usage

### Export thumbnail from map

You can use `CGameCtnChallenge.Thumbnail` to get the pure JPEG bytes, but the thumbnail is going to be upside down. This is a long-standing bug in Nadeo games. `ExportThumbnail` method flips this thumbnail correctly.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Imaging.SkiaSharp; // You need to add this

var map = Gbx.ParseHeaderNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

map.ExportThumbnail("MyThumbnail.jpg", SkiaSharp.SKEncodedImageFormat.Jpeg, quality: 95);
```

### Export icon from any `CGameCtnCollector`

This includes any Item.Gbx, Block.Gbx, Macroblock.Gbx, EDClassic.Gbx, Collection.Gbx, and many more...

For TM2020 after April 2022 update, the WEBP icon is also rotated correctly.

```cs
using GBX.NET;
using GBX.NET.Engines.GameData; // Note it's GameData now instead of Game
using GBX.NET.Imaging.SkiaSharp; // You need to add this

var node = Gbx.ParseHeaderNode("Path/To/My.Item.Gbx");

if (node is CGameCtnCollector collector)
{
    collector.ExportIcon("MyIcon.png");
}
```

Quality should not be degraded as the icon is processed with either pure color bytes or lossless WEBP and exported to PNG as default. Use the `format` and `quality` parameters to tweak the exported format.

## License

GBX.NET.Imaging.SkiaSharp library is MIT Licensed.