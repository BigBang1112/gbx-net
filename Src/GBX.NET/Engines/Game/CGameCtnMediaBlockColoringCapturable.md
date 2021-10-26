# CGameCtnMediaBlockColoringCapturable (0x0316C000)

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
        float gauge = r.ReadSingle();
        float emblem = r.ReadInt32();
    }
}
```
