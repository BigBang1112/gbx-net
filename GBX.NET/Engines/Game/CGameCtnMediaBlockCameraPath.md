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
        float time = r.ReadSingle();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        bool anchorRot = r.ReadBoolean();
        int anchor = r.ReadInt32();
        bool anchorVis = r.ReadBoolean();
        int target = r.ReadInt32();
        Vec3 targetPosition = r.ReadVec3();
        float u01 = r.ReadSingle();
        float u02 = r.ReadSingle();
        float u03 = r.ReadSingle();
        float u04 = r.ReadSingle();
        float u05 = r.ReadSingle();
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
        float time = r.ReadSingle();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        bool anchorRot = r.ReadBoolean();
        int anchor = r.ReadInt32();
        bool anchorVis = r.ReadBoolean();
        int target = r.ReadInt32();
        Vec3 targetPosition = r.ReadVec3();
        float u01 = r.ReadSingle();
        float u02 = r.ReadSingle();
        float u03 = r.ReadSingle();
        float u04 = r.ReadSingle();
        float u05 = r.ReadSingle();
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
        float time = r.ReadSingle();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();
        float zIndex = r.ReadSingle();

        int u01 = r.ReadInt32();
        int u02 = r.ReadInt32();
        int u03 = r.ReadInt32();
        int u04 = r.ReadInt32();
        int u05 = r.ReadInt32();
        int u06 = r.ReadInt32();
        int u07 = r.ReadInt32();
        int u08 = r.ReadInt32();
        int u09 = r.ReadInt32();
        int u10 = r.ReadInt32();
        int u11 = r.ReadInt32();
        int u12 = r.ReadInt32();
        int u13 = r.ReadInt32();
        int u14 = r.ReadInt32();
    }
}
```
