# CGameCtnMediaBlockCameraGame (0x03084000)

### Inherits [CGameCtnMediaBlockCamera](CGameCtnMediaBlockCamera.md)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)
- [0x003](#0x003)
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
    EGameCam type = (EGameCam)r.ReadInt32();
    int target = r.ReadInt32();
}
```

#### Enums

```cs
enum EGameCam : int
{
    Behind,
    Close,
    Internal,
    Orbital
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    LookbackString gameCam = r.ReadLookbackString();
    int target = r.ReadInt32();
}
```

### 0x007

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    EGameCam type = (EGameCam)r.ReadInt32();

    // more unknown data with unknown length
    // you can read this section just fine until you reach 0xFACADE01
}
```

#### Enums

```cs
enum EGameCam : int
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