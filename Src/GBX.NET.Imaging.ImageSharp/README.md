# GBX.NET.Imaging.ImageSharp

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.Imaging.SkiaSharp?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.Imaging.ImageSharp/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Provides extensions for image handling in GBX.NET using ImageSharp.

Async methods are available.

## Framework support

- .NET 10
- .NET 9
- .NET 8

## Usage

### Export thumbnail from map

You can use `CGameCtnChallenge.Thumbnail` to get the pure JPEG bytes, but the thumbnail is going to be upside down. This is a long-standing bug in Nadeo games. `ExportThumbnail` method flips this thumbnail correctly.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Imaging.ImageSharp; // You need to add this

var map = Gbx.ParseHeaderNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

map.ExportThumbnail("MyThumbnail.jpg", new JpegEncoder { Quality = quality });
```

### Export icon from any `CGameCtnCollector`

This includes any Item.Gbx, Block.Gbx, Macroblock.Gbx, EDClassic.Gbx, Collection.Gbx, and many more...

For TM2020 after April 2022 update, the WEBP icon is also rotated correctly.

```cs
using GBX.NET;
using GBX.NET.Engines.GameData; // Note it's GameData now instead of Game
using GBX.NET.Imaging.ImageSharp; // You need to add this

var node = Gbx.ParseHeaderNode("Path/To/My.Item.Gbx");

if (node is CGameCtnCollector collector)
{
    collector.ExportIcon("MyIcon.png");
}
```

Quality should not be degraded as the icon is processed with either pure color bytes or lossless WEBP and exported to PNG as default. Use the `encoder` parameter to tweak the export.

## License

GBX.NET.Imaging.ImageSharp is MIT licensed.

ImageSharp 4 requires a [Six Labors license](https://docs.sixlabors.com/articles/imagesharp/index.html#license) when building this project. Place your supplied `sixlabors.lic` file in this project directory. The file is ignored by Git. You can also set the `SixLaborsLicenseFile` MSBuild property to the path of a license file stored elsewhere, or set `SixLaborsLicenseKey` to the full license string. Keep the license file and key out of commits and build logs.

If you need a license, apply for a [Community license key](https://licensing.sixlabors.com/) if the project qualifies, or use [Six Labors pricing](https://sixlabors.com/pricing/) for a commercial license. Use this project's public repository URL in an open source application.
