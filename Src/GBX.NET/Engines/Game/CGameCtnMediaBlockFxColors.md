# CGameCtnMediaBlockFxColors (0x03080000)

### Inherits [CGameCtnMediaBlockFx](CGameCtnMediaBlockFx.md)

## Chunks

- [0x003](#0x003)

### 0x003

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float intensity = r.ReadSingle();
        float blendZ = r.ReadSingle();
        float distance = r.ReadSingle();
        float farDistance = r.ReadSingle();
        float inverse = r.ReadSingle();
        float hue = r.ReadSingle();
        float saturation = r.ReadSingle(); // from center
        float brightness = r.ReadSingle(); // from center
        float contrast = r.ReadSingle(); // from center
        Vec3 rgb = r.ReadVec3();
        float u01 = r.ReadSingle();
        float u02 = r.ReadSingle();
        float u03 = r.ReadSingle();
        float u04 = r.ReadSingle();
        float farInverse = r.ReadSingle();
        float farHue = r.ReadSingle();
        float farSaturation = r.ReadSingle(); // from center
        float farBrightness = r.ReadSingle(); // from center
        float farContrast = r.ReadSingle(); // from center
        Vec3 farRGB = r.ReadVec3();
        float farU01 = r.ReadSingle();
        float farU02 = r.ReadSingle();
        float farU03 = r.ReadSingle();
        float farU04 = r.ReadSingle();
    }
}
```
