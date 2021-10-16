# CGameCtnMediaBlockDOF (0x03126000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)
- [0x002](#0x002)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for(var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float zFocus = r.ReadSingle();
        float lensSize = r.ReadSingle();
    }
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
        float zFocus = r.ReadSingle();
        float lensSize = r.ReadSingle();
        int u01 = r.ReadInt32();
    }
}
```

### 0x002

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for(var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float zFocus = r.ReadSingle();
        float lensSize = r.ReadSingle();
        int u01 = r.ReadInt32();
        float u02 = r.ReadSingle();
        float u03 = r.ReadSingle();
        float u04 = r.ReadSingle();
    }
}
```
