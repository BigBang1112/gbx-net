# CGameCtnMediaBlockCamera (0x030A2000)

### Inherits [CGameCtnMediaBlockCamera](CGameCtnMediaBlockCamera)

## Chunks

- [0x002](#0x002)
- [0x005](#0x005)
- [0x006](#0x006)

### 0x002

```cs
void Read(GameBoxReader r)
{
	int numKeys = r.ReadInt32();

	for(var i = 0; i < numKeys; i++)
	{
		float time = r.ReadSingle();
		int a = r.ReadInt32();
		int b = r.ReadInt32();
		int c = r.ReadInt32();
		Vec3 position = r.ReadVec3();
		Vec3 pitchYawRoll = r.ReadVec3(); // in radians
		float fov = r.ReadSingle();
		var d = r.ReadInt32();
        var e = r.ReadInt32();
    	var f = r.ReadInt32();
        var g = r.ReadInt32();
		Vec3 targetPosition = r.ReadVec3();
		Vec3 leftTangent = r.ReadVec3();
		Vec3 rightTangent = r.ReadVec3();
	}
}
```

### 0x005

```cs
void Read(GameBoxReader r)
{
	int numKeys = r.ReadInt32();
	
	for(var i = 0; i < numKeys; i++)
	{
		float time = r.ReadSingle();
		int a = r.ReadInt32();
		int b = r.ReadInt32();
		int c = r.ReadInt32();
		Vec3 position = r.ReadVec3();
		Vec3 pitchYawRoll = r.ReadVec3(); // in radians
		float fov = r.ReadSingle();
		int d = r.ReadInt32();
        int anchor = r.ReadInt32();
    	int e = r.ReadInt32();
    	int target = r.ReadInt32();
		Vec3 targetPosition = r.ReadVec3();
		Vec3 leftTangent = r.ReadVec3();
		Vec3 rightTangent = r.ReadVec3();
	}
}
```

### 0x006

```cs
void Read(GameBoxReader r)
{
	int version = r.ReadInt32();
	int numKeys = r.ReadInt32();
	
	for(var i = 0; i < numKeys; i++)
	{
		float time = r.ReadSingle();
		int a = r.ReadInt32();
		int anchorRot = r.ReadInt32();
		int anchor = r.ReadInt32();
		int anchorVis = r.ReadInt32();
		int target = r.ReadInt32();
		Vec3 position = r.ReadVec3();
		Vec3 pitchYawRoll = r.ReadVec3(); // in radians
		float fov = r.ReadSingle();
		int b = r.ReadInt32();
        int c = r.ReadInt32();
    	int d = r.ReadInt32();
        float zIndex = r.ReadSingle();
		Vec3 leftTangent = r.ReadVec3();
    	float e = r.ReadArray<float>(8);
		Vec3 rightTangent = r.ReadVec3();
    	float f = r.ReadArray<float>(8);
	}
}
```