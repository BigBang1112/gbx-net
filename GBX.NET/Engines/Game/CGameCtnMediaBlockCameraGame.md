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
    LookbackString gameCam = r.ReadLookbackString();
    int target = r.ReadInt32();
}
```

### 0x007

```cs
void Read (GameBoxReader r)
{
    
}
```
