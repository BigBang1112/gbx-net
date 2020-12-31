# CGameCtnMediaBlockBloomHdr (0x03128000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x002](#0x002)

### 0x002

```cs
void Read(GameBoxReader r)
{
    int numKeys = r.ReadInt32();

    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float intensity = r.ReadSingle();
        float streaksIntensity = r.ReadSingle();
        float streaksAttenuation = r.ReadSingle();
    }
}
```