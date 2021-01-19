# CGameCtnMediaBlockText (0x030A8000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x001 (text)](#0x001-text)
- [0x002 (color)](#0x002-color)
- [0x003](#0x003)

### 0x001 (text)

```cs
void Read (GameBoxReader r)
{
    string text = r.ReadString();
    CControlEffectSimi effect = r.ReadNodeRef<CControlEffectSimi>();
}
```

### 0x002 (color)

```cs
void Read (GameBoxReader r)
{
    Vec3 color = r.ReadVec3();
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    float u01 = r.ReadSingle(); // 0.2
}
```
