# CGameCtnMediaClip (0x03079000)

## Chunks

- [0x002](#0x002)
- [0x003](#0x003)
- [0x004](#0x004)
- [0x005](#0x005)
- 0x006
- [0x007](#0x007)
- [0x008](#0x008)
- [0x009](#0x009)
- [0x00A](#0x00A)
- [0x00B](#0x00B)
- [0x00C](#0x00C)
- [0x00D](#0x00d)

### 0x002

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

    string name = r.ReadString();
    int u01 = r.ReadInt32();
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

    string name = r.ReadString();
}
```

### 0x004

```cs
void Read (GameBoxReader r)
{
    Node u01 = r.ReadNodeRef();
}
```

### 0x005

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

    string name = r.ReadString();
}
```

### 0x006

Undiscovered.

### 0x007

```cs
void Read (GameBoxReader r)
{
    int localPlayerClipEntIndex = r.ReadInt32();
}
```

### 0x008

```cs
void Read (GameBoxReader r)
{
    float u01 = r.ReadSingle(); // 0.2
}
```

### 0x009

```cs
void Read (GameBoxReader r)
{
    string u01 = r.ReadString();
}
```

### 0x00A

```cs
void Read (GameBoxReader r)
{
    bool stopWhenLeave = r.ReadBoolean();
}
```

### 0x00B

```cs
void Read (GameBoxReader r)
{
    bool u01 = r.ReadBoolean(); // 99% StopWhenRespawn
}
```

### 0x00C

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

### 0x00D

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

    string name = r.ReadString();

    bool stopWhenLeave = r.ReadBoolean();
    bool u03 = r.ReadBoolean();
    bool stopWhenRespawn = r.ReadBoolean();
    string u05 = r.ReadString();
    float u06 = r.ReadSingle();
    int localPlayerClipEntIndex = r.ReadInt32();
}
```
