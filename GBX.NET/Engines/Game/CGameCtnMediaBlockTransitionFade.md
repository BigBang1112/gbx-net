# CGameCtnMediaBlockTransitionFade (0x030AB000)

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
        float opacity = r.ReadSingle();
    }

    Vec3 color = r.ReadVec3();
    float u01 = r.ReadSingle();
}
```
