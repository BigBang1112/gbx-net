# CGameCtnMediaBlockColoringBase (0x03172000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    int u01 = r.ReadInt32();

    int numKeys = r.ReadInt32();
    for(var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float hue = r.ReadSingle();
        float intensity = r.ReadSingle();
        short u02 = r.ReadInt16();
    }
    
    int baseIndex = r.ReadInt32();
}
```
