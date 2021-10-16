# CGameCtnMediaBlockTime (0x03085000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float timeValue = r.ReadSingle();
        float tangent = r.ReadSingle();
    }
}
```
