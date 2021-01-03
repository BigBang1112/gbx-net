# CGameCtnMediaBlockFxBloom (0x03083000)

### Inherits [CGameCtnMediaBlockFx](CGameCtnMediaBlockFx.md)

## Chunks

- [0x001](#0x001)

### 0x001

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float intensity = r.ReadSingle();
        float sensitivity = r.ReadSingle();
    }
}
```
