# CGameCtnMediaBlockSound (0x030A7000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x001](#0x001)
- [0x002](#0x002)
- [0x003](#0x003)
- [0x004](#0x004)

### 0x001

```cs
void Read (GameBoxReader r)
{
    FileRef sound = r.ReadFileRef();

    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float volume = r.ReadSingle();
        float pan = r.ReadSingle();
    }
}
```

### 0x002

```cs
void Read (GameBoxReader r)
{
    int playCount = r.ReadInt32();
    bool isLooping = r.ReadBoolean();
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    int playCount = r.ReadInt32();
    bool isLooping = r.ReadBoolean();
    bool isMusic = r.ReadBoolean();

    if (version >= 1) // ManiaPlanet
    {
        bool stopWithClip = r.ReadBoolean();

        if (version >= 2)
        {
            bool audioToSpeech = r.ReadBoolean();
            int audioToSpeechTarget = r.ReadInt32();
        }
    }
}
```

### 0x004

```cs
void Read (GameBoxReader r)
{
    FileRef sound = r.ReadFileRef();
    int u01 = r.ReadInt32(); // 1
    
    int numKeys = r.ReadInt32();
    for (var i = 0; i < numKeys; i++)
    {
        float time = r.ReadSingle();
        float volume = r.ReadSingle();
        float u02 = r.ReadSingle(); // probably unused pan
        Vec3 position = r.ReadVec3();
    }
}
```
