# CGameCtnMediaBlockCameraCustom (0x030A2000)

### Inherits [CGameCtnMediaBlockCamera.md](CGameCtnMediaBlockCamera.md)

## Chunks

- [0x001](#0x001)
- [0x002](#0x002)
- [0x005](#0x005)
- [0x006](#0x006)

### 0x001

```cs
void Read(GameBoxReader r)
{
    int numKeys = r.ReadInt32();

    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        int u01 = r.ReadInt32(); // 1
        int u02 = r.ReadInt32(); // 0
        int u03 = r.ReadInt32(); // 0
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        int u04 = r.ReadInt32(); // 0
        int u05 = r.ReadInt32(); // -1
        int u06 = r.ReadInt32(); // 1
        int u07 = r.ReadInt32(); // -1
        float u08 = r.ReadSingle();
        float u09 = r.ReadSingle();
        float u10 = r.ReadSingle();
        float u11 = r.ReadSingle();
        float u12 = r.ReadSingle();
    }
}
```

### 0x002

```cs
void Read(GameBoxReader r)
{
    int numKeys = r.ReadInt32();

    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        int u01 = r.ReadInt32();
        int u02 = r.ReadInt32();
        int u03 = r.ReadInt32();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        int u04 = r.ReadInt32();
        int u05 = r.ReadInt32();
        int u06 = r.ReadInt32();
        int u07 = r.ReadInt32();
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
    
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        int u01 = r.ReadInt32();
        int u02 = r.ReadInt32();
        int u03 = r.ReadInt32();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        int u04 = r.ReadInt32();
        int anchor = r.ReadInt32();
        int u05 = r.ReadInt32();
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
    
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        int u01 = r.ReadInt32();
        int anchorRot = r.ReadInt32();
        int anchor = r.ReadInt32();
        int anchorVis = r.ReadInt32();
        int target = r.ReadInt32();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        int u02 = r.ReadInt32();
        int u03 = r.ReadInt32();
        int u04 = r.ReadInt32();
        float zIndex = r.ReadSingle();
        Vec3 leftTangent = r.ReadVec3();
        float u05 = r.ReadArray<float>(8);
        Vec3 rightTangent = r.ReadVec3();
        float u06 = r.ReadArray<float>(8);
    }
}
```
