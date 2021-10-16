# CGameCtnGhost (0x03092000)

### Inherits [CGameGhost](CGameGhost)

## Chunks

- [0x000 - skippable (basic)](#0x000---skippable-basic)
- [0x005 - skippable (race time)](#0x005---skippable-race-time)
- [0x008 - skippable (respawns)](#0x008---skippable-respawns)
- [0x009 - skippable (light trail color)](#0x009---skippable-light-trail-color)
- [0x00A - skippable (stunt score)](#0x00A---skippable-stunt-score)
- [0x00B - skippable (checkpoint times)](#0x00B---skippable-checkpoint-times)
- [0x00C](#0x00C)
- [0x00E](#0x00E)
- [0x00F (ghost login)](#0x00F-ghost-login)
- [0x010](#0x010)
- [0x012](#0x012)
- [0x013 - skippable](#0x013---skippable)
- [0x014 - skippable](#0x014---skippable)
- [0x015 (vehicle)](#0x015-vehicle)
- [0x017 - skippable](#0x017---skippable)
- [0x018](#0x018)
- [0x019](#0x019-validation)
- [0x01C](#0x01C)
- [0x025 - skippable (validation)](#0x025---skippable-validation)

### 0x000 - skippable (basic)

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    Ident model = r.ReadIdent();
    Vec3 u01 = r.ReadVec3();

    int numSkinFiles = r.ReadInt32();
    for(var i = 0; i < numSkinFiles; i++)
        FileRef skinFile = r.ReadFileRef();
    
    bool hasBadges = r.ReadBoolean();
    if (hasBadges)
    {
        int u05 = r.ReadInt32();
        Vec3 u06 = r.ReadVec3();

        int numBadges = r.ReadInt32();
        for (var i = 0; i < numBadges; i++)
        {
            string badgeValue = r.ReadString();
            string badgeKey = r.ReadString();
        }

        int numBadges2 = r.ReadInt32();
        for (var i = 0; i < numBadges2; i++)
        {
            string badge = r.ReadString();
        }
    }

    string ghostNickname = r.ReadString();
    string ghostAvatarFile = r.ReadString();
    
    if (version >= 2)
    {
        string recordingContext = r.ReadString();
        
        if (version >= 4)
        {
            bool u03 = r.ReadBoolean();

            if (version >= 5)
            {
                CPlugEntRecordData recordData = r.ReadNodeRef<CPlugEntRecordData>();

                int num = r.ReadInt32();
                for (var i = 0; i < num; i++)
                    int u04 = r.ReadInt32();

                if (version >= 6)
                {
                    string ghostTrigram = r.ReadString();

                    if (version >= 7)
                        string hostZone = r.ReadString();
                }
            }
        }
    }
}
```

### 0x005 - skippable (race time)

```cs
void Read (GameBoxReader r)
{
    int raceTime = r.ReadInt32();
}
```

### 0x008 - skippable (respawns)

```cs
void Read (GameBoxReader r)
{
    int respawns = r.ReadInt32();
}
```

### 0x009 - skippable (light trail color)

```cs
void Read (GameBoxReader r)
{
    int lightTrailColor = r.ReadVec3();
}
```

### 0x00A - skippable (stunt score)

```cs
void Read (GameBoxReader r)
{
    int stuntScore = r.ReadInt32();
}
```

### 0x00B - skippable (checkpoint times)

```cs
void Read (GameBoxReader r)
{
    int numCheckpoints = r.ReadInt32();
    for(var i = 0; i < numCheckpoints; i++)
        long checkpointTime = r.ReadInt64(); // 64bit
}
```

### 0x00C

```cs
void Read (GameBoxReader r)
{
    int a = r.ReadInt32();
}
```

### 0x00E

```cs
void Read (GameBoxReader r)
{
    Id uid = r.ReadId();
}
```

### 0x00F (ghost login)

```cs
void Read (GameBoxReader r)
{
    string ghostLogin = r.ReadString();
}
```

### 0x010

```cs
void Read (GameBoxReader r)
{
    Id u01 = r.ReadId();
}
```

### 0x012

```cs
void Read (GameBoxReader r)
{
    int a = r.ReadInt32();
    long b = r.ReadInt64();
    long c = r.ReadInt64();
}
```

### 0x013 - skippable

```cs
void Read (GameBoxReader r)
{
    int a = r.ReadInt32();
    int b = r.ReadInt32();
}
```

### 0x014 - skippable

```cs
void Read (GameBoxReader r)
{
    int a = r.ReadInt32();
}
```

### 0x015 (vehicle)

```cs
void Read (GameBoxReader r)
{
    Id vehicle = r.ReadId();
}
```

### 0x017 - skippable

```cs
void Read (GameBoxReader r)
{
    int numSkinPackDescs = r.ReadInt32();
    for(var i = 0; i < numSkinPackDescs; i++)
        FileRef skinPackDesc = r.ReadFileRef();
    
    string ghostNickname = r.ReadString();
    string ghostAvatarName = r.ReadString();
}
```

### 0x018

```cs
void Read (GameBoxReader r)
{
    Ident a = r.ReadIdent();
}
```

### 0x019 (validation)

```cs
void Read (GameBoxReader r)
{
    int eventsDuration = r.ReadInt32();

    if (eventsDuration > 0)
    {
        uint u01 = r.ReadUInt32();

        int numControlNames = r.ReadInt32();
        for (var i = 0; i < numControlNames; i++)
            Id controlName = r.ReadId();

        int numEntries = r.ReadInt32();

        int u02 = r.ReadInt32();

        for (var i = 0; i < numEntries; i++)
        {
            int time = r.ReadInt32();
            byte controlNameIndex = r.ReadByte();
            uint data = r.ReadUInt32();
        }

        string validate_ExeVersion = r.ReadString();
        int validate_ExeChecksum = r.ReadInt32();
        int validate_OsKind = r.ReadInt32();
        int validate_CpuKind = r.ReadInt32();
        string validate_RaceSettings = r.ReadString();
        int u03 = r.ReadInt32();
    }
}
```

### 0x01C

```cs
void Read (GameBoxReader r)
{
    int a = r.ReadInt32();
    int b = r.ReadInt32();
    int c = r.ReadInt32();
    int d = r.ReadInt32();
    int e = r.ReadInt32();
    int f = r.ReadInt32();
    int g = r.ReadInt32();
    int h = r.ReadInt32();
}
```

### 0x025 - skippable (validation)

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    int eventsDuration = r.ReadInt32();

    uint u01 = r.ReadUInt32();

    int numControlNames = r.ReadInt32();
    for (var i = 0; i < numControlNames; i++)
        Id controlName = r.ReadId();

    int numEntries = r.ReadInt32();

    int u02 = r.ReadInt32();

    for (var i = 0; i < numEntries; i++)
    {
        int time = r.ReadInt32();
        byte controlNameIndex = r.ReadByte();
        uint data = r.ReadUInt32();
    }

    string validate_ExeVersion = r.ReadString();
    int validate_ExeChecksum = r.ReadInt32();
    int validate_OsKind = r.ReadInt32();
    int validate_CpuKind = r.ReadInt32();
    string validate_RaceSettings = r.ReadString();
    int u03 = r.ReadInt32();
    bool u04 = r.ReadBoolean();
}
```