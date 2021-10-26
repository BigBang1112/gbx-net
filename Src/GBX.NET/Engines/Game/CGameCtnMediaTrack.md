# CGameCtnMediaTrack (0x03078000)

## Chunks

- [0x001](#0x001)
- [0x002](#0x002)
- [0x003](#0x003)
- [0x004](#0x004)
- [0x005](#0x005)

### 0x001

```cs
void Read (GameBoxReader r)
{
    string name = r.ReadString();
    int u01 = r.ReadInt32();

    int numBlocks = r.ReadInt32();
    for (var i = 0; i < numBlocks; i++)
        CGameCtnMediaBlock block = r.ReadNodeRef<CGameCtnMediaBlock>();

    int u02 = r.ReadInt32();
}
```

### 0x002

Represents `IsKeepPlaying` for **ESWC** tracks. This chunk should be removed or transfered to `0x005` in the new versions of ManiaPlanet.

```cs
void Read (GameBoxReader r)
{
    bool isKeepPlaying = r.ReadBoolean();
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

### 0x004

Represents `IsKeepPlaying` for **TMF** tracks. This chunk should be removed or transfered to `0x005` in the new versions of ManiaPlanet.

```cs
void Read (GameBoxReader r)
{
    bool isKeepPlaying = r.ReadBoolean();
}
```

### 0x005

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    bool isKeepPlaying = r.ReadBoolean();
    int u02 = rw.Int32(U02);
    bool isCycling = r.ReadBoolean();
    float u04 = r.ReadSingle();
    float u05 = r.ReadSingle();
}
```
