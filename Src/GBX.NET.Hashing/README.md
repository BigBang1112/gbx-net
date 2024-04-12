# GBX.NET.Hashing

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.Hashing?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.Hashing/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Hashing features (currently only CRC32) for GBX.NET 2. This hashing function is used in maps when changing the `MapUid` or `HashedPassword`.

## Usage

Additional package `GBX.NET.LZO` is required in this example.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;
using GBX.NET.Hashing;

Gbx.LZO = new MiniLZO();
Gbx.CRC32 = new CRC32(); // You need to add this

var map = Gbx.ParseNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

map.RemovePassword();

map.Save("MapWithNoPassword.Map.Gbx");
```

This example could be broken in older Trackmania games, as these games use older class IDs, to fix this problem, save the map like this:

```cs
map.Save("MapWithNoPassword.Map.Gbx", new()
{
    ClassIdRemapMode = ClassIdRemapMode.Id2006
});
```

## License

GBX.NET.Hashing library is MIT Licensed.

However, if you would use `GBX.NET.LZO` package with it (which is usually required), you'd need to follow the GNU GPL v3 License. See License section on the main README for more details.