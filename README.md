![GBX.NET](logo.png)

GBX.NET is a .NET GBX file parser library written in C#. GBX (or GameBox) is a file type available in many of the Nadeo games.

This GBX parser can recognize the entire GBX file. Modification of some GBX types is supported.
All versions of GBX are supported: ranging from TM1.0 to TM®.

Reading PAK file isn't currently supported.

## Compatibility

GBX.NET is currently compatible with .NET Standard 2.1. At the moment you can't use the library in .NET Framework projects or .NET Core 2 or lower. Wider compatibility is planned for the future.

The library is also set to x86 assembly due to LZO compression problems in x64. It is unsure whenever this will be resolved.

## Dependencies

The library requires these dependencies:
- SharpZipLib.NETStandard
- System.Drawing.Common

Since 0.1.0, the library requires these dependencies:
- Microsoft.CSharp

## Usage

To parse a GBX with a known type:

```cs
GameBox<CGameCtnChallenge> gbx = new GameBox<CGameCtnChallenge>();
gbx.Load("MyMap.Map.Gbx");

// Node data is available in gbx.MainNode
```

To parse a GBX with an unknown type:

```cs
using (var fs = File.OpenRead("MyMap.Map.Gbx"))
{
	var type = GameBox.GetGameBoxType(fs);
	fs.Seek(0, SeekOrigin.Begin);

	if (type == null)
		gbx = new GameBox();
	else
		gbx = (GameBox)Activator.CreateInstance(type);
					
	if (gbx.Read(fs))
	{
		if(gbx is GameBox<CGameCtnChallenge> gbxMap)
		{
			// Node data is available in gbxMap.MainNode
		}
		else if(gbx is GameBox<CGameCtnReplayRecord> gbxReplay)
		{
			// Node data is available in gbxReplay.MainNode
		}
	}
}
```
