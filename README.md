# GBX.NET

GBX.NET is a .NET GBX file parser library written in C#. GBX (or GameBox) is a file type available in many of the Nadeo games.

This GBX parser can recognize the entire GBX file. Modification of some GBX types is supported.
All versions of GBX are supported: ranging from TM1.0 to TM®.

## Compatibility

GBX.NET is currently compatible with .NET Standard 2.1. At the moment you can't use the library in .NET Framework projects or .NET Core 2 or lower. Wider compatibility is planned for the future.

## Dependencies

The library currently requires these dependencies:
- SharpZipLib.NETStandard
- System.Drawing.Common

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