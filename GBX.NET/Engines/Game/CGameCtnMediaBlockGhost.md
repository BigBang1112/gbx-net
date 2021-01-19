# CGameCtnMediaBlockGhost (0x030E5000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x001](#0x001)
- [0x002](#0x002)

### 0x001

```cs
void Read (GameBoxReader r)
{
    float start = r.ReadSingle();
    float end = r.ReadSingle();
    CGameCtnGhost ghostModel = r.ReadNodeRef<CGameCtnGhost>();
    float startOffset = r.ReadSingle();
}
```

### 0x002

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    
    if (version >= 3)
    {
        int numKeys = r.ReadInt32();
        for (var i = 0; i < numKeys; i++)
        {
            float time = r.ReadSingle();
            float u01 = r.ReadSingle();
        }
    }
    else
    {
        float start = r.ReadSingle();
        float end = r.ReadSingle();
    }

    CGameCtnGhost ghostModel = r.ReadNodeRef<CGameCtnGhost>();
    float startOffset = r.ReadSingle();
    bool noDamage = r.ReadBoolean();
    bool forceLight = r.ReadBoolean();
    bool forceHue = r.ReadBoolean();
}
```
