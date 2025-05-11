# GBX.NET.Imaging

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.Imaging?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.Imaging/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Provides extensions for image handling in GBX.NET (GDI+, Windows only).

**Exporting WEBP icons with correct rotation (TM2020 April 2022 update) is not supported**, use `GBX.NET.Imaging.SkiaSharp` or `GBX.NET.Imaging.ImageSharp` instead for this purpose.

## Framework support

- .NET 9 (Windows)
- .NET 8 (Windows)
- .NET 6 (Windows)
- .NET Standard 2.0

## Usage

### Export thumbnail from map

You can use `CGameCtnChallenge.Thumbnail` to get the pure JPEG bytes, but the thumbnail is going to be upside down. This is a long-standing bug in Nadeo games. `ExportThumbnail` method flips this thumbnail correctly.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Imaging; // You need to add this

var map = Gbx.ParseHeaderNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

map.ExportThumbnail("MyThumbnail.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
```

Quality could be degraded as the thumbnail. Use the `format` parameter to tweak the quality.

### Export icon from any `CGameCtnCollector` (before TM2020 April 2022)

This includes any Item.Gbx, Block.Gbx, Macroblock.Gbx, EDClassic.Gbx, Collection.Gbx, and many more...

```cs
using GBX.NET;
using GBX.NET.Engines.GameData; // Note it's GameData now instead of Game
using GBX.NET.Imaging; // You need to add this

var node = Gbx.ParseHeaderNode("Path/To/My.Item.Gbx");

if (node is CGameCtnCollector collector)
{
    collector.ExportIcon("MyIcon.png");
}
```

Quality should not be degraded as the icon is processed with pure color bytes and exported to PNG as default. Use the `format` parameter to tweak the exported format.

## License

GBX.NET.Imaging library is MIT Licensed.