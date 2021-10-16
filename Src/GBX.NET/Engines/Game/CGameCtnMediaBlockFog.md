# CGameCtnMediaBlockFog (0x03199000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float intensity = r.ReadSingle();
        float skyIntensity = r.ReadSingle();
        float distance = r.ReadSingle();

        if (version >= 1)
        {
            float coefficient = r.ReadSingle();
            Vec3 color = r.ReadVec3();

            if (version >= 2)
            {
                float cloudsOpacity = r.ReadSingle();
                float cloudsSpeed = r.ReadSingle();
            }
        }
    });
}
```
