# CGameCtnMediaBlockColorGrading (0x03186000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)

### 0x000

```cs
void Read (GameBoxReader r)
{
    FileRef image = r.ReadFileRef();
}
```

### 0x001

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for(var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float intensity = r.ReadSingle();
    }
}
```
