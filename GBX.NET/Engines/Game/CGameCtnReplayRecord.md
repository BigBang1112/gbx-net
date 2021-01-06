# CGameCtnReplayRecord (0x03093000)

## Chunks

- [0x000 - header chunk (basic)](#0x000---header-chunk-basic)
- [0x001 - header chunk (xml)](#0x001---header-chunk-xml)
- [0x002 - header chunk (author)](#0x002---header-chunk-author)
- [0x002 (track)](#0x002-track)
- [0x003](#0x003)
- [0x004](#0x004)
- [0x00C](#0x00c)
- [0x00D](#0x00d)
- [0x00E](#0x00e)
- [0x010](#0x010)
- [0x011](#0x011)
- [0x014](#0x014)
- [0x015](#0x015)
- [0x024](#0x024)

### 0x000 - header chunk (basic)

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    if (version >= 2)
    {
        Ident mapInfo = r.ReadIdent();
        int time = r.ReadInt32();
        string nickname = r.ReadString();

        if (version >= 6)
        {
            string driverLogin = r.ReadString();

            if (version >= 8)
            {
                byte u01 = r.ReadByte();
                Id titleID = r.ReadId();
            }
        }
    }
}
```

### 0x001 - header chunk (xml)

```cs
void Read (GameBoxReader r)
{
    string xml = r.ReadString();
}
```

### 0x002 - header chunk (author)

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    int authorVersion = r.ReadInt32();
    string authorLogin = r.ReadString();
    string authorNickname = r.ReadString();
    string authorZone = r.ReadString();
    string authorExtraInfo = r.ReadString();
}
```

### 0x002 (track)

```cs
void Read (GameBoxReader r)
{
    int trackSize = r.ReadInt32();
    byte[] trackGbx = r.ReadBytes(trackSize);
}
```

### 0x003

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();

    int numControlNames = r.ReadInt32();
    for (var i = 0; i < numControlNames; i++)
    {
        r.ReadInt32();
        r.ReadInt32();
        string controlName = r.ReadString();
    }

    var numControlEntries = r.ReadInt32() - 1;
    for (var i = 0; i < numControlEntries; i++)
    {
        r.ReadInt32();
        r.ReadInt32();
        r.ReadInt32();
    }

    r.ReadInt32();
}
```

### 0x004

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();

    int numGhosts = r.ReadInt32();
    for (var i = 0; i < numGhosts; i++)
        CGameCtnGhost ghost = r.ReadNodeRef<CGameCtnGhost>();

    int u03 = r.ReadInt32();
    int u04 = r.ReadInt32();
}
```

### 0x00C

```cs
void Read (GameBoxReader r)
{
    object u01 = r.ReadNodeRef();
}
```

### 0x00D

```cs
void Read (GameBoxReader r)
{
    var u01 = r.ReadInt32();
    if (u01 != 0)
    {
        int u02 = r.ReadInt32();
        
        int numControlNames = r.ReadInt32();
        for (var i = 0; i < numControlNames; i++)
            Id controlName = r.ReadId();

        int num = r.ReadInt32();
        int u03 = r.ReadInt32();

        for (var i = 0; i < num; i++)
        {
            r.ReadInt32();
            r.ReadInt32();
            r.ReadByte();
        }
    }
}
```

### 0x00E

```cs
void Read (GameBoxReader r)
{
    CCtnMediaBlockEventTrackMania events = r.ReadNodeRef<CCtnMediaBlockEventTrackMania>();
}
```

### 0x010

Undiscovered.

### 0x011

Undiscovered.

### 0x014

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numGhosts = r.ReadInt32();
    for (var i = 0; i < numGhosts; i++)
        CGameCtnGhost ghost = r.ReadNodeRef<CGameCtnGhost>();

    int u01 = r.ReadInt32();

    int num = r.ReadInt32();
    for (var i = 0; i < num; i++)
        long extra = r.ReadInt64();
}
```

### 0x015

```cs
void Read (GameBoxReader r)
{
    CGameCtnMediaClip clip = r.ReadNodeRef<CGameCtnMediaClip>();
}
```

### 0x024

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();
    CPlugEntRecordData recordData = r.ReadNodeRef<CPlugEntRecordData>();
}
```
