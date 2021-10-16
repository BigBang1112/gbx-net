# CGameCtnMediaClipGroup (0x0307A000)

## Chunks

- [0x001](#0x001)
- [0x002](#0x002)
- [0x003](#0x003)

### 0x001

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numClips = r.ReadInt32();
    for (var i = 0; i < numClips; i++)
        CGameCtnMediaClip clip = r.ReadNodeRef<CGameCtnMediaClip>();

    int numTriggers = r.ReadInt32();
    for (var i = 0; i < numTriggers; i++)
    {
        int numCoords = r.ReadInt32();
        for (var j = 0; j < numCoords; j++)
            Int3 coord = r.ReadInt3();
    }
}
```

### 0x002

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numClips = r.ReadInt32();
    for (var i = 0; i < numClips; i++)
        CGameCtnMediaClip clip = r.ReadNodeRef<CGameCtnMediaClip>();

    int numTriggers = r.ReadInt32();
    for (var i = 0; i < numTriggers; i++)
    {
        int numCoords = r.ReadInt32();
        for (var j = 0; j < numCoords; j++)
        {
            Int3 coord = r.ReadInt3();
            int u01 = r.ReadInt32();
            int u02 = r.ReadInt32();
            int u03 = r.ReadInt32();
            int u04 = r.ReadInt32();
        }
    }
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numClips = r.ReadInt32();
    for (var i = 0; i < numClips; i++)
        CGameCtnMediaClip clip = r.ReadNodeRef<CGameCtnMediaClip>();

    int numTriggers = r.ReadInt32();
    for (var i = 0; i < numTriggers; i++)
    {
        int numCoords = r.ReadInt32();
        for (var j = 0; j < numCoords; j++)
        {
            int u01 = r.ReadInt32();
            int u02 = r.ReadInt32();
            int u03 = r.ReadInt32();
            int u04 = r.ReadInt32();
            int u05 = r.ReadInt32();
            int u06 = r.ReadInt32();
            Int3 coord = r.ReadInt3();
        }
    }
}
```
