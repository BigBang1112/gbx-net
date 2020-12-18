# CGameCtnMediaBlockCameraPath (0x030A1000)

### Inherits [CGameCtnMediaBlockCamera](CGameCtnMediaBlockCamera.md)

## Chunks

- [0x000](#0x000)
- [0x002](#0x002)
- [0x003](#0x003)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
	for (var i = 0; i < numKeys; i++)
	{
		var time = r.ReadSingle();
		var position = r.ReadVec3();
		var pitchYawRoll = r.ReadVec3(); // in radians
		var fov = r.ReadSingle();
		var anchorRot = r.ReadBoolean();
		var anchor = r.ReadInt32();
		var anchorVis = r.ReadBoolean();
		var target = r.ReadInt32();
		var targetPosition = r.ReadVec3();
		var a = r.ReadSingle();
		var b = r.ReadSingle();
		var c = r.ReadSingle();
		var d = r.ReadSingle();
		var e = r.ReadSingle();
	}
}
```

### 0x002

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
	for (var i = 0; i < numKeys; i++)
	{
    	var time = r.ReadSingle();
		var position = r.ReadVec3();
		var pitchYawRoll = r.ReadVec3(); // in radians
		var fov = r.ReadSingle();
		var anchorRot = r.ReadBoolean();
		var anchor = r.ReadInt32();
		var anchorVis = r.ReadBoolean();
		var target = r.ReadInt32();
		var targetPosition = r.ReadVec3();
		var a = r.ReadSingle();
		var b = r.ReadSingle();
		var c = r.ReadSingle();
		var d = r.ReadSingle();
		var e = r.ReadSingle();
	}
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
	for (var i = 0; i < numKeys; i++)
	{
    	var time = r.ReadSingle();
		var position = r.ReadVec3();
		var pitchYawRoll = r.ReadVec3(); // in radians
		var fov = r.ReadSingle();
		var zIndex = r.ReadSingle();

		var a = r.ReadInt32();
		var b = r.ReadInt32();
		var c = r.ReadInt32();
		var d = r.ReadInt32();
		var e = r.ReadInt32();
		var f = r.ReadInt32();
		var g = r.ReadInt32();
		var h = r.ReadInt32();
		var i = r.ReadInt32();
		var j = r.ReadInt32();
		var k = r.ReadInt32();
		var l = r.ReadInt32();
		var m = r.ReadInt32();
		var n = r.ReadInt32();
	}
}
```
