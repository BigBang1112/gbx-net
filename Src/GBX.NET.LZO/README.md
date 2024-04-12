# GBX.NET.LZO

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.LZO?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.LZO/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

An LZO compression plugin for GBX.NET to allow de/serialization of compressed Gbx bodies. This official implementation uses minilzo 2.06.

The compression logic is split up from the read/write logic to **allow GBX.NET 2 library to be distributed under the MIT license**, as Oberhumer distributes the open source version of LZO under the GNU GPL v3. Therefore, using GBX.NET.LZO 2 requires you to license your project under the GNU GPL v3, see [License](#license).

**Gbx header is not compressed** and can contain useful information (icon data, replay time, ...), and also many of the **internal Gbx files from Pak files are not compressed**, so you can avoid LZO for these purposes.

## Usage

At the beginning of your program execution, you add the `Gbx.LZO = new MiniLZO();` to prepare the LZO compression. It should be run **only once**.

> This project example expects you to have `<ImplicitUsings>enable</ImplicitUsings>`. If this does not work for you, add `using System.Linq;`.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO; // Add this

Gbx.LZO = new MiniLZO(); // Add this ONLY ONCE and before you start using Parse methods

var map = Gbx.ParseNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

Console.WriteLine($"Block count: {map.GetBlocks().Count()}");
```

You should not get the LZO exception anymore when you read a compressed Gbx file.

## License

GBX.NET.LZO library is GNU GPL v3 Licensed.

If you use the LZO compression library, you must license your project under the GNU GPL v3.