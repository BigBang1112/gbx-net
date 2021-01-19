# CGameCtnMediaBlockToneMapping (0x03127000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x004](#0x004)

### 0x004

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float exposure = r.ReadSingle();
        float maxHDR = r.ReadSingle();
        float lightTrailScale = r.ReadSingle();
        int u01 = r.ReadInt32();
    }
}
```
