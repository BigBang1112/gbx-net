# CGameCtnMediaBlockCameraGame (0x03084000)

### Inherits [CGameCtnMediaBlockCamera](CGameCtnMediaBlockCamera.md)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)
- 0x002
- [0x003](#0x003)
- 0x004
- [0x005](#0x005)
- 0x006
- [0x007](#0x007)

### 0x000

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    int u01 = r.ReadInt32();
}
```

### 0x001

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    int type = r.ReadInt32();
    int target = r.ReadInt32();
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    Id gameCam = r.ReadId();
    int target = r.ReadInt32();
}
```

### 0x005

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    Id gameCam = r.ReadId();
    int target = r.ReadInt32();

    r.ReadTillFacade();
    // More unknown data, possibly helicopter camera transform?
}
```

### 0x007

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    EGameCam gameCam = (EGameCam)r.ReadInt32();

    r.ReadTillFacade();
    // More unknown data, possibly helicopter camera transform?
    // 17 ints, sometimes 19
}
```

#### Enums

```cs
public enum EGameCam : int
{
    Default,
    Internal,
    External,
    Helico,
    Free,
    Spectator,
    External_2
}
```