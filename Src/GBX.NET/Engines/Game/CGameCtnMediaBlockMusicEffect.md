# CGameCtnMediaBlockMusicEffect (0x030A6000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float musicVolume = r.ReadSingle();
    }
}
```

### 0x001

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float musicVolume = r.ReadSingle();
        float soundVolume = r.ReadSingle();
    }
}
```
