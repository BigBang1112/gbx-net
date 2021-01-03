# CGameCtnMediaBlockImage (0x030A5000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)

### 0x000

```cs
void Read (GameBoxReader r)
{
    CControlEffectSimi effect = r.ReadNodeRef<CControlEffectSimi>();
    FileRef image = r.ReadFileRef();
}
```

### 0x001

```cs
void Read (GameBoxReader r)
{
    float u01 = r.ReadSingle(); // 0.2
}
```
