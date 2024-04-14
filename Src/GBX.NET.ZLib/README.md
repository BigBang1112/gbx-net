# GBX.NET.ZLib

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.ZLib?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.ZLib/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

A zlib compression plugin for GBX.NET to allow de/serialization of compressed Gbx parts like ghost/entity data or lightmap cache. This official implementation uses ZLibDotNet.

The compression logic is split up from the read/write logic to separate dependencies better.

## Usage

Additional package `GBX.NET.LZO` is required in this example.

At the beginning of your program execution, you add the `Gbx.ZLib = new ZLib();` to prepare the ZLib compression. It should be run **only once**.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.ZLib; // Add this

Gbx.LZO = new MiniLZO();
Gbx.ZLib = new ZLib(); // Add this ONLY ONCE and before you start using Parse methods

var ghost = Gbx.ParseNode<CGameCtnGhost>("Path/To/My.Ghost.Gbx");

// SampleData will (likely) use ZLib decompression
foreach (var sample in ghost.SampleData.Samples)
{
    Console.WriteLine(sample.Position);
}
```

You should not get the ZLib exception anymore when you attempt to get `SampleData`.

## License

GBX.NET.ZLib library is MIT Licensed.

If you use the LZO compression library, you must license your project under the GNU GPL v3.