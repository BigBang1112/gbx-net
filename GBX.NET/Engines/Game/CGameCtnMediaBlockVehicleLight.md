# CGameCtnMediaBlockVehicleLight (0x03133000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)
- [0x001 (target)](#0x001-target)

### 0x000

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
}
```

### 0x001 (target)

```cs
void Read (GameBoxReader r)
{
    int target = r.ReadInt32();
}
```
