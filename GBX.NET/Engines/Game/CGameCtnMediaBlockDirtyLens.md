# CGameCtnMediaBlockDirtyLens (0x03165000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numKeys = r.ReadInt32();
    for(var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float intensity = r.ReadSingle();
    }
}
```
