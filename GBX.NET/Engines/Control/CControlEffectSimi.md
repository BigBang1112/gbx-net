# CControlEffectSimi (0x07010000)

## Chunks

- [0x002](#0x002)
- [0x004](#0x004)
- [0x005](#0x005)

### 0x002

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float x = r.ReadSingle();
        float y = r.ReadSingle();
        float rot = r.ReadSingle();
        float scaleX = r.ReadSingle();
        float scaleY = r.ReadSingle();
        float opacity = r.ReadSingle();
        float depth = r.ReadSingle();
    });

    bool centered = r.ReadBoolean();
}
```

### 0x004

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float x = r.ReadSingle();
        float y = r.ReadSingle();
        float rot = r.ReadSingle();
        float scaleX = r.ReadSingle();
        float scaleY = r.ReadSingle();
        float opacity = r.ReadSingle();
        float depth = r.ReadSingle();
        float u01 = r.ReadSingle();
        float isContinousEffect = r.ReadSingle();
        float u02 = r.ReadSingle();
        float u03 = r.ReadSingle();
    });

    bool centered = r.ReadBoolean();
    int colorBlendMode = r.ReadInt32();
    bool isContinousEffect = r.ReadBoolean();
}
```

### 0x005

```cs
void Read (GameBoxReader r)
{
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float x = r.ReadSingle();
        float y = r.ReadSingle();
        float rot = r.ReadSingle();
        float scaleX = r.ReadSingle();
        float scaleY = r.ReadSingle();
        float opacity = r.ReadSingle();
        float depth = r.ReadSingle();
        float u01 = r.ReadSingle();
        float isContinousEffect = r.ReadSingle();
        float u02 = r.ReadSingle();
        float u03 = r.ReadSingle();
    });

    bool centered = r.ReadBoolean();
    int colorBlendMode = r.ReadInt32();
    bool isContinousEffect = r.ReadBoolean();
    bool isInterpolated = r.ReadBoolean();
}
```
