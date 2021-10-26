# CGameCtnMediaBlockFxBlurDepth (0x03081000)

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
        float lensSize = r.ReadSingle();
        bool forceFocus = r.ReadBoolean();
        float focusZ = r.ReadSingle();
    }
}
```
