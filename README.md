![GBX.NET](logo.png)

GBX.NET is a .NET GBX file parser library written in C#. GBX (or GameBox) is a file type available in many of the Nadeo games.

This GBX parser can recognize the entire GBX file. Modification of some GBX types is supported.
All versions of GBX are supported: ranging from TM1.0 to TM®.

Reading PAK file isn't currently supported.

## Compatibility

GBX.NET is compatible with .NET Standard 2.0 since version 0.2.0. Earlier versions are built on .NET Standard 2.1.

The library is also set to x86 assembly due to LZO compression problems in x64. It is unsure whenever this will be resolved.

## Dependencies

### GBX.NET
- SharpZipLib.NETStandard
- System.Drawing.Common

#### 0.1.0+
- Microsoft.CSharp

### GBX.NET.Json
- Newtonsoft.Json

## Usage

To parse a GBX with a known type:

```cs
var gbx = GameBox.Parse<CGameCtnChallenge>("MyMap.Map.Gbx"); // Node data is available in gbx.MainNode
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
