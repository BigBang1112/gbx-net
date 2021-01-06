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
    int version = r.ReadInt32();

    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        Vec3 position = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3(); // in radians
        float fov = r.ReadSingle();

        if (version >= 3)
            float nearZ = r.ReadSingle();

        var anchorRot = r.ReadBoolean();
        var anchor = r.ReadInt32();
        var anchorVis = r.ReadBoolean();
        var target = r.ReadInt32();
        var targetPosition = r.ReadVec3();

        int u01 = r.ReadSingle();
        int u02 = r.ReadSingle();
        int u03 = r.ReadSingle();
        int u04 = r.ReadSingle();
        int u05 = r.ReadSingle();

        if (version >= 4)
        {
            int u06 = r.ReadInt32();
            int u07 = r.ReadInt32();
        }
    }
}
```
